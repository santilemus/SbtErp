namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class EFinancieroModeloListViewControllerWeb: BaseListViewControllerWeb
    {
        public EFinancieroModeloListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.EstadoFinancieroModelo);
        }
    }
}
