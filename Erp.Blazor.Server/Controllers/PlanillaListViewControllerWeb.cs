namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class PlanillaListViewControllerWeb : BaseListViewControllerWeb
    {
        public PlanillaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.Planilla);
        }
    }

}

