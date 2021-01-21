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
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcRecordatorioClinico : ViewControllerBase
    {
        public vcRecordatorioClinico()
        {
            InitializeComponent();
            //TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Paciente);
            TargetViewType = ViewType.DetailView;
            //pwsActionSeleccionPlan.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Paciente);
            pwsActionSeleccionPlan.TargetViewType = ViewType.DetailView;
            pwsActionSeleccionPlan.Caption = "Copiar de Plan";
            pwsActionSeleccionPlan.Category = "RecordEdit";
            pwsActionSeleccionPlan.ActionMeaning = ActionMeaning.Accept;
            pwsActionSeleccionPlan.ToolTip = "Generar recordatorios clinicos del paciente a partir de plan seleccionado";
            pwsActionSeleccionPlan.ImageName = "LoadFrom";
            pwsActionSeleccionPlan.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            pwsActionSeleccionPlan.CustomizePopupWindowParams += pwsActionSeleccionPlan_CustomizePopupWindowParams;
            pwsActionSeleccionPlan.Execute += pwsActionSeleccionPlan_Execute;
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            pwsActionSeleccionPlan.CustomizePopupWindowParams -= pwsActionSeleccionPlan_CustomizePopupWindowParams;
            pwsActionSeleccionPlan.Execute -= pwsActionSeleccionPlan_Execute;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void pwsActionSeleccionPlan_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs args)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(PlanMedico));
            args.View = Application.CreateListView(objectSpace, typeof(PlanMedico), true);
            //Optionally customize the window display settings.
            //args.Size = new System.Drawing.Size(600, 400);
            //args.Maximized = true;
            //args.IsSizeable = false;
        }

        private void pwsActionSeleccionPlan_Execute(object sender, PopupWindowShowActionExecuteEventArgs args)
        {
            var rc = (RecordatorioClinico)View.CurrentObject;
            bool existe = false;
            foreach (PlanMedico pl in args.PopupWindowViewSelectedObjects)
            {
                foreach (PlanMedicoDetalle detapl in pl.Detalles)
                {
                    existe = (rc != null) && 
                                (View.ObjectSpace.FindObject<RecordatorioClinico>(CriteriaOperator.Parse("[Paciente] = ? And [Regla] = ?", rc.Paciente, detapl.Regla)) != null);
                    if (rc == null || !existe)
                    {
                        var reClinico = new RecordatorioClinico(((DevExpress.ExpressApp.Xpo.XPObjectSpace)View.ObjectSpace).Session) { Regla = detapl.Regla };
                        reClinico.Save();
                    }
                }
            }
        }
    }
}
