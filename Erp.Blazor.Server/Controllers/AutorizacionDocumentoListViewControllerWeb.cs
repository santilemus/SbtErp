namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class AutorizacionDocumentoListViewControllerWeb: BaseListViewControllerWeb
    {
        public AutorizacionDocumentoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.AutorizacionDocumento);
        }
    }
}
