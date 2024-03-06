using SBT.Apps.Medico.Expediente.Module.BusinessObjects;

namespace SBT.Medico.Blazor.Server.Controllers
{
    public class ConsultaNutricionListViewControllerWeb: CustomListViewControllerWeb
    {
        public ConsultaNutricionListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(ConsultaNutricion);
        }
    }
}
