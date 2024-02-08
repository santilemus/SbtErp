using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.CxC.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    /// <summary>
    /// View Controller que corresponde a los documentos de Cuentas por Cobrar
    /// </summary>
    public class vcCxCTransaccion : ViewControllerBase
    {
        InventarioTipoMovimiento tipoMovimiento;
        public vcCxCTransaccion() : base()
        {
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion);
            TargetViewType = ViewType.Any;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_Changed;
            ObjectSpace.Committing += ObjectSpace_Commiting;
        }

        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_Changed;
            ObjectSpace.Committing -= ObjectSpace_Commiting;
            base.OnDeactivated();
        }

        private void ObjectSpace_Commiting(object Sender, CancelEventArgs e)
        {
            /// 204 es el Codigo del Tipo de Movimiento de Inventario que corresponde a devoluciones de clientes (notas de credito)
            tipoMovimiento = ObjectSpace.FindObject<InventarioTipoMovimiento>(CriteriaOperator.And(new BinaryOperator("Codigo", "204"), new BinaryOperator("Activo", true)));
            if (tipoMovimiento == null)
            {
                MostrarError($"No se encontró Tipo de Movimiento de Inventario con código 204 que debe corresponder a Devoluciones de Clientes, el Inventario no se puede actualizar ");
                e.Cancel = true;
                return;
            }
            System.Collections.IList items = ObjectSpace.ModifiedObjects;
            foreach (object item in items)
            {
                if (item.GetType() == typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion) ||
                    item.GetType() == typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCDocumento))
                {
                    if (((CxCTransaccion)item).Venta.Estado == EEstadoFactura.Anulado)
                    {
                        e.Cancel = true;
                        return;
                    }
                    ActualizarSaldoFactura((CxCTransaccion)item);
                }
                else
                {
                    CxCDocumentoDetalle fItem = (CxCDocumentoDetalle)item;
                    if (fItem.VentaDetalle.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Servicios ||
                        fItem.VentaDetalle.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Intangible ||
                        fItem.VentaDetalle.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Otros)
                        continue;
                    ActualizarInventario(fItem);
                    ActualizarKardex(fItem);
                    if (fItem.VentaDetalle.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.PEPS ||
                        fItem.VentaDetalle.Producto.Categoria.MetodoCosteo == Producto.Module.BusinessObjects.EMetodoCosteoInventario.UEPS)
                        ActualizarLote(fItem);
                }
            }
        }

        /// <summary>
        /// actualizar el inventario por cada linea del detalle del documento de cuenta por cobrar, normalmente una nota
        /// de crédito por devolución de mercancia. 
        /// El acumulado para el inventario se obtiene al sumar las cantidades devueltas en el mes, excepto para el detalle
        /// del documento de cxc (ncr devolucion) que esta siendo procesado. Cuando es modificacion se suma item.Cantidad al
        /// al acumulado del mes.
        /// Cuando se ha eliminado el item, considerar solo la cantidad acumulada del mes como  se meciono
        /// antes, tiene el mismo efecto que restar de las entradas la cantidad del item eliminado del mes
        /// </summary>
        /// <param name="item">El item del detalle de la venta que esta siendo procesado</param>
        private void ActualizarInventario(CxCDocumentoDetalle item)
        {
            Inventario.Module.BusinessObjects.Inventario inventarioItem = ObjectSpace.FindObject<Inventario.Module.BusinessObjects.Inventario>(
                   CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ? && [TipoMovimiento.Oid] == ?",
                   item.VentaDetalle.Bodega.Oid, item.VentaDetalle.Producto.Oid, tipoMovimiento.Oid));
            if (inventarioItem == null)
            {
                inventarioItem = ObjectSpace.CreateObject<Inventario.Module.BusinessObjects.Inventario>();
                inventarioItem.Bodega = item.VentaDetalle.Bodega;
                inventarioItem.Producto = item.VentaDetalle.Producto;
                inventarioItem.TipoMovimiento = tipoMovimiento;
            }
            if (ObjectSpace.IsNewObject(item))
                inventarioItem.Cantidad += item.Cantidad;
            else
            {
                decimal cantidadMes = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(CxCDocumentoDetalle), CriteriaOperator.Parse("Sum([Cantidad])"),
                     CriteriaOperator.Parse("GetMonth([Venta.Fecha]) == ? && GetYear([Venta.Fecha]) == ? && [Producto.Oid] == ? && [Bodega.Oid] == ? && [Oid] != ?",
                     item.CxCDocumento.Fecha.Month, item.CxCDocumento.Fecha.Year, item.VentaDetalle.Producto.Oid, item.VentaDetalle.Bodega.Oid, item.Oid)));
                // Si la fila fue borrada, la nueva cantidad (de entrada por devolución) es la cantidad acumulada del mes, sin considerar al item [Oid] != item.Oid
                cantidadMes += item.IsDeleted ? 0 : item.Cantidad;
                inventarioItem.Cantidad = cantidadMes;
            }
            inventarioItem.Save();
        }

        /// <summary>
        /// actualizar el kardex por cada linea del detalle del documento de cxc (en este caso una nota de crédito por devolucion. 
        /// Se verifica que el kardex no existe a traves del Producto, TipoMovimiento, Fecha y Referencia
        /// Cuando se ha eliminado el item, considerar solo la cantidad acumulada del mes como  se meciono antes, tiene
        /// el mismo efecto que restar de las entradas por devolucion, la cantidad del item eliminado del mes
        /// </summary>
        /// <param name="item"></param>
        /// <remarks>
        /// Evaluar en las pruebas porque cuando pase por aqui es posible que item.Oid aun no tenga valor, en cuyo caso 
        /// tendremos que usar otro datos, por ejemplo item.CxCDocumento.Oid
        /// </remarks>
        private void ActualizarKardex(CxCDocumentoDetalle item)
        {
            Kardex kardexItem = ObjectSpace.FindObject<Kardex>(
                CriteriaOperator.Parse("[Producto.Oid] == ? && [TipoMovimiento] == ? && [Fecha] = ? && [Referencia] == ?",
                item.VentaDetalle.Producto.Oid, tipoMovimiento.Oid, item.CxCDocumento.Fecha, item.Oid));
            if (kardexItem == null)
            {
                kardexItem = ObjectSpace.CreateObject<Kardex>();
                kardexItem.Bodega = item.VentaDetalle.Bodega;
                kardexItem.Producto = item.VentaDetalle.Producto;
                kardexItem.TipoMovimiento = tipoMovimiento;
            }
            kardexItem.Cantidad = item.Cantidad;
            kardexItem.CostoUnidad = item.VentaDetalle.Costo;
            kardexItem.PrecioUnidad = item.PrecioUnidad;
            kardexItem.Referencia = item;
            kardexItem.Save();
        }

        /// <summary>
        /// Actualizar las entradas del lote cuando el metodo de costeo del inventario es PEPS o UEPS y el documento
        /// de cuenta por cobrar es una nota de credito por devolucion de producto
        /// </summary>
        /// <param name="item">La linea de detalle del documento de cxc (instancia del BO CxCDocumentoDetalle)</param>
        /// <remarks>Es posible que al lote tengamos que agregarle una propiedad para las devoluciones porque no es 
        /// correcto sumar las devoluciones a las entradas es confuso. Una alternativa es agregar una propiedad aparte
        /// para las devoluciones</remarks>
        private void ActualizarLote(CxCDocumentoDetalle item)
        {
            if (item.VentaDetalle.Lote != null)
            {
                InventarioLote lote = ObjectSpace.GetObjectByKey<InventarioLote>(item.VentaDetalle.Lote.Oid);
                if (lote == null)
                    return;
                if (ObjectSpace.IsNewObject(item))
                {
                    lote.Entrada += item.Cantidad;
                    lote.Costo = item.VentaDetalle.Costo;
                }
                else
                {
                    if (item.IsDeleted)
                        lote.Entrada -= item.Cantidad;

                }

                lote.Save();
            }
        }

        /// <summary>
        /// Actualizar el saldo de la factura
        /// </summary>
        /// <param name="item">Es un documento de CxC</param>
        private void ActualizarSaldoFactura(CxCTransaccion item)
        {
            //decimal fSaldo = Convert.ToDecimal(ObjectSpace.Evaluate(typeof(CxCDocumento), 
            //    CriteriaOperator.Parse("Sum(Iif([CxCTransaccion.Concepto.Tipo] == 1, [Total] + [Valor], -[Total] - [Valor]))"),
            //    CriteriaOperator.Parse("[Venta.Oid] == ? && [CxCTransaccion.Estado] != 2", item.Venta.Oid)));

            decimal fCargo = Convert.ToDecimal(item.Venta.CxCTransacciones.Where(x => x.Venta == item.Venta && x.Tipo.TipoOperacion == ETipoOperacion.Cargo &&
                                                           x.Estado != ECxCTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal fAbono = Convert.ToDecimal(item.Venta.CxCTransacciones.Where(x => x.Venta == item.Venta && x.Tipo.TipoOperacion == ETipoOperacion.Abono &&
                                                           x.Estado != ECxCTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal fNcredito = Convert.ToDecimal(item.Venta.CxCTransacciones.Where(
                x => x.Venta == item.Venta && x.Tipo.Oid > 1 && x.Tipo.Oid <= 4 && x.Estado != ECxCTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal monto = Math.Abs(fCargo - fAbono);
            if (fNcredito > 0 && item.Venta.Total == fNcredito)
                item.Venta.ActualizarSaldo(0.0m, EEstadoFactura.Devolucion, true);
            else if ((item.Venta.Total - monto) == 0.0m)
            {
                item.Venta.ActualizarSaldo(0.0m, EEstadoFactura.Pagado, true);
            }
            else
                item.Venta.ActualizarSaldo(item.Venta.Total - monto, EEstadoFactura.Debe, true);
            item.Venta.Save();
        }

        /// <summary>
        /// Evento que se dispara cuando cambia una propiedad del BO. Se utilizara para verificar si es factible crear
        /// un documento de cuenta por cobrar, porque la factura debe estar en Estado Debe y el Saldo ser diferente de 0.0
        /// </summary>
        /// <param name="sender">El objeto que dispara el evento</param>
        /// <param name="e">Argumento del evento</param>
        private void ObjectSpace_Changed(object sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == e.Object && e.PropertyName == "Venta")
            {
                var cxcDocumento = (CxCDocumento)View.CurrentObject;
                if (cxcDocumento.Venta.Saldo <= 0.0m || cxcDocumento.Venta.Estado != EEstadoFactura.Debe)
                    MostrarError($"La venta con {cxcDocumento.Venta.TipoFactura.Nombre} debe tener saldo >= 0 y Estado Debe para aplicarle {cxcDocumento.Tipo}");
            }
        }

    }
}
