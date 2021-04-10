using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.Controllers;
using System;
using System.Linq;

namespace SBT.Apps.Producto.Module.Controllers
{
    public class vcProductoPrecio : ViewControllerBase
    {
        public vcProductoPrecio() : base()
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
