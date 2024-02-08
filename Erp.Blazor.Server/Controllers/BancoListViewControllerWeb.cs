namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class BancoListViewControllerWeb: BaseListViewControllerWeb
    {
        public BancoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.Banco);
        }
    }
}
