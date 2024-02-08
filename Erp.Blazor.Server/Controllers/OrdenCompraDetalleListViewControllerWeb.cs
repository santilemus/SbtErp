namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class OrdenCompraDetalleListViewControllerWeb: BaseListViewControllerWeb
    {
        public OrdenCompraDetalleListViewControllerWeb() :base() 
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.OrdenCompraDetalle);
        }
    }
}
