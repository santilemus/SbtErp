namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ViewController que aplica a la plataforma web para el BO VentaDetalle
    /// </summary>
    public class vcVentaDetalleWeb : ViewControllerBaseWeb
    {
        public vcVentaDetalleWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.Model;
        }
    }
}
