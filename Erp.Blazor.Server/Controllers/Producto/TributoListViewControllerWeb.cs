using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class TributoListViewControllerWeb : BaseListViewControllerWeb
    {
        public TributoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Producto.Module.BusinessObjects.Tributo);
        }
    }
}
