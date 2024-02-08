namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ActivoCatalogoListViewControllerWeb: BaseListViewControllerWeb
    {
        public ActivoCatalogoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoCatalogo);
        }
    }
}
