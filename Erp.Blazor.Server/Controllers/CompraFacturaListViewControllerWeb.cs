namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CompraFacturaListViewControllerWeb: BaseListViewControllerWeb
    {
        public CompraFacturaListViewControllerWeb() : base() 
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
        }

    }
}
