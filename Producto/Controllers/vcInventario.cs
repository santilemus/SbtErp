using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using System;

namespace SBT.Apps.Producto.Module.Controllers
{
    /// <summary>
    /// View Controller para el BO Inventario
    /// </summary>
    public class vcInventario : ViewControllerBase
    {
        public vcInventario() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Inventario.Module.BusinessObjects.Inventario);
            TargetViewType = ViewType.Any;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) &&
                !(((ListView)View).CollectionSource.Criteria.ContainsKey("Empresa Actual")))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Bodega.Empresa.Oid] == ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
