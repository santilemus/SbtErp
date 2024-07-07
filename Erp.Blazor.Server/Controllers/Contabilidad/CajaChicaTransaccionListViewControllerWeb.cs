namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class CajaChicaTransaccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public CajaChicaTransaccionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.CajaChicaTransaccion);
        }
    }
}
