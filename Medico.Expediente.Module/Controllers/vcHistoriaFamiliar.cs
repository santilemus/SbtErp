using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using System;

namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class vcHistoriaFamiliar : ViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction pwsActionEnfermedades;

        public vcHistoriaFamiliar()
        {
            InitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.HistoriaFamiliar);
            //TargetViewType = ViewType.DetailView;
            pwsActionEnfermedades.Caption = "Enfermedades";
            pwsActionEnfermedades.TargetViewType = ViewType.DetailView;
            pwsActionEnfermedades.Caption = "RecordEdit";
            pwsActionEnfermedades.ToolTip = "Seleccior las Enfermedades diagnosticadas al Pariente";
            pwsActionEnfermedades.ImageName = "book_open";

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            pwsActionEnfermedades.CustomizePopupWindowParams += pwsActionEnfermedad_CustomizePopupWindowParams;
            pwsActionEnfermedades.Execute += pwsActionEnfermedad_Execute;
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            pwsActionEnfermedades.CustomizePopupWindowParams -= pwsActionEnfermedad_CustomizePopupWindowParams;
            pwsActionEnfermedades.Execute -= pwsActionEnfermedad_Execute;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void pwsActionEnfermedad_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Enfermedad));
            e.View = Application.CreateListView(objectSpace, typeof(Enfermedad), true);
            //Optionally customize the window display settings.
            //args.Size = new System.Drawing.Size(600, 400);
            //args.Maximized = true;
            //args.IsSizeable = false;
        }

        private void pwsActionEnfermedad_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            HistoriaFamiliar historia = (HistoriaFamiliar)View.CurrentObject;
            foreach (Enfermedad enfermedad in e.PopupWindowViewSelectedObjects)
            {
                if (!string.IsNullOrEmpty(historia.QuePadecen))
                {
                    historia.QuePadecen += Environment.NewLine;
                }
                historia.QuePadecen += enfermedad.Nombre;
            }
            if (((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                View.ObjectSpace.CommitChanges();
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
    }
}
