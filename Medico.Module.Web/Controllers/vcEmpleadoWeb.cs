using System;
using System.Linq;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller Empleado. Para implementar optimizaciones y funciones propias de la plataforma web
    /// </summary>
    public class vcEmpleadoWeb: ViewControllerBaseWeb
    {
        public vcEmpleadoWeb(): base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }


    }
}
