namespace SBT.Apps.Erp.Blazor.Server.Controllers.RecursoHumano
{
    public class OperacionListViewControllerWeb : BaseListViewControllerWeb
    {
        public OperacionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.RecursoHumano.Module.BusinessObjects.Operacion);
        }
    }

}

