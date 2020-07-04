using System;
using System.Linq;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para BO BancoConciliacion. Para implementar optimizaciones y funciones de la plataforma web
    /// </summary>
    public class vcBancoConciliacionWeb: ViewControllerBaseWeb
    {
        public vcBancoConciliacionWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoConciliacion);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
