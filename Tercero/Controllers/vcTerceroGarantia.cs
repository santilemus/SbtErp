using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Tercero.Module.Controllers
{
    /// <summary>
    /// controlador que corresponde a TerceroGarantia
    /// </summary>
    public class vcTerceroGarantia : ViewControllerBase
    {
        public vcTerceroGarantia() : base()
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
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.TerceroGarantia);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
