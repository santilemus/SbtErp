namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ActividadEconomicaListViewControllerWeb: BaseListViewControllerWeb
    {
        public ActividadEconomicaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.ActividadEconomica);
        }
    }
}
