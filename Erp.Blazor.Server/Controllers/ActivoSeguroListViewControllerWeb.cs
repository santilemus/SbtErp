namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ActivoSeguroListViewControllerWeb: BaseListViewControllerWeb
    {
        public ActivoSeguroListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoSeguro);
        }
    }
}
