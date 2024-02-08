namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ProductoProveedorListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoProveedorListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.ProductoProveedor);
        }
    }
}
