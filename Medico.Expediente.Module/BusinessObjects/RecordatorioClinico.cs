using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Recordatorio Clínico"), NavigationItem(false), Persistent("RecordatorioClinico"),
        DefaultProperty("Regla")]
    [RuleCombinationOfPropertiesIsUnique("RecordatorioClinico.PacientePlanRegla_Unico", DefaultContexts.Save, "Paciente,Regla", 
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, IncludeCurrentObject = true, SkipNullOrEmptyValues = false)]
    [ImageName(nameof(RecordatorioClinico))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class RecordatorioClinico : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RecordatorioClinico(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        EEstadoRegla ajuste = EEstadoRegla.PorDefecto;
        Regla regla;
        Paciente paciente;

        [Association("Paciente-Recordatorios"), ModelDefault("Caption", "Paciente"), Persistent("Paciente"), 
            DbType("int")]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }


        //[Association("Regla-PacientesRecordatorio")]
        [XafDisplayName("Regla"), Persistent("Regla"), RuleRequiredField("RecordatorioClinico.Regla_Requerido", DefaultContexts.Save)]
        public Regla Regla
        {
            get => regla;
            set => SetPropertyValue(nameof(Regla), ref regla, value);
        }

        
        [DbType("smallint"), Persistent("Ajuste"), XafDisplayName("Ajuste"), RuleRequiredField("RecordatorioClinico.Ajuste_Requerido", "Save")]
        public EEstadoRegla Ajuste
        {
            get => ajuste;
            set => SetPropertyValue(nameof(Ajuste), ref ajuste, value);
        }


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}