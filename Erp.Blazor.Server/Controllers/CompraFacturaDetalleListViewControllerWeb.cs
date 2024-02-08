namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CompraFacturaDetalleListViewControllerWeb: BaseListViewControllerWeb
    {
        public CompraFacturaDetalleListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFacturaDetalle);
        }
    }
}
