namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class AccionPersonalListViewControllerWeb: BaseListViewControllerWeb
    {
        public AccionPersonalListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.AccionPersonal);
        }
    }

}

