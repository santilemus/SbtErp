using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;
using SBT.Apps.Base.Module.Controllers;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;

namespace SBT.Apps.Banco.Module.Controllers
{
    /// <summary>
    /// Bancos.
    /// ViewController para el BO BancoTransaccion que corresponde al encabezado de las transacciones de bancos
    /// </summary>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcBancoTransaccion: ViewControllerBase
    {
        PopupWindowShowAction pwsaSelectProveedor;

        public vcBancoTransaccion(): base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Es para filtrar los datos para la empresa de la sesion y evitar que se mezclen cuando hay más de una empresa
            if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) &&
                !(((ListView)View).CollectionSource.Criteria.ContainsKey("Empresa Actual")))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[BancoCuenta.Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
            ObjectSpace.Committed += ObjectSpace_Committed;
            pwsaSelectProveedor.CustomizePopupWindowParams += pwsaSelectProveedor_CustomizePopupWindowParams;
            pwsaSelectProveedor.Execute += pwsaSelectProveedorExecute;
        }

        protected override void OnDeactivated()
        {
            pwsaSelectProveedor.CustomizePopupWindowParams -= pwsaSelectProveedor_CustomizePopupWindowParams;
            pwsaSelectProveedor.Execute -= pwsaSelectProveedorExecute;
            ObjectSpace.Committed -= ObjectSpace_Committed;
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
            pwsaSelectProveedor = new PopupWindowShowAction(this, "pwsaSelectProveedor", PredefinedCategory.RecordEdit);
            pwsaSelectProveedor.TargetViewType = ViewType.DetailView;
            pwsaSelectProveedor.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            pwsaSelectProveedor.Caption = "Tercero";
            pwsaSelectProveedor.ImageName = "Tercero";
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
    }
}
