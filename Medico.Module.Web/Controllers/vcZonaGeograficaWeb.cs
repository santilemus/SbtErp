using System;
using System.Linq;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para BO ZonaGeogrica. Para implementar optimizaciones y funciones de la plataforma Web
    /// </summary>
    public class vcZonaGeograficaWeb: ViewControllerBaseWeb
    {
        public vcZonaGeograficaWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.ZonaGeografica);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
