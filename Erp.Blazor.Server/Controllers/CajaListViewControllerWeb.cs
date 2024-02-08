namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CajaListViewControllerWeb: BaseListViewControllerWeb
    {
        public CajaListViewControllerWeb() :base() 
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Caja);
        }
    }
}
