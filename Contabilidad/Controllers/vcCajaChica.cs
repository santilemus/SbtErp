using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Banco.Module.Controllers
{
    /// <summary>
    /// Bancos.
    /// ViewController para el BO CajaChica que corresponde a los parámetros de Caja Chica.
    /// </summary>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcCajaChica : ViewControllerBase
    {
        public vcCajaChica() : base()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
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

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            this.TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.CajaChica);
        }
    }
}
