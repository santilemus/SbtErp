namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ParametroRentaListViewControllerWeb : BaseListViewControllerWeb
    {
        public ParametroRentaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.ParametroRenta);
        }
    }

}

