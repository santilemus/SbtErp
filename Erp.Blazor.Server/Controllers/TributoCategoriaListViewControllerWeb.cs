namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class TributoCategoriaListViewControllerWeb : BaseListViewControllerWeb
    {
        public TributoCategoriaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.TributoCategoria);
        }
    }
}
