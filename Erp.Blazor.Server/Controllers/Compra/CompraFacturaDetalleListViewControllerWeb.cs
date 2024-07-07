namespace SBT.Apps.Erp.Blazor.Server.Controllers.Compra
{
    public class CompraFacturaDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public CompraFacturaDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Compra.Module.BusinessObjects.CompraFacturaDetalle);
        }
    }
}
