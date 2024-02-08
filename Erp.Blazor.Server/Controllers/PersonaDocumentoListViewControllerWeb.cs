namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class PersonaDocumentoListViewControllerWeb: BaseListViewControllerWeb
    {
        public PersonaDocumentoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.PersonaDocumento);
        }
    }
}
