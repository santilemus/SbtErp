using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    public class FacturaCompraListViewControllerWeb: ViewControllerBaseWeb
    {
        public FacturaCompraListViewControllerWeb(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
            TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
