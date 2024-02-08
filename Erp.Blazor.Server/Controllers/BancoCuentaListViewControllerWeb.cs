namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class BancoCuentaListViewControllerWeb: BaseListViewControllerWeb
    {
        public BancoCuentaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoCuenta);
        }
    }
}
