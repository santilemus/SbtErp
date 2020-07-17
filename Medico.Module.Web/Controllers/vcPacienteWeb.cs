using System;
using System.Linq;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// ViewController Web Paciente. Para implementar ajustes y funciones propias de la plataforma web
    /// </summary>
    public class vcPacienteWeb : ViewControllerBaseWeb
    {
        public vcPacienteWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Paciente);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
