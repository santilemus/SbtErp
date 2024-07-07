namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class ProductoPrecioListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoPrecioListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Producto.Module.BusinessObjects.ProductoPrecio);
        }
    }
}
