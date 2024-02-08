namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class OrdenCompraListViewControllerWeb: BaseListViewControllerWeb
    {
        public OrdenCompraListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.OrdenCompra);
        }
    }
}
