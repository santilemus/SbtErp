namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class TerceroDireccionListViewControllerWeb: BaseListViewControllerWeb
    {
        public TerceroDireccionListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.TerceroDireccion);
        }
    }
}
