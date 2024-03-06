namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class PartidaModeloListViewContrrollerWeb : BaseListViewControllerWeb
    {
        public PartidaModeloListViewContrrollerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Contabilidad.Module.BusinessObjects.PartidaModelo);
        }
    }
}
