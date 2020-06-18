using System;
using System.Linq;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// Controller para el BO Catalogo. Implementar optimizaciones y funciones del catalogo contable en la plataforma web
    /// </summary>
    public class vcCatalogoContableWeb: ViewControllerBaseWeb
    {
        public vcCatalogoContableWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Contabilidad.BusinessObjects.Catalogo);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
