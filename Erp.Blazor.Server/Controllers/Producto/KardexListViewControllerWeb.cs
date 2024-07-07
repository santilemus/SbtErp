namespace SBT.Apps.Erp.Blazor.Server.Controllers.Producto
{
    public class KardexListViewControllerWeb : BaseListViewControllerWeb
    {
        public KardexListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Inventario.Module.BusinessObjects.Kardex);
        }
    }
}
