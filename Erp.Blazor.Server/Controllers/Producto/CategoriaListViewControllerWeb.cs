using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class CategoriaListViewControllerWeb : BaseListViewControllerWeb
    {
        public CategoriaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.Categoria);
        }
    }
}
