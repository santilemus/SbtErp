using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class ProfesionListViewControllerWeb: ListViewControllerBase
    {
        public ProfesionListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.Profesion);
        }
    }
}
