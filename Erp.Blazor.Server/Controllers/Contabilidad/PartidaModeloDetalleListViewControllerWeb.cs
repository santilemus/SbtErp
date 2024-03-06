namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class PartidaModeloDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public PartidaModeloDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Contabilidad.Module.BusinessObjects.PartidaModeloDetalle);
        }
    }
}
