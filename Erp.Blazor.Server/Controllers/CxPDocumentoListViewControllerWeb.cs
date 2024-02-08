namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CxPDocumentoListViewControllerWeb: BaseListViewControllerWeb
    {
        public CxPDocumentoListViewControllerWeb() : base() 
        {
            TargetObjectType = typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPDocumento);
        }
    }
}
