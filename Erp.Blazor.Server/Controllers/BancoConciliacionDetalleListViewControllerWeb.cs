namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class BancoConciliacionDetalleListViewControllerWeb: BaseListViewControllerWeb
    {
        public BancoConciliacionDetalleListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoConciliacionDetalle);
        }
    }
}
