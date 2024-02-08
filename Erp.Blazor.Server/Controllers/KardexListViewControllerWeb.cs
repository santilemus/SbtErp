namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class KardexListViewControllerWeb : BaseListViewControllerWeb
    {
        public KardexListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Inventario.Module.BusinessObjects.Kardex);
        }
    }
}
