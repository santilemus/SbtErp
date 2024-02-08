namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ProductoEnsambleListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoEnsambleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.ProductoEnsamble);
        }
    }
}
