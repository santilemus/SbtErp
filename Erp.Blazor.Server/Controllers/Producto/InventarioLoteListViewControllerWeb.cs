namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class InventarioLoteListViewControllerWeb : BaseListViewControllerWeb
    {
        public InventarioLoteListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Inventario.Module.BusinessObjects.InventarioLote);
        }
    }
}
