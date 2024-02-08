namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class VentaDetalleListViewControllerWeb: BaseListViewControllerWeb
    {
        public VentaDetalleListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.VentaDetalle);
        }

    }
}
