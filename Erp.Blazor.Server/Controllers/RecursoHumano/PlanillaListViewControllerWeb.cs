namespace SBT.Apps.Erp.Blazor.Server.Controllers.RecursoHumano
{
    public class PlanillaListViewControllerWeb : BaseListViewControllerWeb
    {
        public PlanillaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.RecursoHumano.Module.BusinessObjects.Planilla);
        }
    }

}

