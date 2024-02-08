using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View controller Para ocultar del lookup de Agencias el action New, porque en el login no debe de estar.
    /// </summary>
    public class vcEmpresaUnidadLookupListViewHideNew : ObjectViewController
    {
        private string key = "Desactivar";
        NewObjectViewController newController;

        public vcEmpresaUnidadLookupListViewHideNew() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad);
            TargetViewType = ViewType.ListView;
            TargetViewId = "EmpresaUnidad_LookupListView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            newController = Frame.GetController<NewObjectViewController>();
            if (newController != null)
                newController.Active[key] = !(View.ObjectTypeInfo.Type == typeof(SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad) && View is ListView);
        }

        protected override void OnDeactivated()
        {
            if (newController != null)
            {
                newController.Active.RemoveItem(key);
                newController = null;
            }
            base.OnDeactivated();
        }
    }
}
