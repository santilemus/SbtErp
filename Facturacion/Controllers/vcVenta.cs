using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

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
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.Committing += ObjectSpace_Committing;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            ObjectSpace.Committing -= ObjectSpace_Committing;
        }

        /// <summary>
        /// Lanzar mensaje con advertencia que no existe un control de correlativos valido para el documento, y no esperar
        /// a que se ejecuten las validaciones
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_ObjectChanged(object Sender, ObjectChangedEventArgs e)
        {
            // cuando cambia la propiedad AutorizacionCorrelativo
            if (View.CurrentObject == e.Object && e.PropertyName == "AutorizacionCorrelativo" && ObjectSpace.IsNewObject(View.CurrentObject))
            //(ObjectSpace.IsNewObject(View.CurrentObject) || ObjectSpace.IsModified) && e.OldValue != e.NewValue)
            {
                if (e.NewValue == null)
                {
                    MostrarError($"No se encontró la autorización de correlativos para {((Venta)View.CurrentObject).TipoFactura.Nombre}. No conoce el número de documento");
                    return;
                }
                AutorizacionDocumento aud = (AutorizacionDocumento)e.NewValue;
                int noFact = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate<AutorizacionDocumento>(CriteriaOperator.Parse("max([NoFactura])"),
                             CriteriaOperator.Parse("AutorizacionCorrelativo.Oid == ?", aud.Oid)));
                if (noFact >= aud.NoDesde && noFact < aud.NoHasta)
                    ((Venta)View.CurrentObject).NoFactura = noFact + 1;
                else
                    MostrarError($"No hay autorización de correlativo disponible para {((Venta)View.CurrentObject).TipoFactura.Nombre}");
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
            /// pendiente de reemplazar 1 por el tipo de movimiento correspondiente
            tipoMovimiento = ObjectSpace.GetObjectByKey<InventarioTipoMovimiento>(1);

            System.Collections.IList items = ObjectSpace.ModifiedObjects;
            foreach (VentaDetalle item in items)
            {
                if (item.GetType() != typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle))
                    continue;
                DoActualizarInventario(item);
                DoActualizarKardex(item);
                if (item.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.PEPS ||
                    item.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.UEPS)
                    DoActualizarLote(item);
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
        /// <param name="item">El item del detalle de la venta que esta siendo procesado</param>
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
            }
            if (ObjectSpace.IsNewObject(item))
                inventarioItem.Cantidad = item.Cantidad;
            else
            {
                decimal cantidadMes = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(VentaDetalle), CriteriaOperator.Parse("Sum([Cantidad])"),
                     CriteriaOperator.Parse("GetMonth([Venta.Fecha]) == ? && GetYear([Venta.Fecha]) == ? && [Producto.Oid] == ? && [Bodega.Oid] == ? && [Oid] != ?",
                     item.Venta.Fecha.Month, item.Venta.Fecha.Year, item.Producto.Oid, item.Bodega.Oid, item.Oid)));
                // Si la fila fue borrada, la nueva cantidad (de salidas por venta) es la cantidad acumulada del mes, sin considerar al item [Oid] != item.Oid
                cantidadMes += ((VentaDetalle)item).IsDeleted ? 0 : item.Cantidad;
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
        /// <param name="item"></param>
        private void DoActualizarKardex(VentaDetalle item)
        {
            Kardex kardexItem = ObjectSpace.FindObject<Kardex>(
                CriteriaOperator.Parse("[Producto.Oid] == ? && [TipoMovimiento] == ? && [Fecha] = ? && [Referencia] == ?",
                item.Producto.Oid, tipoMovimiento.Oid, item.Venta.Fecha, item.Venta.Oid));
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
            kardexItem.Save();
        }

        private void DoActualizarLote(VentaDetalle item)
        {
            if (item.Lote != null)
            {
                Producto.Module.BusinessObjects.ProductoLote lote = ObjectSpace.GetObjectByKey<Producto.Module.BusinessObjects.ProductoLote>(item.Lote.Oid);
                if (lote == null)
                    return;
                if (ObjectSpace.IsNewObject(item))
                {
                    lote.Salida += item.Cantidad;
                    lote.Costo = item.Costo;
                }
                else
                {
                    if (item.IsDeleted)
                        lote.Salida -= item.Cantidad;

                }

                lote.Save();
            }
        }

    }
}
