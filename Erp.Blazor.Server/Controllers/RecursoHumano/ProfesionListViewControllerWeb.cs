using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Erp.Blazor.Server.Controllers.RecursoHumano
{
    public class ProfesionListViewControllerWeb : ListViewControllerBase
    {
        public ProfesionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Base.Module.BusinessObjects.Profesion);
        }
    }
}
