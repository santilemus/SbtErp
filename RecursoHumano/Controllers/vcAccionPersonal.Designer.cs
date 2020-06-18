namespace SBT.Apps.RecursoHumano.Module.Controllers
{
    partial class vcAccionPersonal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction pwsaAprobar;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction pwsaRechazar;
    }
}
