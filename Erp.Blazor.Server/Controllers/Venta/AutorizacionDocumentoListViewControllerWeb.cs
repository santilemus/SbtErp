namespace SBT.Apps.Erp.Blazor.Server.Controllers.Venta
{
    public class AutorizacionDocumentoListViewControllerWeb : BaseListViewControllerWeb
    {
        public AutorizacionDocumentoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Facturacion.Module.BusinessObjects.AutorizacionDocumento);
        }
    }
}
