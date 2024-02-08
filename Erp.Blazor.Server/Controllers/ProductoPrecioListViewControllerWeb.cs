namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ProductoPrecioListViewControllerWeb: BaseListViewControllerWeb
    {
        public ProductoPrecioListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.ProductoPrecio);
        }
    }
}
