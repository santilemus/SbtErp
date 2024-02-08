namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ProductoEquivalenteListViewControllerWeb : BaseListViewControllerWeb
    {
        public ProductoEquivalenteListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.ProductoEquivalente);
        }
    }
}
