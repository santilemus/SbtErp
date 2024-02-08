namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class CatalogoListViewControllerWeb: BaseListViewControllerWeb
    {
        public CatalogoListViewControllerWeb()
        {
            TargetObjectType = typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo);
        }
    }
}
