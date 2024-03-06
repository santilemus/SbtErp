namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class BancoCajaChicaTransaccionDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public BancoCajaChicaTransaccionDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.CajaChicaTransaccionDetalle);
        }
    }
}
