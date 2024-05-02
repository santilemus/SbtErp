namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CxPDocumentoDetalleListViewControllerWeb: BaseListViewControllerWeb
    {
        public CxPDocumentoDetalleListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPDocumentoDetalle);
        }
    }
}
