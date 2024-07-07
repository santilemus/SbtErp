namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class CajaChicaListViewControllerWeb : BaseListViewControllerWeb
    {
        public CajaChicaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.CajaChica);
        }
    }
}
