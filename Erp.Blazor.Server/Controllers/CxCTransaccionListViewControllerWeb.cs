namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CxCTransaccionListViewControllerWeb: BaseListViewControllerWeb
    {
        public CxCTransaccionListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion);
        }
    }
}
