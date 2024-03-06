namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class BancoConciliacionListViewControllerWeb : BaseListViewControllerWeb
    {
        public BancoConciliacionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.BancoConciliacion);
        }
    }
}
