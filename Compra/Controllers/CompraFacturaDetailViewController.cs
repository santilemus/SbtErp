using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Compra.Module.Controllers
{
    /// <summary>
    /// View Controller que aplica a la vista de detalle del BO CompraFactura
    /// </summary>
    public class CompraFacturaDetailViewController : ViewController<DetailView>
    {
        private PopupWindowShowAction pwsaPagoAplicar;
        const string key = "Inactivo";
        private PopupWindowShowAction pwsaAnular;

        public CompraFacturaDetailViewController() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
            pwsaPagoAplicar = new PopupWindowShowAction(this, "saPagoAplicar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaPagoAplicar.Caption = "Aplicar Pago";
            pwsaPagoAplicar.ImageName = "bill";
            pwsaPagoAplicar.TargetViewId = "CompraFactura_DetailView";

            pwsaAnular = new PopupWindowShowAction(this, "FacturaCompra_Anular", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaAnular.TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
            pwsaAnular.TargetViewType = ViewType.DetailView;
            pwsaAnular.ToolTip = "Clic para Anular el documento seleccionado";
            pwsaAnular.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            pwsaAnular.Caption = "Anular";
            pwsaAnular.ImageName = "Attention";
            pwsaAnular.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            pwsaAnular.TargetObjectsCriteria = "[Estado] = 'Debe' && [Saldo] != 0.0";
            pwsaAnular.AcceptButtonCaption = "Anular";
            pwsaAnular.CancelButtonCaption = "Cancelar";
            pwsaAnular.ConfirmationMessage = "Esta segur@ de anular el documento seleccionado";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaPagoAplicar.CustomizePopupWindowParams += PwsaPagoAplicar_CustomizePopupWindowParams;
            pwsaPagoAplicar.Active[key] = (View.ObjectTypeInfo.Type == typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura) &&
                                           View.ObjectSpace.IsNewObject(View.CurrentObject));
            pwsaPagoAplicar.Execute += PwsaPagoAplicar_Execute;
            ObjectSpace.Committing += ObjectSpace_Committing;

            pwsaAnular.CustomizePopupWindowParams += PwsaAnular_CustomizePopupWindowParams;
            pwsaAnular.Execute += PwsaAnular_Execute;
        }

        private void PwsaPagoAplicar_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowView.ObjectSpace.IsNewObject(e.PopupWindow.View.CurrentObject))
            {
                var pago = (e.PopupWindow.View.CurrentObject as CxP.Module.BusinessObjects.CxPTransaccion);
                pago.Save();
                if ((View.CurrentObject as CompraFactura).Saldo == pago.Monto)
                    (View.CurrentObject as CompraFactura).ActualizarSaldo(pago.Monto, Base.Module.BusinessObjects.EEstadoFactura.Pagado, true);
                else
                    (View.CurrentObject as CompraFactura).ActualizarSaldo(pago.Monto, Base.Module.BusinessObjects.EEstadoFactura.Debe, true); // pago incompleto, no deberia de suceder revisar la vista para que no se pueda
            }
        }

        private void PwsaPagoAplicar_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            if (View.ObjectSpace.IsNewObject(View.CurrentObject))
            {
                IObjectSpace osPago = Application.CreateObjectSpace(typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion));
                CxPTransaccion pagoObj = osPago.CreateObject<CxPTransaccion>();
                pagoObj.Fecha = (View.CurrentObject as CompraFactura).Fecha;
                var mone = osPago.GetObjectByKey<Moneda>((View.CurrentObject as CompraFactura).Moneda.Codigo);
                pagoObj.Moneda = mone;
                pagoObj.Monto = (View.CurrentObject as CompraFactura).Saldo;
                pagoObj.Estado = ECxPTransaccionEstado.Aplicado;
                DetailView PagoDetailView = Application.CreateDetailView(osPago, "CxPTransaccion_DetailView_Pago", true, pagoObj);
                e.View = PagoDetailView;
            }
        }

        protected override void OnDeactivated()
        {
            pwsaAnular.CustomizePopupWindowParams -= PwsaAnular_CustomizePopupWindowParams;
            pwsaAnular.Execute -= PwsaAnular_Execute;

            pwsaPagoAplicar.CustomizePopupWindowParams -= PwsaPagoAplicar_CustomizePopupWindowParams;
            pwsaPagoAplicar.Execute -= PwsaPagoAplicar_Execute;
            pwsaPagoAplicar.Active.RemoveItem(key);
            ObjectSpace.Committing -= ObjectSpace_Committing;
            base.OnDeactivated();
        }

        /// <summary>
        /// Evento del ObjectSpace que se ejecuta previo a guardar los cambios en la bd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var factura = View.CurrentObject as CompraFactura;
            if (factura.CxPTransacciones.Count > 0)
            {
                var monto = factura.CxPTransacciones.Where(x => x.Estado != ECxPTransaccionEstado.Anulado && x.Tipo.TipoOperacion == ETipoOperacion.Cargo).Sum(x => x.Monto) -
                            factura.CxPTransacciones.Where(x => x.Estado != ECxPTransaccionEstado.Anulado && x.Tipo.TipoOperacion == ETipoOperacion.Abono).Sum(x => x.Monto);
                factura.ActualizarSaldo(monto, monto == factura.Saldo ? EEstadoFactura.Pagado: factura.Estado, true);
                /*
                decimal totalCxP = factura.CxPTransacciones.Where(x => x.Tipo.Padre.Oid == 1 && x.Estado != ECxPTransaccionEstado.Anulado).Sum(x => x.Monto);
                if (totalCxP > 0 && (factura.Saldo - totalCxP) == 0.0m) 
                {
                    factura.ActualizarSaldo(factura.Saldo - totalCxP, EEstadoFactura.Devolucion, true);
                    return;
                }
                totalCxP = factura.CxPTransacciones.Sum(x => x.Tipo.TipoOperacion == ETipoOperacion.Cargo ? x.Monto : -x.Monto);
                if (totalCxP > 0)
                    factura.ActualizarSaldo(factura.Saldo - totalCxP, (factura.Saldo - totalCxP) == 0.0m ? EEstadoFactura.Pagado: factura.Estado, true);
                */
            }
        }

        private void PwsaAnular_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            CompraFactura compra = (CompraFactura)View.CurrentObject;
            compra.Anular(((AnularParametros)e.PopupWindowView.CurrentObject));
            View.ObjectSpace.CommitChanges();
        }

        private void PwsaAnular_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            using IObjectSpace osParam = Application.CreateObjectSpace(typeof(AnularParametros));
            AnularParametros anularParams = osParam.CreateObject<AnularParametros>();
            anularParams.FechaAnulacion = DateTime.Now;
            e.View = Application.CreateDetailView(osParam, anularParams);
            e.View.Caption = "Anular Venta";
        }

    }
}
