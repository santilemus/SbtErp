namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller especifico de la plataforma Web para el BO ProbemaMedico. 
    /// </summary>
    public class vcProblemaMedicoWeb : ViewControllerBaseWeb
    {
        public vcProblemaMedicoWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.ProblemaMedico);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
