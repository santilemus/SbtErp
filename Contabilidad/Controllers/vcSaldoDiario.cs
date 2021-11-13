using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    /// <summary>
    /// ViewController que corresponde al BO SaldoDiario
    /// </summary>
    public class vcSaldoDiario: ViewControllerBase
    {
        public vcSaldoDiario(): base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Para filtrar los datos para la empresa de la sesion y evitar que se mezclen cuando hay más de una empresa
            if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) && (((ListView)View).ObjectTypeInfo.FindMember("Empresa") != null) &&
                !(((ListView)View).CollectionSource.Criteria.ContainsKey("Empresa Actual")))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Cuenta.Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.SaldoDiario);
        }
    }
}
