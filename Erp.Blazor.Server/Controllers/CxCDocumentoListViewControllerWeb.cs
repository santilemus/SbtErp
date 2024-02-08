namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CxCDocumentoListViewControllerWeb : BaseListViewControllerWeb
    {
        public CxCDocumentoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCDocumento);
        }
    }
}
