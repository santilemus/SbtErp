using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    /// <summary>
    /// View Controller que aplica al BO BancoTarjetaTransaccion
    /// </summary>
    public class vcBancoTarjetaTransaccion: ViewControllerBase
    {
        public vcBancoTarjetaTransaccion(): base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Es para filtrar los datos para la empresa de la sesion y evitar que se mezclen cuando hay más de una empresa
            if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) &&
                !(((ListView)View).CollectionSource.Criteria.ContainsKey("Empresa Actual")))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Tarjeta.Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTarjetaTransaccion);
        }
    }
}
