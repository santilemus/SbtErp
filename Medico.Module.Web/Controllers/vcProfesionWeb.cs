using System;
using System.Linq;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para el BO Profesion para implementar optimizaciones y funciones en la plataforma web
    /// </summary>
    public class vcProfesionWeb: ViewControllerBaseWeb
    {
        public vcProfesionWeb(): base()
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
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.Profesion);
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
