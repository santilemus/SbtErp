namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class EFinancieroModeloListViewControllerWeb : BaseListViewControllerWeb
    {
        public EFinancieroModeloListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Contabilidad.Module.BusinessObjects.EstadoFinancieroModelo);
        }
    }
}
