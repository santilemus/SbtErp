using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using System;

namespace SBT.Apps.Banco.Module.Controllers
{
    /// <summary>
    /// Bancos.
    /// ViewController para el BO BancoTransaccion que corresponde al encabezado de las transacciones de bancos
    /// </summary>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcBancoTransaccion : ViewControllerBase
    {

        public vcBancoTransaccion() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Es para filtrar los datos para la empresa de la sesion y evitar que se mezclen cuando hay más de una empresa
            if (!(((ListView)View).CollectionSource.Criteria.ContainsKey("Empresa Actual")))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[BancoCuenta.Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);

        }

        protected override void OnDeactivated()
        {

            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTransaccion);
            TargetViewType = ViewType.ListView;
        }
    }
}
