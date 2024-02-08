namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class TipoPlanillaListViewControllerWeb : BaseListViewControllerWeb
    {
        public TipoPlanillaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.TipoPlanilla);
        }
    }

}

