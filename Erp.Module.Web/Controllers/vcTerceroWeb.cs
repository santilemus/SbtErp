using System;
using System.Linq;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// View Controller Tercero. Para implementar optimizaciones y funciones propias de la plataforma web
    /// </summary>
    public class vcTerceroWeb: ViewControllerBaseWeb
    {
        public vcTerceroWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.Tercero);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
