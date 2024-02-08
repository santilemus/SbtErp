namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class SaldoMesListViewControllerWeb : BaseListViewControllerWeb
    {
        public SaldoMesListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Contabilidad.Module.BusinessObjects.SaldoMes);
        }
    }
}
