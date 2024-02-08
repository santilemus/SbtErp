namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class PartidaDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public PartidaDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Contabilidad.Module.BusinessObjects.PartidaDetalle);
        }
    }
}
