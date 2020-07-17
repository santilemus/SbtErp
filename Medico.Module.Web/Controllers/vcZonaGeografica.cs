using DevExpress.ExpressApp;
using DevExpress.Web.ASPxTreeList;
using System;
using System.Linq;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcZonaGeografica : ViewController
    {
        public vcZonaGeografica()
        {
            InitializeComponent();
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
            ASPxTreeList control = (View as ListView).Editor.Control as ASPxTreeList;
            control.Settings.ShowFilterBar = DevExpress.Web.GridViewStatusBarMode.Auto;
            control.Settings.ShowFilterRow = true;
            control.Settings.ShowFilterRowMenu = true;
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
