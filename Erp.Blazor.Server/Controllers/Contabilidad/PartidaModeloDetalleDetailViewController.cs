using DevExpress.ExpressApp;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using DevExpress.ExpressApp.Blazor.Editors;

namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    /// <summary>
    /// Controller para el BO Partida Modelo Detalle. No se usa, se creo para probar funcionalidad
    /// Borrarlo si no se va a utilizar
    /// </summary>
    public class PartidaModeloDetalleDetailViewController : ViewController<DetailView>
    {
        public PartidaModeloDetalleDetailViewController() : base()
        {
            TargetObjectType = typeof(PartidaModeloDetalle);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            var criteriaItem = View.FindItem("Criteria");
            if (criteriaItem != null && criteriaItem.GetType().Name == "PopupFilterPropertyEditor")
            {
                PopupFilterPropertyEditor popFilter = (criteriaItem as PopupFilterPropertyEditor);
            }
        }
    }
}
