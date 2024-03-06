using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Medico.Blazor.Server.Controllers
{
    public class MedicoListViewControllerWeb: CustomListViewControllerWeb
    {
        public MedicoListViewControllerWeb(): base() 
        {
            TargetObjectType = typeof(SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico);
        }

    }
}
