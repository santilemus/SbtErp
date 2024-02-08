namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class TransaccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public TransaccionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.Transaccion);
        }
    }

}

