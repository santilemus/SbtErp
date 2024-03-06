using SBT.Apps.Medico.Expediente.Module.BusinessObjects;

namespace SBT.Medico.Blazor.Server.Controllers
{
    public class PacienteListViewControllerWeb: CustomListViewControllerWeb
    {
        public PacienteListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(Paciente);
        }
    }
}
