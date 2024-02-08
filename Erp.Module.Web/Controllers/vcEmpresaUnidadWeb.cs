namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// Controller para BO EmpresaUnidad. Implementar optimizaciones y funciones especificas de la plataforma web
    /// </summary>
    public class vcEmpresaUnidadWeb : ViewControllerBaseWeb
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
            FixColumnWidthInListView = ETipoAjusteColumnaListView.BestFit;
        }
    }
}
