namespace SBT.Apps.Erp.Blazor.Server.Controllers.RecursoHumano
{
    public class TipoPlanillaListViewControllerWeb : BaseListViewControllerWeb
    {
        public TipoPlanillaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.RecursoHumano.Module.BusinessObjects.TipoPlanilla);
        }
    }

}

