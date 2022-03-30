using System;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Compra.Module.Controllers
{
    /// <summary>
    /// View Controller que corresponde a la vista de lista del BO CompraFactura
    /// </summary>
    public class vcCompraFacturaList: ViewControllerBase
    {
        public vcCompraFacturaList(): base()
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
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
            TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
        }

    }
}
