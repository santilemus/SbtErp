namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class BancoTransaccionListViewControllerWeb: BaseListViewControllerWeb
    {
        public BancoTransaccionListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
        }
    }
}
