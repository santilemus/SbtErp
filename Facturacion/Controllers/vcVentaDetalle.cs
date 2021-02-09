using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.Controllers;
using System;
using System.Linq;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    public class vcVentaDetalle : ViewControllerBase
    {
        public vcVentaDetalle()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle);
            TargetViewType = ViewType.Any;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        /// <summary>
        /// Generar alerta cuando la existencia disponible en el inventario o en lote, no es suficiente para cubrir la
        /// cantidad (acumulada) del producto que esta siendo facturado. El proposito no es esperar hasta la validacion
        /// para que el usuario se de cuenta
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_ObjectChanged(object Sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == e.Object && e.PropertyName == "Cantidad" && ObjectSpace.IsNewObject(View.CurrentObject))
            {
                if (e.NewValue == null)
                    return;
                var itemVenta = ((VentaDetalle)View.CurrentObject);
                if (itemVenta.Producto.Categoria.Clasificacion >= Producto.Module.BusinessObjects.EClasificacion.Servicios)
                    return;
                if (itemVenta.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.Ninguno)
                    return;
                decimal cantVta;
                if (itemVenta.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.PEPS ||
                    itemVenta.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.UEPS)
                {
                    if (itemVenta.Lote == null)
                        return;
                    cantVta = itemVenta.Venta.Detalles.Where(x => x.Bodega == itemVenta.Bodega && x.Producto == itemVenta.Producto && x.Lote == itemVenta.Lote).Sum(x => x.Cantidad);
                    if (cantVta > itemVenta.Lote.Existencia)
                        MostrarError($"Una validación no se cumple, la cantidad a vender {cantVta} debe ser menor o igual a la existencia del lote {itemVenta.Lote.Existencia}");
                }
                else
                {
                    cantVta = itemVenta.Venta.Detalles.Where(x => x.Bodega == itemVenta.Bodega && x.Producto == itemVenta.Producto).Sum(x => x.Cantidad);
                    decimal existencia = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(Inventario.Module.BusinessObjects.Inventario),
                        CriteriaOperator.Parse("Sum(Iif([TipoMovimiento.Operacion] In (0, 1), [Cantidad], -[Cantidad]))"),
                        CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ?", itemVenta.Bodega.Oid, itemVenta.Producto.Oid)));
                    if (cantVta > existencia)
                        MostrarError($"Una validación no se cumple, La cantidad a vender {cantVta} debe ser menor o igual al disponible en inventario {existencia}");
                }
            }
        }
    }
}
