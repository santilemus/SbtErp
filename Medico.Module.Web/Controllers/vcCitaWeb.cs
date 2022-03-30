using System;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.Controllers;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    // ViewController que se aplica al BO Cita y para la plataforma web
    public class vcCitaWeb: ViewControllerBaseWeb
    {
        public vcCitaWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Cita);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
