using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBT.Apps.Iva.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Iva.Module.Controllers;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    public class LibroCompraListViewControllerWeb: ViewControllerBaseWeb
    {
        public LibroCompraListViewControllerWeb(): base()
        {
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroCompra);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
