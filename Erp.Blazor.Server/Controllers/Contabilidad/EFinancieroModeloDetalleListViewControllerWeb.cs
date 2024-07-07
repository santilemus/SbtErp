namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class EFinancieroModeloDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public EFinancieroModeloDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Contabilidad.Module.BusinessObjects.EstadoFinancieroModeloDetalle);
        }
    }
}
