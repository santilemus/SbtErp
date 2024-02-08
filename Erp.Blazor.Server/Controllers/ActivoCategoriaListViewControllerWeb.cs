namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ActivoCategoriaListViewControllerWeb: BaseListViewControllerWeb
    {
        public ActivoCategoriaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Activo.Module.BusinessObjects.ActivoCategoria);
        }
    }
}
