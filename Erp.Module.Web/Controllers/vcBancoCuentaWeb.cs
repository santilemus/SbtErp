using System;
using System.Linq;


namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para BO BancoCuenta. Implementar aqui las optimizaciones y funcionalidad de la plataforma web
    /// </summary>
    public class vcBancoCuentaWeb: ViewControllerBaseWeb
    {
        public vcBancoCuentaWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoCuenta);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
