using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Producto.Module.Controllers
{
    public class vcProductoPrecio: ViewControllerBase
    {
        public vcProductoPrecio(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Producto.Module.BusinessObjects.ProductoPrecio);
            TargetViewType = ViewType.Any;
            
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
