namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class BancoConciliacionListViewControllerWeb: BaseListViewControllerWeb
    {
        public BancoConciliacionListViewControllerWeb(): base() 
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoConciliacion);
        }
    }
}
