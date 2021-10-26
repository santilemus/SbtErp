using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.Controllers;
using DevExpress.Persistent.Base;
using SBT.Apps.Inventario.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;
using SBT.Apps.Compra.Module.BusinessObjects;

namespace SBT.Apps.Inventario.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO InventarioMovimiento y se utiliza para actualizar el inventario, kardex y lotes
    /// </summary>
    public class vcInventarioMovimiento : ViewControllerBase
    {
        private SimpleAction saIngresoDesdeOrdenCompra;
        private SimpleAction saIngresoDesdeFactura;
        public vcInventarioMovimiento(): base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            ObjectSpace.Committing += ObjectSpace_Committing;
            saIngresoDesdeOrdenCompra.Execute += saIngresoDesdeOrdenCompra_Execute;
            saIngresoDesdeFactura.Execute += saIngresoDesdeFactura_Execute;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            ObjectSpace.Committing -= ObjectSpace_Committing;
            saIngresoDesdeOrdenCompra.Execute -= saIngresoDesdeOrdenCompra_Execute;
            saIngresoDesdeFactura.Execute -= saIngresoDesdeFactura_Execute;
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Inventario.Module.BusinessObjects.InventarioMovimiento);
            TargetViewType = ViewType.Any;
            saIngresoDesdeOrdenCompra = new SimpleAction(this, "saIngresoDesdeOrdenCompra", PredefinedCategory.RecordEdit);
            saIngresoDesdeOrdenCompra.Caption = "Desde Orden Compra";
            saIngresoDesdeOrdenCompra.ToolTip = "Generar el detalle del ingreso desde la orden de compra";
            saIngresoDesdeOrdenCompra.ImageName = "";
            saIngresoDesdeOrdenCompra.TargetObjectsCriteria = "[OrdenCompra] Is Not Null && [TipoMovimiento.Codigo] In ('201, '202', '203') && [Detalles][].Count() == 0";
            saIngresoDesdeFactura = new SimpleAction(this, "saIngresoDesdeFactura", PredefinedCategory.RecordEdit);
            saIngresoDesdeFactura.Caption = "Desde Factura";
            saIngresoDesdeFactura.ToolTip = "Generar el detalle del ingreso desde la factura de compra";
            saIngresoDesdeFactura.ImageName = "";
            saIngresoDesdeFactura.TargetObjectsCriteria = "[Factura] Is Not Null && [TipoMovimiento.Codigo] In ('201', '202', '203') && [Detalles][].Count() == 0";
        }

        protected override void Dispose(bool disposing)
        {
            saIngresoDesdeOrdenCompra.Dispose();
            saIngresoDesdeFactura.Dispose();
            base.Dispose(disposing);
        }

        private void ObjectSpace_ObjectChanged(object Sender, ObjectChangedEventArgs e)
        {

        }

        private void ObjectSpace_Committing(object Sender, CancelEventArgs﻿ e)
        {
            if (View.CurrentObject == null)
                return;
            if (((InventarioMovimiento)View.CurrentObject).TipoMovimiento == null)
            {
                MostrarError($"No se encontró el Tipo de Movimiento de Inventario. No se puede actualizar el Inventario");
                e.Cancel = true;
                return;
            }
            var items = ObjectSpace.ModifiedObjects.Cast<object>().Where(y => y.GetType() == typeof(SBT.Apps.Inventario.Module.BusinessObjects.InventarioMovimientoDetalle)).
                                                                                     Cast<InventarioMovimientoDetalle>();
            foreach (var fItem in items)
            {
                if (fItem.Producto.Categoria.Clasificacion == EClasificacion.Servicios || fItem.Producto.Categoria.Clasificacion == EClasificacion.Intangible ||
                    fItem.Producto.Categoria.Clasificacion == EClasificacion.Otros)
                    continue;
                DoActualizarInventario(fItem);
                DoActualizarKardex(fItem);
                if (fItem.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.PEPS ||
                    fItem.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.UEPS)
                    DoActualizarLote(fItem);
            }
        }


        private void DoActualizarInventario(InventarioMovimientoDetalle item)
        {
            Inventario.Module.BusinessObjects.Inventario inventarioItem = ObjectSpace.FindObject<Inventario.Module.BusinessObjects.Inventario>(
                            CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ? && [TipoMovimiento.Oid] == ?",
                            item.Movimiento.Bodega.Oid, item.Producto.Oid, item.Movimiento.TipoMovimiento.Oid));
            if (inventarioItem == null)
            {
                inventarioItem = ObjectSpace.CreateObject<Inventario.Module.BusinessObjects.Inventario>();
                inventarioItem.Bodega = item.Movimiento.Bodega;
                inventarioItem.Producto = item.Producto;
                inventarioItem.TipoMovimiento = item.Movimiento.TipoMovimiento;
                inventarioItem.Cantidad = 0.0m;
            }
            if (ObjectSpace.IsNewObject(item))
                inventarioItem.Cantidad += item.Unidades;
            else
            {
                decimal cantidadMes = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(InventarioMovimientoDetalle), CriteriaOperator.Parse("Sum([Unidades])"),
                     CriteriaOperator.Parse("GetMonth([Venta.Fecha]) == ? && GetYear([Venta.Fecha]) == ? && [Producto.Oid] == ? && [Bodega.Oid] == ? && [Oid] != ?",
                     item.Movimiento.Fecha.Month, item.Movimiento.Fecha.Year, item.Producto.Oid, item.Movimiento.Oid, item.Oid)));
                // Si la fila fue borrada, la nueva cantidad (de salidas por venta) es la cantidad acumulada del mes, sin considerar al item [Oid] != item.Oid
                cantidadMes += item.IsDeleted ? 0 : item.Unidades;
                inventarioItem.Cantidad = cantidadMes;
            }
            inventarioItem.Save();
        }

        private void DoActualizarKardex(InventarioMovimientoDetalle item)
        {
            Kardex kardexItem = ObjectSpace.FindObject<Kardex>(CriteriaOperator.Parse("[Producto.Oid] == ? && [TipoMovimiento] == ? && [Fecha] = ? && [Referencia] == ?",
                                item.Producto.Oid, item.Movimiento.TipoMovimiento.Oid, item.Movimiento.Fecha, item.Oid));
            if (kardexItem == null)
            {
                kardexItem = ObjectSpace.CreateObject<Kardex>();
                kardexItem.Bodega = item.Movimiento.Bodega;
                kardexItem.Producto = item.Producto;
                kardexItem.TipoMovimiento = item.Movimiento.TipoMovimiento;
            }
            kardexItem.Cantidad = item.Unidades;
            if (item.OrdenCompraItem != null)
                kardexItem.CostoUnidad = item.OrdenCompraItem.PrecioUnidad;
            kardexItem.Referencia = item.Oid;
            kardexItem.Save();
        }

        private void DoActualizarLote(InventarioMovimientoDetalle item)
        {
            if (string.IsNullOrEmpty(item.CodigoLote))
                return;
            InventarioLote lote = ObjectSpace.FindObject<InventarioLote>(new BinaryOperator("CodigoLote", item.CodigoLote)); 
            if (lote == null)
            {
                lote = ObjectSpace.CreateObject<InventarioLote>();
                lote.IngresoDetalle = item;
                // compra o importacion o consignacion
                if (item.Movimiento.TipoMovimiento.Codigo == "201" || item.Movimiento.TipoMovimiento.Codigo == "202" || item.Movimiento.TipoMovimiento.Codigo == "203")
                {
                    lote.CodigoLote = item.CodigoLote;
                    lote.FechaFabricacion = item.FechaFabricacion;
                    lote.FechaVence = item.FechaVence;
                }
            }

            if (item.Movimiento.TipoMovimiento.Operacion == ETipoOperacionInventario.Entrada || 
                item.Movimiento.TipoMovimiento.Operacion == ETipoOperacionInventario.Inicial)
            {
                if (ObjectSpace.IsNewObject(item) || !item.IsDeleted)
                    lote.Entrada = item.Unidades;
                else if (item.IsDeleted)
                    lote.Entrada -= item.Unidades;
            }
            else
            {
                if (ObjectSpace.IsNewObject(item) || !item.IsDeleted)
                    lote.Salida = item.Unidades;
                else if (item.IsDeleted)
                    lote.Salida -= item.Unidades;
            }
            lote.Save();
        }

        private void saIngresoDesdeOrdenCompra_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var docInventario = ((InventarioMovimiento)View.CurrentObject);
            if (docInventario.OrdenCompra == null)
            {
                MostrarInformacion("La orden de compra no tiene detalles para generar el ingreso a bodega");
                return;
            }
            foreach(OrdenCompraDetalle item in docInventario.OrdenCompra.Detalles)
            {
                InventarioMovimientoDetalle ingresoDetalle = ObjectSpace.CreateObject<InventarioMovimientoDetalle>();
                ingresoDetalle.OrdenCompraItem = item;
                ingresoDetalle.Producto = item.Producto;
                ingresoDetalle.UnidadesTeorica = item.Unidades;
                docInventario.Detalles.Add(ingresoDetalle);
            }
        }

        private void saIngresoDesdeFactura_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var docInventario = ((InventarioMovimiento)View.CurrentObject);
            if (docInventario.Factura == null)
            {
                MostrarInformacion("La factura no tiene detalles para generar el ingreso a bodega");
                return;
            }
            foreach(CompraFacturaDetalle item in docInventario.Factura.Detalles)
            {
                InventarioMovimientoDetalle ingresoDetalle = ObjectSpace.CreateObject<InventarioMovimientoDetalle>();
                ingresoDetalle.FacturaItem = item;
                ingresoDetalle.Producto = item.Producto;
                ingresoDetalle.UnidadesTeorica = item.Unidades;
                // faltan datos: codigoLote, fechaFabricacion, fechaVence, deben ir en el detalle de la factura de compra para poderlos asignar
                docInventario.Detalles.Add(ingresoDetalle);
            }
        }
    }
}
