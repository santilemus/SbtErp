using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.RecursoHumano.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcAccionPersonal : ViewControllerBase
    {
        public vcAccionPersonal()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0)
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Empleado.Empresa] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
            pwsaAprobar.CustomizePopupWindowParams += CustomizePopupWindowParam;
            pwsaRechazar.CustomizePopupWindowParams += CustomizePopupWindowParam;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            pwsaAprobar.CustomizePopupWindowParams -= CustomizePopupWindowParam;
            pwsaRechazar.CustomizePopupWindowParams -= CustomizePopupWindowParam;
            base.OnDeactivated();
        }

        private void pwsaAprobar_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (((AccionPersonal)View.CurrentObject).Estado == EEstadoAccionPersonal.Digitada)
            {
                try
                {
                    ((AccionPersonal)View.CurrentObject).Aprobar(((APersonalParam)e.PopupWindowViewCurrentObject).Comentario);
                    MostrarMensajeResultado($"La Acción de Personal de {((AccionPersonal)View.CurrentObject).Empleado.NombreCompleto} se aprobó");
                }
                catch (Exception E)
                {
                    MostrarError($"La acción no se pudo aprobar por el siguiente error {E.Message}");
                }
            }
        }

        private void CustomizePopupWindowParam(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace();
            APersonalParam param = objectSpace.CreateObject<APersonalParam>();
            e.View = Application.CreateDetailView(objectSpace, param);
            e.Size = new System.Drawing.Size(500, 200);
            e.IsSizeable = false;
        }

        private void pwsaRechazar_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

            if (((AccionPersonal)View.CurrentObject).Estado == EEstadoAccionPersonal.Digitada)
            {
                try
                {
                    ((AccionPersonal)View.CurrentObject).Rechazar(((APersonalParam)e.PopupWindowViewCurrentObject).Comentario);
                    MostrarMensajeResultado($"La Acción de Personal de {((AccionPersonal)View.CurrentObject).Empleado.NombreCompleto} se rechazó");
                }
                catch (Exception E)
                {
                    MostrarError($"La acción no se pudo aprobar por el siguiente error {E.Message}");
                }
            }
        }
    }
}
