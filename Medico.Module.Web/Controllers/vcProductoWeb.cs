namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// Controlador Web para Producto. El proposito es implementar optimizaciones y fuciones especificas cuando
    /// la plataforma es web
    /// </summary>
    public class vcProductoWeb : ViewControllerBaseWeb
    {
        public vcProductoWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.Producto);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
