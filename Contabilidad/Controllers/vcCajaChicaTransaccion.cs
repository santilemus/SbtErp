using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;
using SBT.Apps.Base.Module.Controllers;


namespace SBT.Apps.Banco.Module.Controllers
{
    /// <summary>
    /// Bancos.
    /// ViewController para el BO CajaChicaTransaccion que corresponde al encabezado de las transacciones de caja chica
    /// </summary>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcCajaChicaTransaccion : ViewControllerBase
    {
        public vcCajaChicaTransaccion(): base()
        {

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0)
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[CajaChica.Empresa] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
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
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.CajaChicaTransaccion);
        }
    }
}
