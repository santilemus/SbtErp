namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class InventarioListViewControllerWeb : BaseListViewControllerWeb
    {
        public InventarioListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Inventario.Module.BusinessObjects.Inventario);
        }
    }
}
