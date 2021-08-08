using System;
using System.Linq;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Banco.Module.Controllers
{
    /// <summary>
    /// Bancos.
    /// ViewController para el BO BancoCuenta que corresponde a las cuentas bancarias de la empresa
    /// </summary>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcBancoCuenta : ViewControllerBase
    {
        public vcBancoCuenta(): base()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Se comento porque el filtro se aplica en ViewControllerBase.
            //if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) && View.ObjectTypeInfo.Type == typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoCuenta))
            //{
            //    ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = new BinaryOperator("Empresa", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
            //}
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            this.TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoCuenta);
        }
    }
}
