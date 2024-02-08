using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    /// <summary>
    /// View controller que aplica al BO Paciente
    /// </summary>
    public class vcPaciente : ViewControllerBase
    {
        private PopupWindowShowAction pwsaSelectPlanMedico;
        private XPObjectSpace os;
        public vcPaciente() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaSelectPlanMedico.CustomizePopupWindowParams += pwsaSelectPlanMedico_CustomizePopupWindowParams;
            pwsaSelectPlanMedico.Execute += pwsaSelectPlanMedico_Execute;
        }

        protected override void OnDeactivated()
        {
            pwsaSelectPlanMedico.CustomizePopupWindowParams -= pwsaSelectPlanMedico_CustomizePopupWindowParams;
            pwsaSelectPlanMedico.Execute -= pwsaSelectPlanMedico_Execute;
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Paciente);
            pwsaSelectPlanMedico = new PopupWindowShowAction(this, "pwsaSelectPlanMedico", DevExpress.Persistent.Base.PredefinedCategory.Tools);
            pwsaSelectPlanMedico.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Paciente);
            pwsaSelectPlanMedico.ToolTip = @"Generar Recordatario Clínico a partir de un Plan Medico";
            pwsaSelectPlanMedico.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            pwsaSelectPlanMedico.Caption = @"Plan Medico";
            pwsaSelectPlanMedico.ActionMeaning = ActionMeaning.Accept;
            pwsaSelectPlanMedico.AcceptButtonCaption = @"Copiar";
            pwsaSelectPlanMedico.CancelButtonCaption = @"Cancelar";
            pwsaSelectPlanMedico.ImageName = "LoadFrom";
        }

        private void pwsaSelectPlanMedico_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            os = (XPObjectSpace)Application.CreateObjectSpace(typeof(PlanMedico));
            string planMedicoListViewId = Application.FindLookupListViewId(typeof(PlanMedico));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(os, typeof(PlanMedico), planMedicoListViewId);
            e.View = Application.CreateListView(planMedicoListViewId, collectionSource, true);
        }

        private void pwsaSelectPlanMedico_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (View.CurrentObject != null && e.PopupWindowView.SelectedObjects != null)
            {
                bool existe = false;
                int OidPaciente = ((Paciente)View.CurrentObject).Oid;
                var paciente = os.GetObjectByKey<Paciente>(OidPaciente);
                foreach (PlanMedico pl in e.PopupWindowView.SelectedObjects)
                {
                    foreach (PlanMedicoDetalle detapl in pl.Detalles)
                    {
                        RecordatorioClinico rc = paciente.Session.FindObject<RecordatorioClinico>(CriteriaOperator.Parse("[Paciente.Oid] == ? && [Regla.Oid] == ?",
                            paciente.Oid, detapl.Regla.Oid));
                        if (rc == null)
                        {
                            foreach (RecordatorioClinico obj in paciente.Recordatorios)
                                if (obj.Regla == detapl.Regla)
                                {
                                    existe = true;
                                    break;
                                }
                            if (!existe)
                                paciente.Recordatorios.Add(new RecordatorioClinico(paciente.Session) { Regla = detapl.Regla }); //, Paciente = paciente});
                            existe = false;
                        }
                    }
                }
                paciente.Save();
                os.CommitChanges();
                os.Dispose();
                ((Paciente)View.CurrentObject).Recordatorios.Reload();
            }
        }
    }


}
