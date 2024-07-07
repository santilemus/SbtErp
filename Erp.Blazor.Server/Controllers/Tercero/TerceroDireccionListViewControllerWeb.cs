namespace SBT.Apps.Erp.Blazor.Server.Controllers.Tercero
{
    public class TerceroDireccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public TerceroDireccionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Tercero.Module.BusinessObjects.TerceroDireccion);
        }
    }
}
