namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para BO PartidaDetalle. Utilizar para optimizaciones y funciones de la plataforma web
    /// </summary>
    public class vcPartidaDetalleWeb : ViewControllerBaseWeb
    {
        public vcPartidaDetalleWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.Model;
        }
    }
}
