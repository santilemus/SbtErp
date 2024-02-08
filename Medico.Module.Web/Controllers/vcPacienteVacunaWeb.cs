namespace SBT.Apps.Medico.Module.Web.Controllers
{
    public class vcPacienteVacunaWeb : ViewControllerBaseWeb
    {
        public vcPacienteVacunaWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.PacienteVacuna);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
