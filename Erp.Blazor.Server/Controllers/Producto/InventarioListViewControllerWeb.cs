namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class InventarioListViewControllerWeb : BaseListViewControllerWeb
    {
        public InventarioListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Inventario.Module.BusinessObjects.Inventario);
        }
    }
}
