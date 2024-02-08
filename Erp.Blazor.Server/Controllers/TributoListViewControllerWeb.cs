using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class TributoListViewControllerWeb: BaseListViewControllerWeb
    {
        public TributoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.Tributo);
        }
    }
}
