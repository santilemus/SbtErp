using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;

namespace SBT.Apps.Compra.Module.Controllers
{
    /// <summary>
    /// View Controller que aplica a la vista de detalle del BO CompraFactura
    /// </summary>
    public class vcCompraFacturaDetail: ViewController<DetailView>
    {
        private PopupWindowShowAction pwsaPagoAplicar;
        const string key = "Inactivo";
        public vcCompraFacturaDetail(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
            pwsaPagoAplicar = new PopupWindowShowAction(this, "saPagoAplicar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaPagoAplicar.Caption = "Aplicar Pago";
            pwsaPagoAplicar.ImageName = "bill";
            pwsaPagoAplicar.TargetViewId = "CompraFactura_DetailView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaPagoAplicar.CustomizePopupWindowParams += PwsaPagoAplicar_CustomizePopupWindowParams;
            pwsaPagoAplicar.Active[key] = (View.ObjectTypeInfo.Type == typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura) && 
                                           View.ObjectSpace.IsNewObject(View.CurrentObject));
            pwsaPagoAplicar.Execute += PwsaPagoAplicar_Execute;
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
            pwsaPagoAplicar.CustomizePopupWindowParams -= PwsaPagoAplicar_CustomizePopupWindowParams;
            pwsaPagoAplicar.Execute -= PwsaPagoAplicar_Execute;
            pwsaPagoAplicar.Active.RemoveItem(key);
            base.OnDeactivated();
        }


    }
}
