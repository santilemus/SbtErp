namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class EmpleadoListViewControllerWeb: BaseListViewControllerWeb
    {
        public EmpleadoListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado);
        }
    }
}
