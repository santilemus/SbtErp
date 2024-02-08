namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class PartidaModeloDetalleListViewControllerWeb: BaseListViewControllerWeb
    {
        public PartidaModeloDetalleListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.PartidaModeloDetalle);
        }
    }
}
