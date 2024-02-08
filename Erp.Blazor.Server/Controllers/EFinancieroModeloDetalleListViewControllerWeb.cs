namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class EFinancieroModeloDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public EFinancieroModeloDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.EstadoFinancieroModeloDetalle);
        }
    }
}
