namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class BancoConciliacionDetalleListViewControllerWeb : BaseListViewControllerWeb
    {
        public BancoConciliacionDetalleListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.BancoConciliacionDetalle);
        }
    }
}
