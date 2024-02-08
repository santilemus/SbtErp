namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ViewController que aplica al BO ActivoCategoria para la plataforma Web
    /// </summary>
    public class vcActivoCategoria : ViewControllerBaseWeb
    {
        public vcActivoCategoria() : base()
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
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoCategoria);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
