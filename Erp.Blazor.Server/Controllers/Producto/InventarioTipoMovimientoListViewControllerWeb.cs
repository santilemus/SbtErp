namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class InventarioTipoMovimientoListViewControllerWeb : BaseListViewControllerWeb
    {
        public InventarioTipoMovimientoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Inventario.Module.BusinessObjects.InventarioTipoMovimiento);
        }
    }
}
