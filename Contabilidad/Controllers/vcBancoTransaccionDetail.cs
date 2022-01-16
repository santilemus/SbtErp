using System;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;

namespace SBT.Apps.Banco.Module.Controllers
{
    /// <summary>
    /// View Controller que corresponde a la vista de detalle del BO BancoTransaccion
    /// </summary>
    /// <remarks>
    ///  Con relacion a la personalizacion de la apariencia, ver mas informacion en
    ///  https://docs.devexpress.com/eXpressAppFramework/113374/conditional-appearance/how-to-customize-the-conditional-appearance-module-behavior
    /// </remarks>
    public class vcBancoTransaccionDetail: ViewController<DetailView>
    {
        private PopupWindowShowAction pwsaSelectProveedor;
        private AppearanceController appearanceController;
        public vcBancoTransaccionDetail()
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
            pwsaSelectProveedor = new PopupWindowShowAction(this, "pwsaSelectProveedor", PredefinedCategory.RecordEdit);
            pwsaSelectProveedor.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            pwsaSelectProveedor.Caption = "Tercero";
            pwsaSelectProveedor.ImageName = "Tercero";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.Committed += ObjectSpace_Committed;
            pwsaSelectProveedor.CustomizePopupWindowParams += pwsaSelectProveedor_CustomizePopupWindowParams;
            pwsaSelectProveedor.Execute += pwsaSelectProveedorExecute;
            
            appearanceController = Frame.GetController<AppearanceController>();
            if (appearanceController != null)
            {
                appearanceController.CustomApplyAppearance +=
                    new EventHandler<ApplyAppearanceEventArgs>(
                        appearanceController_CustomApplyAppearance);
            }
        }

        protected override void OnDeactivated()
        {
            pwsaSelectProveedor.CustomizePopupWindowParams -= pwsaSelectProveedor_CustomizePopupWindowParams;
            pwsaSelectProveedor.Execute -= pwsaSelectProveedorExecute;
            ObjectSpace.Committed -= ObjectSpace_Committed;

            if (appearanceController != null)
            {
                appearanceController.CustomApplyAppearance -=
                     new EventHandler<ApplyAppearanceEventArgs>(
                         appearanceController_CustomApplyAppearance);
            }

            base.OnDeactivated();
        }

        protected override void Dispose(bool disposing)
        {
            if (pwsaSelectProveedor != null)
                pwsaSelectProveedor.Dispose();
            base.Dispose(disposing);
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            var bancoTransaccion = (this.View.CurrentObject as Banco.Module.BusinessObjects.BancoTransaccion);
            if (bancoTransaccion != null && bancoTransaccion.BancoCuenta != null)
            {
                bancoTransaccion.BancoCuenta.CalcularSaldo(bancoTransaccion.Fecha);
            }
        }

        private void pwsaSelectProveedor_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = e.Application.CreateObjectSpace(typeof(Tercero.Module.BusinessObjects.Tercero));
            string idView = e.Application.FindLookupListViewId(typeof(Tercero.Module.BusinessObjects.Tercero));
            IModelListView modelListView = (IModelListView)Application.FindModelView(idView);
            modelListView.FilterEnabled = true;
            modelListView.DataAccessMode = CollectionSourceDataAccessMode.Server;
            //alternativa CollectionSourceDataAccessMode.ServerView es más light
            //modelListView.DataAccessMode = CollectionSourceDataAccessMode.ServerView;
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(os, typeof(Tercero.Module.BusinessObjects.Tercero), idView);
            e.View = Application.CreateListView(modelListView, collectionSource, true);
        }

        private void pwsaSelectProveedorExecute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            // cuando modelListView.DataAccessMode = CollectionSourceDataAccessMode.ServerView. Se tiene que obtener el tercero.
            //var tercero = ObjectSpace.GetObjectByKey<Tercero.Module.BusinessObjects.Tercero>(((DevExpress.ExpressApp.ObjectRecord)e.PopupWindowView.CurrentObject).ObjectKeyValue);
            var tercero = (e.PopupWindowView.CurrentObject as Tercero.Module.BusinessObjects.Tercero);
            (View.CurrentObject as Banco.Module.BusinessObjects.BancoTransaccion).Beneficiario = tercero.Nombre;

        }

        private void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e)
        {
            if (e.AppearanceObject.Enabled == false)
            {
                CustomizeDisabledEditorsAppearance(e);
                // e.Handled = true;
            }
        }

        protected virtual void CustomizeDisabledEditorsAppearance(ApplyAppearanceEventArgs e) { }
    }
}
