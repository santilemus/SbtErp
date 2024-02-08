namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ParametroListViewControllerWeb : BaseListViewControllerWeb
    {
        public ParametroListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.Parametro);
        }
    }

}

