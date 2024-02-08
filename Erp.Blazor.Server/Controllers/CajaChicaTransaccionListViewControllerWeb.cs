namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CajaChicaTransaccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public CajaChicaTransaccionListViewControllerWeb() :base()
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.CajaChicaTransaccion);
        }
    }
}
