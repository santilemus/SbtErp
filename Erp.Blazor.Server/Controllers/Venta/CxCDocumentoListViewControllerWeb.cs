namespace SBT.Apps.Erp.Blazor.Server.Controllers.Venta
{
    public class CxCDocumentoListViewControllerWeb : BaseListViewControllerWeb
    {
        public CxCDocumentoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(CxC.Module.BusinessObjects.CxCDocumento);
        }
    }
}
