namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class TributoCategoriaListViewControllerWeb : BaseListViewControllerWeb
    {
        public TributoCategoriaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Producto.Module.BusinessObjects.TributoCategoria);
        }
    }
}
