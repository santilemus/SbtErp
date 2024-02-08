namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class VentaListViewControllerWeb: BaseListViewControllerWeb
    {
        public VentaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
        }
    }
}
