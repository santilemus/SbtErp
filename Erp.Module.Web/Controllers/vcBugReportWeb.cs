using System;
using System.Linq;


namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para el BO BugReport que registra los bug encontrados en la aplicacion
    /// </summary>
    public class vcBugReportWeb: ViewControllerBaseWeb
    {
        public vcBugReportWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.BugReport);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
