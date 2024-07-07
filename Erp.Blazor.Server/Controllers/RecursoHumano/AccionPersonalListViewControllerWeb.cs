namespace SBT.Apps.Erp.Blazor.Server.Controllers.RecursoHumano
{
    public class AccionPersonalListViewControllerWeb : BaseListViewControllerWeb
    {
        public AccionPersonalListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.RecursoHumano.Module.BusinessObjects.AccionPersonal);
        }
    }

}

