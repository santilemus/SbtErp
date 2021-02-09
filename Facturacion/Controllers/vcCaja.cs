using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    public class vcCaja : ViewControllerBase
    {
        public vcCaja() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0)
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Agencia.Empresa.Oid] == ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Caja);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
