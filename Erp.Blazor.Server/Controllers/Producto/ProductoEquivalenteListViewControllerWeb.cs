namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class ProductoEquivalenteListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoEquivalenteListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Producto.Module.BusinessObjects.ProductoEquivalente);
        }
    }
}
