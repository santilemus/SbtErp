namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class ProductoListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.Producto);
        }
    }
}
