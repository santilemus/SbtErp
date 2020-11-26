using System;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller que aplica al BO UltrasonografiaPelvica plataforma web
    /// </summary>
    public class vcUltrasonografiaPelvicaWeb: ViewControllerBaseWeb
    {
        public vcUltrasonografiaPelvicaWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Ginecologia.UltrasonografiaPelvica);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
