namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class BancoCuentaListViewControllerWeb : BaseListViewControllerWeb
    {
        public BancoCuentaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.BancoCuenta);
        }
    }
}
