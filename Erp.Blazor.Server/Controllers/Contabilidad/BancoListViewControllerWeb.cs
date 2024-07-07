namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class BancoListViewControllerWeb : BaseListViewControllerWeb
    {
        public BancoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.Banco);
        }
    }
}
