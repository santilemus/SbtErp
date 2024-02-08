namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ViewController para el BO ActivoMovimiento para la plataforma web
    /// </summary>
    public class vcActivoMovimiento : ViewControllerBaseWeb
    {
        public vcActivoMovimiento() : base()
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
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoMovimiento);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
