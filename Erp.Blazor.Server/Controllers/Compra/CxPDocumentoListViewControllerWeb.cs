namespace SBT.Apps.Erp.Blazor.Server.Controllers.Compra
{
    public class CxPDocumentoListViewControllerWeb : BaseListViewControllerWeb
    {
        public CxPDocumentoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(CxP.Module.BusinessObjects.CxPDocumento);
        }
    }
}
