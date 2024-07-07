namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class CajaListViewControllerWeb : BaseListViewControllerWeb
    {
        public CajaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Facturacion.Module.BusinessObjects.Caja);
        }
    }
}
