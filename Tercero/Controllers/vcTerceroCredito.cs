using SBT.Apps.Base.Module.Controllers;
using System;
using System.Linq;

namespace SBT.Apps.Tercero.Module.Controllers
{
    /// <summary>
    /// Controlador que corresponde a TerceroCredito
    /// </summary>
    public class vcTerceroCredito : ViewControllerBase
    {
        public vcTerceroCredito() : base()
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

        }

        protected override void Dispose(bool disposing)
        {
            // codigo de objetos a destruir va aqui

            base.Dispose(disposing);
        }

    }
}
