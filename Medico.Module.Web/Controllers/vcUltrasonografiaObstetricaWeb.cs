namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para el BO UltraSonografiaObstetrica, aplica a la plataforma web
    /// </summary>
    public class vcUltrasonografiaObstetricaWeb : ViewControllerBaseWeb
    {
        public vcUltrasonografiaObstetricaWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Ginecologia.UltraSonografiaObstetrica);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
