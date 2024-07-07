namespace SBT.Apps.Erp.Blazor.Server.Controllers.Venta
{
    public class VentaDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public VentaDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Facturacion.Module.BusinessObjects.VentaDetalle);
        }

    }
}
