using System;
using System.Linq;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// ViewController Web Consulta. Para implementar ajustes y funciones propias de la plataforma web
    /// </summary>
    public class vcConsultaWeb: ViewControllerBaseWeb
    {
        public vcConsultaWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Consulta);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
