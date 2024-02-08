namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class EmpresaListViewControllerWeb: BaseListViewControllerWeb
    {
        public EmpresaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa);
        }
    }
}
