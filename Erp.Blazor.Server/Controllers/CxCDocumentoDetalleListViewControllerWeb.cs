namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CxCDocumentoDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public CxCDocumentoDetalleListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCDocumentoDetalle);
        }
    }
}
