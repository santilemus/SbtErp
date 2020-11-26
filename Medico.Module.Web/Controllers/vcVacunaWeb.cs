using System;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para la plataforma web y que aplica al BO Vacuna
    /// </summary>
    public class vcVacunaWeb: ViewControllerBaseWeb
    {
        public vcVacunaWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Generico.Module.BusinessObjects.Vacuna);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }

    }
}
