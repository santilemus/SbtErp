namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class TerceroDocumentoListViewControllerWeb: BaseListViewControllerWeb
    {
        public TerceroDocumentoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(Tercero.Module.BusinessObjects.TerceroDocumento);
        }
    }
}
