namespace SBT.Apps.Erp.Blazor.Server.Controllers.RecursoHumano
{
    public class TransaccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public TransaccionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.RecursoHumano.Module.BusinessObjects.Transaccion);
        }
    }

}

