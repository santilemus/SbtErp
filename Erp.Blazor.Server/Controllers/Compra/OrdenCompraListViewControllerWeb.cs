namespace SBT.Apps.Erp.Blazor.Server.Controllers.Compra
{
    public class OrdenCompraListViewControllerWeb : BaseListViewControllerWeb
    {
        public OrdenCompraListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Compra.Module.BusinessObjects.OrdenCompra);
        }
    }
}
