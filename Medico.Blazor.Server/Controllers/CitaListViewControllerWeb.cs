using SBT.Apps.Medico.Expediente.Module.BusinessObjects;

namespace SBT.Medico.Blazor.Server.Controllers
{
    public class CitaListViewControllerWeb: CustomListViewControllerWeb
    {
        public CitaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(Cita);
        }

    }
}
