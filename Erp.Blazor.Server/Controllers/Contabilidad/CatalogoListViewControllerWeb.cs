namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class CatalogoListViewControllerWeb : BaseListViewControllerWeb
    {
        public CatalogoListViewControllerWeb()
        {
            TargetObjectType = typeof(Apps.Contabilidad.BusinessObjects.Catalogo);
        }
    }
}
