using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;


namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    /// <summary>
    /// View Controller que corresponde a las vistas de listas del BO Consulta
    /// </summary>
    public class vcConsultaListView : ViewControllerBase
    {
        public vcConsultaListView(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Consulta);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
