using System;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller que aplica al BO UltrasonografiaObstetricaDetalle plataforma web
    /// </summary>
    public class vcUltrasonografiaObstetricaDetalleWeb: ViewControllerBaseWeb
    {
       
        public vcUltrasonografiaObstetricaDetalleWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Ginecologia.UltrasonografiaObstetricaDetalle);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
