namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CxPTransaccionListViewControllerWeb: BaseListViewControllerWeb
    {
        public CxPTransaccionListViewControllerWeb() :base() 
        {
            TargetObjectType = typeof(SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion);
        }
    }
}
