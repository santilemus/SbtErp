namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class ProductoEnsambleListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoEnsambleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Producto.Module.BusinessObjects.ProductoEnsamble);
        }
    }
}
