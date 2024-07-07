namespace SBT.Apps.Erp.Blazor.Server.Controllers.Compra
{
    public class OrdenCompraDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public OrdenCompraDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Compra.Module.BusinessObjects.OrdenCompraDetalle);
        }
    }
}
