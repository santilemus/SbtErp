using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using System;

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View controller Para ocultar del lookup de Empresas el action New, porque en el login no debe de estar. 
    /// </summary>
    public class vcEmpresaLookupListViewHideNew : ObjectViewController
    {
        private string key = "Desactivar";
        NewObjectViewController newController;

        public vcEmpresaLookupListViewHideNew() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa);
            TargetViewType = ViewType.ListView;
            TargetViewId = "Empresa_LookupListView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            newController = Frame.GetController<NewObjectViewController>();
            if (newController != null)
                newController.Active[key] = !(View.ObjectTypeInfo.Type == typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa) && View is ListView);
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
    }
}
