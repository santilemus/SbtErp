namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class InventarioTipoMovimientoListViewControllerWeb : BaseListViewControllerWeb
    {
        public InventarioTipoMovimientoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Inventario.Module.BusinessObjects.InventarioTipoMovimiento);
        }
    }
}
