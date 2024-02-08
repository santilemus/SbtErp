namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class InventarioLoteListViewControllerWeb : BaseListViewControllerWeb
    {
        public InventarioLoteListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Inventario.Module.BusinessObjects.InventarioLote);
        }
    }
}
