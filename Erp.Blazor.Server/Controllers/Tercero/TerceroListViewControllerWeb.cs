using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Erp.Blazor.Server.Controllers.Tercero
{
    public class TerceroListViewControllerWeb : BaseListViewControllerWeb
    {
        public TerceroListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.Tercero);
        }
    }
}
