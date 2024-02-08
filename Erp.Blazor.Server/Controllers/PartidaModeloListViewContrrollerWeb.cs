namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class PartidaModeloListViewContrrollerWeb: BaseListViewControllerWeb
    {
        public PartidaModeloListViewContrrollerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaModelo);
        }
    }
}
