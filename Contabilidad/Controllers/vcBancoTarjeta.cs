using System;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    /// <summary>
    /// View Controller que aplica al BO BancoTarjeta
    /// </summary>
    public class vcBancoTarjeta: ViewControllerBase
    {
        public vcBancoTarjeta(): base()
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
            TargetObjectType = typeof(SBT.Apps.Banco.Module.BusinessObjects.BancoTarjeta);
        }
    }
}
