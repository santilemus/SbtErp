namespace SBT.Apps.Erp.Blazor.Server.Controllers.Compra
{
    public class CxPDocumentoDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public CxPDocumentoDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(CxP.Module.BusinessObjects.CxPDocumentoDetalle);
        }
    }
}
