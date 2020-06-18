using System;
using System.Linq;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ViewController Web Empresa. Para implementar ajustes y funciones propias de la plataforma web
    /// </summary>
    public class vcEmpresaWeb : ViewControllerBaseWeb
    {
        public vcEmpresaWeb() : base()
        {
            //DoInitializeComponent();
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
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa);
            this.FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
