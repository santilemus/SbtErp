using System;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// View Controller que aplica al BO ActivoCatalogo para la plataforma Web
    /// </summary>
    public class vcActivoCatalogoWeb: ViewControllerBaseWeb
    {
        public vcActivoCatalogoWeb(): base()
        {

        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoCatalogo);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
