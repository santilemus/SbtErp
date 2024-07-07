namespace SBT.Apps.Erp.Blazor.Server.Controllers.RecursoHumano
{
    public class ParametroListViewControllerWeb : BaseListViewControllerWeb
    {
        public ParametroListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.RecursoHumano.Module.BusinessObjects.Parametro);
        }
    }

}

