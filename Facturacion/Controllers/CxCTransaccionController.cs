using DevExpress.Charts.Native;
using DevExpress.Data.Filtering;
using DevExpress.Data.Helpers;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.CxC.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    /// <summary>
    /// View Controller que corresponde a los documentos de Cuentas por Cobrar
    /// </summary>
    public class CxCTransaccionController : ViewControllerBase
    {
        private SimpleAction saImprimirDocumento;

        public CxCTransaccionController() : base()
        {
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion);
            TargetViewType = ViewType.Any;

            // para impresion del documento cuando es nota de crédito, débito o cualquier otro documento de la cuenta por cobrar
            saImprimirDocumento = new SimpleAction(this, "CxC_ImprimirDocumento", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            saImprimirDocumento.Caption = "Imprimir";
            saImprimirDocumento.TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion);
            saImprimirDocumento.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            saImprimirDocumento.ToolTip = "Click para mostrar la vista previa de la impresión del documento seleccionado";
            // la acción solo esta habilitada para notas de crédito o debito (condicion Tipo.Padrre.Oid in (1,16)
            saImprimirDocumento.TargetObjectsCriteria = "[Oid] > 0 And [Tipo.Padre.Oid] in (1, 16)";
            saImprimirDocumento.ImageName = "ShowPrintPreview";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_Changed;
            ObjectSpace.Committing += ObjectSpace_Commiting;
            saImprimirDocumento.Execute += SaImprimirDocumento_Execute;
        }

        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_Changed;
            ObjectSpace.Committing -= ObjectSpace_Commiting;
            saImprimirDocumento.Execute -= SaImprimirDocumento_Execute;
            base.OnDeactivated();
        }

        private void ObjectSpace_Commiting(object Sender, CancelEventArgs e)
        {
            var items = ObjectSpace.ModifiedObjects.Cast<IXPObject>().Where<IXPObject>(x => x.GetType() == typeof(CxCTransaccion) || x.GetType() == typeof(CxCDocumento));
            try
            {
                if (items != null && items.Count() > 0)
                    ActualizarCxC(items);
                items = ObjectSpace.ModifiedObjects.Cast<IXPObject>().Where<IXPObject>(x => x.GetType() == typeof(CxCDocumentoDetalle));
                if (items != null && items.Count() > 0)
                {
                    if (!DoProcesarDetalleCxC(items))
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error {ex.Message}", InformationType.Error);
                e.Cancel = true;
            }
        }

        private void ActualizarCxC(IEnumerable<IXPObject> items)
        {
            foreach (CxCTransaccion item in items)
                if (item.Estado != ECxCTransaccionEstado.Anulado)
                    ActualizarSaldoFactura(item);
        }

        private bool DoProcesarDetalleCxC(IEnumerable<IXPObject> items)
        {
            /// 204 es el Codigo del Tipo de Movimiento de Inventario que corresponde a devoluciones de clientes (notas de credito)
            /// las 2 siguientes lineas estan fuera del foreach para evitar ejecutarlo mas de una vez
            CriteriaOperator criteria = CriteriaOperator.FromLambda<InventarioTipoMovimiento>(x => x.Codigo == "204" && x.Activo);
            InventarioTipoMovimiento tipoMovimiento = ObjectSpace.FindObject<InventarioTipoMovimiento>(criteria);
            if (tipoMovimiento == null)
            {
                Application.ShowViewStrategy.ShowMessage($"No se encontró Tipo de Movimiento de Inventario con código 204 que debe corresponder a Devoluciones de Clientes, el Inventario no se puede actualizar ",
                    InformationType.Error);
                return false;
            }
            foreach (CxCDocumentoDetalle item in items)
            {
                if (item.CxCDocumento.Tipo.Oid == 2 || item.CxCDocumento.Tipo.Oid == 17)
                {              
                    ActualizarInventario(item, tipoMovimiento);
                    ActualizarKardex(item, tipoMovimiento);
                    //ActualizarLote;
                }
            }
            return true;
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
        private void ActualizarInventario(CxCDocumentoDetalle item, InventarioTipoMovimiento tipo)
        {
            Inventario.Module.BusinessObjects.Inventario inventarioItem = ObjectSpace.FindObject<Inventario.Module.BusinessObjects.Inventario>(
                   CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ? && [TipoMovimiento.Oid] == ?",
                   item.VentaDetalle.Bodega.Oid, item.VentaDetalle.Producto.Oid, tipo.Oid));
            if (inventarioItem == null)
            {
                inventarioItem = ObjectSpace.CreateObject<Inventario.Module.BusinessObjects.Inventario>();
                inventarioItem.Bodega = item.VentaDetalle.Bodega;
                inventarioItem.Producto = item.VentaDetalle.Producto;
                inventarioItem.TipoMovimiento = tipo;
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
        private void ActualizarKardex(CxCDocumentoDetalle item, InventarioTipoMovimiento tipo)
        {
            Kardex kardexItem = ObjectSpace.FindObject<Kardex>(
                CriteriaOperator.Parse("[Producto.Oid] == ? && [TipoMovimiento] == ? && [Fecha] = ? && [Referencia] == ?",
                item.VentaDetalle.Producto.Oid, tipo.Oid, item.CxCDocumento.Fecha, item.Oid));
            if (kardexItem == null)
            {
                kardexItem = ObjectSpace.CreateObject<Kardex>();
                kardexItem.Bodega = item.VentaDetalle.Bodega;
                kardexItem.Producto = item.VentaDetalle.Producto;
                kardexItem.TipoMovimiento = tipo;
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
            decimal fCargo = Convert.ToDecimal(item.Venta.CxCTransacciones.Where(x => x.Venta == item.Venta && x.Tipo.TipoOperacion == ETipoOperacion.Cargo &&
                                                           x.Estado != ECxCTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal fAbono = Convert.ToDecimal(item.Venta.CxCTransacciones.Where(x => x.Venta == item.Venta && x.Tipo.TipoOperacion == ETipoOperacion.Abono &&
                                                           x.Estado != ECxCTransaccionEstado.Anulado).Sum(x => x.Monto));
            decimal fNcredito = Convert.ToDecimal(item.Venta.CxCTransacciones.Where(
                x => x.Venta == item.Venta && x.Tipo.Padre.Oid == 1 && x.Estado != ECxCTransaccionEstado.Anulado).Sum(x => x.Monto));
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
            if (View == null || View.CurrentObject == null || e.Object == null)
                return;

            if (View.CurrentObject == e.Object && e.PropertyName == "Venta")
            {
                var cxcDocumento = (CxCDocumento)View.CurrentObject;
                if (cxcDocumento.Venta.Saldo <= 0.0m || cxcDocumento.Venta.Estado != EEstadoFactura.Debe)
                    MostrarError($"La venta con {cxcDocumento.Venta.TipoFactura.Nombre} debe tener saldo >= 0 y Estado Debe para aplicarle {cxcDocumento.Tipo}");
            }
            // para obtener el correlativo del documento de nota de credito autorizado
            if (View.CurrentObject == e.Object && ObjectSpace.IsNewObject(View.CurrentObject))
            {
                if (e.PropertyName == "AutorizacionDocumento")
                {
                    if (e.NewValue == null)
                    {
                        ((CxCTransaccion)View.CurrentObject).NumeroDocumento = null;
                        MostrarError($"No se encontró la autorización de correlativos para {((CxCTransaccion)View.CurrentObject).Tipo.Nombre}. No conoce el número de documento");
                        return;
                    }
                    AutorizacionDocumento aud = (AutorizacionDocumento)e.NewValue;
                    /// *** NOTA OJO VER PARA CAMBIAR ***
                    /// Cuando no es dte es que se obtiene el max. Si es un Dte es un Guid. y no se puede obtener el max
                    string noDoc = Convert.ToString(((XPObjectSpace)ObjectSpace).Session.Evaluate<CxCTransaccion>(CriteriaOperator.Parse("max([NumeroDocumento])"),
                                 CriteriaOperator.Parse("Oid == ?", aud.Oid))) + 1;
                    if (int.TryParse(noDoc, out int val))
                    {
                        if (val >= aud.NoDesde && val < aud.NoHasta)
                            ((CxCTransaccion)View.CurrentObject).NumeroDocumento = noDoc;
                    }
                    else
                    {
                        ((CxCTransaccion)View.CurrentObject).NumeroDocumento = null;
                        MostrarError($"No hay autorización de correlativo disponible para {((CxCTransaccion)View.CurrentObject).Tipo.Nombre}");
                    }
                }
            }
        }

        /// <summary>
        /// Imprimir el documento solo cuando esta habilitada para el tipo de documento: originalmente nota de crédito y débito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SaImprimirDocumento_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject == null)
            {
                Application.ShowViewStrategy.ShowMessage("No se encuentra ningún documento para imprimir", InformationType.Error);
                return;
            }
            if (View.CurrentObject.GetType() != typeof(CxCDocumento))
            {
                Application.ShowViewStrategy.ShowMessage($@"Objeto seleccionado no válido, debe ser del tipo {nameof(CxCDocumento)}", InformationType.Error);
                return;
            }
            var reportOsProvider = ReportDataProvider.GetReportObjectSpaceProvider(this.Application.ServiceProvider);
            var reportStorage = ReportDataProvider.GetReportStorage(this.Application.ServiceProvider);

            CxCDocumento doc = (CxCDocumento)View.CurrentObject;
            IObjectSpace objectSpace = reportOsProvider.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData = objectSpace.GetObject<IReportDataV2>(doc.AutorizacionDocumento.Reporte);
            string handle = reportStorage.GetReportContainerHandle(reportData);
            ReportServiceController controller = Frame.GetController<ReportServiceController>();
            CriteriaOperator objectsCriteria = ((BaseObjectSpace)objectSpace).GetObjectsCriteria(View.ObjectTypeInfo, e.SelectedObjects);
            if (controller != null)
            {
                controller.ShowPreview(handle, objectsCriteria);
            };
        }
    }
}
