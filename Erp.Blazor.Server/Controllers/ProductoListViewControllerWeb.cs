namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ProductoListViewControllerWeb: BaseListViewControllerWeb
    {
        public ProductoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(Producto.Module.BusinessObjects.Producto);
        }
    }
}
