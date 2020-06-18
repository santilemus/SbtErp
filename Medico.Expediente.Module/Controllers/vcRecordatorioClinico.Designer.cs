namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    partial class vcRecordatorioClinico
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
            this.pwsActionSeleccionPlan = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // pwsActionSeleccionPlan
            // 
            this.pwsActionSeleccionPlan.AcceptButtonCaption = null;
            this.pwsActionSeleccionPlan.CancelButtonCaption = null;
            this.pwsActionSeleccionPlan.Caption = null;
            this.pwsActionSeleccionPlan.ConfirmationMessage = null;
            this.pwsActionSeleccionPlan.Id = "ed425b0e-5521-49e1-8c59-9bd48a644b8c";
            this.pwsActionSeleccionPlan.ToolTip = null;
            // 
            // vcRecordatorio
            // 
            this.Actions.Add(this.pwsActionSeleccionPlan);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction pwsActionSeleccionPlan;
    }
}
