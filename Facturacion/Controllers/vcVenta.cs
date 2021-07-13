using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Xpo;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO Ventas
    /// </summary>
    /// <remarks>
    /// Para el evento ObjectChanged mas info aqui: https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.IObjectSpace.ObjectChanged
    /// El objetivo es mostrar mensajes proactivos cuando cambia la propiedad AutorizacionCorrelativo. Calcular el NoFactura
    /// o mostrar mensaje de error si se han emitido todos los documentos autorizados
    /// </remarks>
    public class vcVenta : ViewControllerBase
    {
        InventarioTipoMovimiento tipoMovimiento;
        public vcVenta() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            TargetViewType = ViewType.Any;
            TargetViewNesting = Nesting.Root;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            //newObjectController = Frame.GetController<NewObjectViewController>();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.Committing += ObjectSpace_Committing;
        }

        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            ObjectSpace.Committing -= ObjectSpace_Committing;
            base.OnDeactivated();
        }

        /// <summary>
        /// Lanzar mensaje con advertencia que no existe un control de correlativos valido para el documento, y no esperar
        /// a que se ejecuten las validaciones
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_ObjectChanged(object Sender, ObjectChangedEventArgs e)
        {
            if (View == null || View.CurrentObject == null || e.Object == null)
                return;
            // cuando cambia la propiedad AutorizacionCorrelativo
            if (View.CurrentObject == e.Object && e.PropertyName == "AutorizacionCorrelativo" && ObjectSpace.IsNewObject(View.CurrentObject))
            //(ObjectSpace.IsNewObject(View.CurrentObject) || ObjectSpace.IsModified) && e.OldValue != e.NewValue)
            {
                if (e.NewValue == null)
                {
                    ((Venta)View.CurrentObject).NoFactura = null;
                    MostrarError($"No se encontró la autorización de correlativos para {((Venta)View.CurrentObject).TipoFactura.Nombre}. No conoce el número de documento");
                    return;
                }
                AutorizacionDocumento aud = (AutorizacionDocumento)e.NewValue;
                int noFact = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate<Venta>(CriteriaOperator.Parse("max([NoFactura])"),
                             CriteriaOperator.Parse("Oid == ?", aud.Oid))) + 1;
                if (noFact >= aud.NoDesde && noFact < aud.NoHasta)
                    ((Venta)View.CurrentObject).NoFactura = noFact;
                else
                {
                    ((Venta)View.CurrentObject).NoFactura = null;
                    MostrarError($"No hay autorización de correlativo disponible para {((Venta)View.CurrentObject).TipoFactura.Nombre}");
                }
            }

            // agregar aqui si se quiere procesar otros eventos
        }

        /// <summary>
        /// realizar la actualizacion del inventario, el kardex y el lote cuando el metodo de costeo es PEPS o UEPS
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_Committing(object Sender, CancelEventArgs﻿ e)
        {
            System.Collections.IList items = ObjectSpace.ModifiedObjects;
            if (items.Count == 1 && items.GetType() != typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle))
                return;

            if (items.Count > 0)
            {
                /// 301 es el Codigo del Tipo de Movimiento de Inventario que corresponde a facturacion
                tipoMovimiento = ObjectSpace.FindObject<InventarioTipoMovimiento>(CriteriaOperator.And(new BinaryOperator("Codigo", "301"), new BinaryOperator("Activo", true)));
                if (tipoMovimiento == null)
                {
                    MostrarError($"No se encontró Tipo de Movimiento de Inventario con código 301 que debe corresponder a Facturación, el Inventario no se puede actualizar ");
                    e.Cancel = true;
                    return;
                }

                foreach (object item in items)
                {
                    if (item.GetType() != typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle))
                        continue;
                    VentaDetalle fItem = (VentaDetalle)item;
                    if (fItem.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Servicios ||
                        fItem.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Intangible ||
                        fItem.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Otros)
                        continue;
                    DoActualizarInventario(fItem);
                    DoActualizarKardex(fItem);
                    if (fItem.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.PEPS ||
                        fItem.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.UEPS)
                        DoActualizarLote(fItem);
                }
            }
        }

        /// <summary>
        /// actualizar el inventario por cada linea del detalle del documento de venta. 
        /// El acumulado de las salidas por venta en el inventario se obtiene al sumar las cantidades vendida en el mes, 
        /// excepto para el detalle de venta que esta siendo procesado. Cuando es modificacion se suma item.Cantidad al
        /// al acumulado del mes.
        /// Cuando se ha eliminado el item de la venta, considerar solo la cantidad acumulada del mes como  se meciono
        /// antes, tiene el mismo efecto que restar de la salida la cantidad del item eliminado del mes
        /// </summary>
        /// <param name="item">La linea de detalle de la factura (instancia del BO VentaDetalle)</param>
        private void DoActualizarInventario(VentaDetalle item)
        {
            Inventario.Module.BusinessObjects.Inventario inventarioItem = ObjectSpace.FindObject<Inventario.Module.BusinessObjects.Inventario>(
                CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ? && [TipoMovimiento.Oid] == ?", item.Bodega.Oid, item.Producto.Oid, tipoMovimiento.Oid));
            if (inventarioItem == null)
            {
                inventarioItem = ObjectSpace.CreateObject<Inventario.Module.BusinessObjects.Inventario>();
                inventarioItem.Bodega = item.Bodega;
                inventarioItem.Producto = item.Producto;
                inventarioItem.TipoMovimiento = tipoMovimiento;
                inventarioItem.Cantidad = 0.0m;
            }
            if (ObjectSpace.IsNewObject(item))
                inventarioItem.Cantidad += item.Cantidad;
            else
            {
                decimal cantidadMes = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(VentaDetalle), CriteriaOperator.Parse("Sum([Cantidad])"),
                     CriteriaOperator.Parse("GetMonth([Venta.Fecha]) == ? && GetYear([Venta.Fecha]) == ? && [Producto.Oid] == ? && [Bodega.Oid] == ? && [Oid] != ?",
                     item.Venta.Fecha.Month, item.Venta.Fecha.Year, item.Producto.Oid, item.Bodega.Oid, item.Oid)));
                // Si la fila fue borrada, la nueva cantidad (de salidas por venta) es la cantidad acumulada del mes, sin considerar al item [Oid] != item.Oid
                cantidadMes += item.IsDeleted ? 0 : item.Cantidad;
                inventarioItem.Cantidad = cantidadMes;
            }
            inventarioItem.Save();
        }

        /// <summary>
        /// actualizar el kardex por cada linea del detalle del documento de venta. 
        /// Se verifica que el kardex no existe a traves del Producto, TipoMovimiento, Fecha y Referencia
        /// Cuando se ha eliminado el item de la venta, considerar solo la cantidad acumulada del mes como  se meciono
        /// antes, tiene el mismo efecto que restar de la salida la cantidad del item eliminado del mes
        /// </summary>
        /// <param name="item">La linea de detalle de la factura (instancia del BO VentaDetalle)</param>
        /// <remarks>
        /// Evaluar en las pruebas porque cuando pase por aqui es posible que item.Oid aun no tenga valor, en cuyo caso 
        /// tendremos que usar otro datos, por ejemplo item.Venta.Oid
        /// </remarks>
        private void DoActualizarKardex(VentaDetalle item)
        {
            Kardex kardexItem = ObjectSpace.FindObject<Kardex>(
                CriteriaOperator.Parse("[Producto.Oid] == ? && [TipoMovimiento] == ? && [Fecha] = ? && [Referencia] == ?",
                item.Producto.Oid, tipoMovimiento.Oid, item.Venta.Fecha, item.Oid));
            if (kardexItem == null)
            {
                kardexItem = ObjectSpace.CreateObject<Kardex>();
                kardexItem.Bodega = item.Bodega;
                kardexItem.Producto = item.Producto;
                kardexItem.TipoMovimiento = tipoMovimiento;
            }
            kardexItem.Cantidad = item.Cantidad;
            kardexItem.CostoUnidad = item.Costo;
            kardexItem.PrecioUnidad = item.PrecioUnidad;
            kardexItem.Referencia = item.Oid;
            kardexItem.Save();
        }

        /// <summary>
        /// Actualizar las salidas del lote cuando el metodo de costeo del inventario es PEPS o UEPS
        /// </summary>
        /// <param name="item">La linea de detalle de la factura (instancia del BO VentaDetalle)</param>
        private void DoActualizarLote(VentaDetalle item)
        {
            if (item.Lote != null)
            {
                InventarioLote lote = ObjectSpace.GetObjectByKey<InventarioLote>(item.Lote.Oid);
                if (lote == null)
                    return;
                if (ObjectSpace.IsNewObject(item))
                {
                    if (tipoMovimiento.Operacion == ETipoOperacionInventario.Salida)
                        lote.Salida += item.Cantidad;
                    else
                        lote.Entrada += item.Cantidad;
                    lote.Costo = item.Costo;
                }
                else
                {
                    if (item.IsDeleted)
                    {
                        if (tipoMovimiento.Operacion == ETipoOperacionInventario.Salida)
                            lote.Salida -= item.Cantidad;
                        else
                            lote.Entrada -= item.Cantidad;
                    }
                }
                lote.Save();
            }
        }

        private void ExportSalesIvaF07(DateTime ADesde, DateTime AHasta)
        {
        }

    }
}
