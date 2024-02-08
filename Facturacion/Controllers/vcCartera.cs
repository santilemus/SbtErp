using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.CxC.Module.Controllers
{
    /// <summary>
    /// Cuenta por Cobrar. View Controller que corresponde a la Cartera de Cuenta por Cobrar
    /// </summary>
    public class vcCartera : ViewControllerBase
    {
        public vcCartera() : base()
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
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.Cartera);
        }

        protected override void Dispose(bool disposing)
        {
            // destruir los objetos aqui
            base.Dispose(disposing);
        }

    }
}
