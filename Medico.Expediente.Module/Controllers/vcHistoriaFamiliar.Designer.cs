namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    partial class vcHistoriaFamiliar
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
            this.pwsActionEnfermedades = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // pwsActionEnfermedades
            // 
            this.pwsActionEnfermedades.AcceptButtonCaption = null;
            this.pwsActionEnfermedades.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.pwsActionEnfermedades.CancelButtonCaption = null;
            this.pwsActionEnfermedades.Caption = "Enfermedades";
            this.pwsActionEnfermedades.Category = "RecordEdit";
            this.pwsActionEnfermedades.ConfirmationMessage = null;
            this.pwsActionEnfermedades.Id = "a9cadd06-5f47-4629-b69e-9b3b3d8d87f6";
            this.pwsActionEnfermedades.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.HistoriaFamiliar);
            this.pwsActionEnfermedades.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.pwsActionEnfermedades.ToolTip = null;
            this.pwsActionEnfermedades.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // vcHistoriaFamiliar
            // 
            this.Actions.Add(this.pwsActionEnfermedades);
            this.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.HistoriaFamiliar);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction pwsActionEnfermedades;
    }
}
