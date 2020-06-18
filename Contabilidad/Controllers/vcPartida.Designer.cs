namespace SBT.Apps.Contabilidad.Module.Controllers
{
    partial class vcPartida
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
            this.PwaCierreDiario = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PwaAbrirDias = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PwaCierreMes = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // PwaCierreDiario
            // 
            this.PwaCierreDiario.AcceptButtonCaption = "Aceptar";
            this.PwaCierreDiario.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.PwaCierreDiario.CancelButtonCaption = "Cerrar";
            this.PwaCierreDiario.Caption = "Cierre Diario";
            this.PwaCierreDiario.ConfirmationMessage = null;
            this.PwaCierreDiario.Id = "Partida_CierreDiario";
            this.PwaCierreDiario.ImageName = "CierreDiario";
            this.PwaCierreDiario.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            this.PwaCierreDiario.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PwaCierreDiario.ToolTip = "Realizar el cierre diario contable";
            this.PwaCierreDiario.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PwaCierreDiario.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaCierreDiario_CustomizePopupWindowParams);
            this.PwaCierreDiario.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PwaCierreDiario_Execute);
            // 
            // PwaAbrirDias
            // 
            this.PwaAbrirDias.AcceptButtonCaption = "Aceptar";
            this.PwaAbrirDias.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.PwaAbrirDias.CancelButtonCaption = "Cerrar";
            this.PwaAbrirDias.Caption = "Abrir Días";
            this.PwaAbrirDias.ConfirmationMessage = null;
            this.PwaAbrirDias.Id = "Partida_AbrirDias";
            this.PwaAbrirDias.ImageName = "abrircaja";
            this.PwaAbrirDias.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            this.PwaAbrirDias.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PwaAbrirDias.ToolTip = "Abrir rango de dias que ya se encuentran cerrados";
            this.PwaAbrirDias.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PwaAbrirDias.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaAbrirDias_CustomizePopupWindowParams);
            this.PwaAbrirDias.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PwaAbrirDias_Execute);
            // 
            // PwaCierreMes
            // 
            this.PwaCierreMes.AcceptButtonCaption = "Aceptar";
            this.PwaCierreMes.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.PwaCierreMes.CancelButtonCaption = "Cerrar";
            this.PwaCierreMes.Caption = "Cierre de Mes";
            this.PwaCierreMes.Id = "Partida_CierreMes";
            this.PwaCierreMes.ImageName = "service";
            this.PwaCierreMes.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);
            this.PwaCierreMes.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PwaCierreMes.ToolTip = "Cierre contable de un Mes. Este proceso no puede revertirse";
            this.PwaCierreMes.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PwaCierreMes.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PwaCierreMes_CustomizePopupWindowParams);
            this.PwaCierreMes.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PwaCierreMes_Execute);
            // 
            // vcPartida
            // 
            this.Actions.Add(this.PwaCierreDiario);
            this.Actions.Add(this.PwaAbrirDias);
            this.Actions.Add(this.PwaCierreMes);
            this.TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PwaCierreDiario;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PwaAbrirDias;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PwaCierreMes;
    }
}
