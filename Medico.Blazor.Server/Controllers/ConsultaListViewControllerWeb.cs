using SBT.Apps.Medico.Expediente.Module.BusinessObjects;

namespace SBT.Medico.Blazor.Server.Controllers
{
    public class ConsultaListViewControllerWeb: CustomListViewControllerWeb
    {
        public ConsultaListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(Consulta);
        }
    }
}
