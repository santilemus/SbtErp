using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.RecursoHumano.Module.BusinessObjects;
using System;

namespace SBT.Apps.RecursoHumano.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcAccionPersonal : ViewControllerBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction pwsaAprobar;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction pwsaRechazar;

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

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pwsaAprobar = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.pwsaRechazar = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // pwsaAprobar
            // 
            this.pwsaAprobar.AcceptButtonCaption = "Aprobar";
            this.pwsaAprobar.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.pwsaAprobar.CancelButtonCaption = "Cerrar";
            this.pwsaAprobar.Caption = "Aprobar Acción de Personal";
            this.pwsaAprobar.Category = "RecordEdit";
            this.pwsaAprobar.ConfirmationMessage = null;
            this.pwsaAprobar.Id = "pwsaAprobar";
            this.pwsaAprobar.ImageName = "service";
            this.pwsaAprobar.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.pwsaAprobar.TargetObjectsCriteria = "[Estado] == 0";
            this.pwsaAprobar.TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.AccionPersonal);
            this.pwsaAprobar.ToolTip = "Aprobar la Acción de Personal seleccionada";
            this.pwsaAprobar.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.pwsaAprobar_Execute);
            // 
            // pwsaRechazar
            // 
            this.pwsaRechazar.AcceptButtonCaption = "Rechazar";
            this.pwsaRechazar.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.pwsaRechazar.CancelButtonCaption = "Cerrar";
            this.pwsaRechazar.Caption = "Rechazar Acción Personal";
            this.pwsaRechazar.Category = "RecordEdit";
            this.pwsaRechazar.ConfirmationMessage = null;
            this.pwsaRechazar.Id = "pwsaRechazar";
            this.pwsaRechazar.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.pwsaRechazar.TargetObjectsCriteria = "[Estado] == 0";
            this.pwsaRechazar.TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.AccionPersonal);
            this.pwsaRechazar.ToolTip = "Rechazar la acción de personal seleccionada";
            this.pwsaRechazar.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.pwsaRechazar_Execute);
            // 
            // vcAccionPersonal
            // 
            this.Actions.Add(this.pwsaAprobar);
            this.Actions.Add(this.pwsaRechazar);
            this.TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.AccionPersonal);

        }
    }
}
