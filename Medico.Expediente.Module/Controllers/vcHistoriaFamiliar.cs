using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcHistoriaFamiliar : ViewController
    {
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

        private void pwsActionEnfermedad_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs args)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Enfermedad));
            args.View = Application.CreateListView(objectSpace, typeof(Enfermedad), true);
            //Optionally customize the window display settings.
            //args.Size = new System.Drawing.Size(600, 400);
            //args.Maximized = true;
            //args.IsSizeable = false;
        }

        private void pwsActionEnfermedad_Execute(object sender, PopupWindowShowActionExecuteEventArgs args)
        {
            HistoriaFamiliar historia = (HistoriaFamiliar)View.CurrentObject;
            foreach (Enfermedad enfermedad in args.PopupWindowViewSelectedObjects)
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
    }
}
