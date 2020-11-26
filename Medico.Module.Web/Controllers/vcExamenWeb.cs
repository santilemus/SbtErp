using System;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    ///  Controller para la plataforma Web y que aplica al BO Examen
    /// </summary>
    public class vcExamenWeb: ViewControllerBaseWeb
    {
        public vcExamenWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Generico.Module.BusinessObjects.Examen);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
