namespace SBT.Apps.Erp.Module.Web.Controllers
{
    public class LibroCompraListViewControllerWeb : ViewControllerBaseWeb
    {
        public LibroCompraListViewControllerWeb() : base()
        {
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroCompra);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
