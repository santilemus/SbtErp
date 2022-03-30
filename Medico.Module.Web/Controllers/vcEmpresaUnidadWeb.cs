using System;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// ViewController Web EmpresaUnidad. Para implementar ajustes y funciones propias de la plataforma web
    /// </summary>
    /// 
    public class vcEmpresaUnidadWeb: ViewControllerBaseWeb
    {
        public vcEmpresaUnidadWeb() : base()
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
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad);
            this.FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
