namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class OperacionListViewControllerWeb : BaseListViewControllerWeb
    {
        public OperacionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.Operacion);
        }
    }

}

