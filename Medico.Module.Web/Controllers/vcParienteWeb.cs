using System;
using System.Text;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    public class vcParienteWeb: ViewControllerBaseWeb
    {
        public vcParienteWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Pariente);
            this.FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
