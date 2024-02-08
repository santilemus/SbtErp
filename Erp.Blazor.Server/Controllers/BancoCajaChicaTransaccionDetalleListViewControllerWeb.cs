namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class BancoCajaChicaTransaccionDetalleListViewControllerWeb: BaseListViewControllerWeb   
    {
        public BancoCajaChicaTransaccionDetalleListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.CajaChicaTransaccionDetalle);
        }
    }
}
