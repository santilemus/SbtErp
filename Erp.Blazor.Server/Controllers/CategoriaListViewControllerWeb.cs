using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CategoriaListViewControllerWeb: BaseListViewControllerWeb
    {
        public CategoriaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(Producto.Module.BusinessObjects.Categoria);
        }
    }
}
