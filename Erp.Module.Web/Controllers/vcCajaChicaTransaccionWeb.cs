namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para BO CajaChicaTransaccion. Implementa optimizaciones y funciones de la plataforma web
    /// </summary>
    public class vcCajaChicaTransaccionWeb : ViewControllerBaseWeb
    {
        public vcCajaChicaTransaccionWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.CajaChicaTransaccion);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
