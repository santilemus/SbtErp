using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class TerceroListViewControllerWeb: BaseListViewControllerWeb
    {
        public TerceroListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(Tercero.Module.BusinessObjects.Tercero);
        }
    }
}
