using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using System;

namespace SBT.Apps.Medico.Expediente.Module.Controllers
{
    /// <summary>
    /// View Controller que corresponde a las vistas de detalle del BO Consulta
    /// </summary>
    public class ConsultaDetailViewController : ViewController<DetailView>
    {
        private SimpleAction saCancelarConsulta;
        private SimpleAction saIniciarConsulta;
        public ConsultaDetailViewController() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Consulta);
            saCancelarConsulta = new SimpleAction(this, "saCancelarConsulta", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            saCancelarConsulta.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Consulta);
            saCancelarConsulta.TargetViewType = ViewType.DetailView;
            saCancelarConsulta.Caption = "Cancelar";
            saCancelarConsulta.ImageName = "cancelar";
            saCancelarConsulta.ToolTip = "Clic para Cancelar la consulta actual";
            saCancelarConsulta.TargetObjectsCriteria = "[Estado] == 'Espera'";
            saIniciarConsulta = new SimpleAction(this, "saIniciarConsulta", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            saIniciarConsulta.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Consulta);
            saIniciarConsulta.TargetViewType = ViewType.DetailView;
            saIniciarConsulta.Caption = "Iniciar";
            saIniciarConsulta.ToolTip = "Clic para indicar que el paciente inicia la consulta con su medico";
            saIniciarConsulta.ImageName = "aceptar";
            saIniciarConsulta.TargetObjectsCriteria = "[Estado] == 'Espera'";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            saCancelarConsulta.Execute += SaCancelarConsulta_Execute;
            saIniciarConsulta.Execute += SaIniciarConsulta_Execute;
            ObjectSpace.Committing += ObjectSpace_Committing;
            ObjectSpace.Committed += ObjectSpace_Committed;
        }

        protected override void OnDeactivated()
        {
            saCancelarConsulta.Execute -= SaCancelarConsulta_Execute;
            saIniciarConsulta.Execute -= SaIniciarConsulta_Execute;
            ObjectSpace.Committing -= ObjectSpace_Committing;
            ObjectSpace.Committed -= ObjectSpace_Committed;
            base.OnDeactivated();
        }

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View.CurrentObject.GetType() == typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.Consulta))
            {
                if (((Usuario)SecuritySystem.CurrentUser).ClassInfo.FindMember("Empleado") == null)
                    return;
                var medicoSesion = ((Usuario)SecuritySystem.CurrentUser).GetMemberValue("Empleado") as SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico;
                // falta validar que la propiedad Medico del currentObject no sea nulo, para evitar errores desagradables
                if (medicoSesion != null && ((Consulta)View.CurrentObject).Medico.Oid == medicoSesion.Oid &&
                    (((Consulta)View.CurrentObject).Estado == EEstadoConsulta.Iniciada || ((Consulta)View.CurrentObject).Estado == EEstadoConsulta.Espera))
                    ((Consulta)View.CurrentObject).Estado = EEstadoConsulta.Finalizada;
            }
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            if (((Usuario)SecuritySystem.CurrentUser).ClassInfo.FindMember("Empleado") == null)
                return;
            var medicoSesion = ((Usuario)SecuritySystem.CurrentUser).GetMemberValue("Empleado") as SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico;
            if (medicoSesion != null && ((Consulta)View.CurrentObject).Medico.Oid == medicoSesion.Oid)
                ObjectSpace.Refresh();
        }

        private void SaIniciarConsulta_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var consulta = (Consulta)e.CurrentObject;
            consulta.Estado = EEstadoConsulta.Iniciada;
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
            Application.ShowViewStrategy.ShowMessage("Paciente Inicia su Consulta", InformationType.Info);
        }

        private void SaCancelarConsulta_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var consulta = (Consulta)e.CurrentObject;
            consulta.Estado = EEstadoConsulta.Cancelada;
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
            Application.ShowViewStrategy.ShowMessage("Consulta Cancelada", InformationType.Info);
        }

    }
}
