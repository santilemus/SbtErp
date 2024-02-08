namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CajaChicaListViewControllerWeb: BaseListViewControllerWeb
    {
        public CajaChicaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.CajaChica);
        }
    }
}
