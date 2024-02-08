namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class PartidaListViewControllerWeb : BaseListViewControllerWeb
    {
        public PartidaListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Apps.Contabilidad.Module.BusinessObjects.Partida);
        }
    }
}
