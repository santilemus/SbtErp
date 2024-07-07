namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class ProductoProveedorListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoProveedorListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Producto.Module.BusinessObjects.ProductoProveedor);
        }
    }
}
