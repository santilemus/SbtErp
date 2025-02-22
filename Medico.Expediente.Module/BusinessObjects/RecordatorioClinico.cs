﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Recordatorio Clínico"), NavigationItem(false), Persistent("RecordatorioClinico"),
        DefaultProperty("Regla"), CreatableItem(false)]
    [RuleCombinationOfPropertiesIsUnique("RecordatorioClinico.PacientePlanRegla_Unico", DefaultContexts.Save, "Paciente,Regla",
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
    [ImageName(nameof(RecordatorioClinico))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class RecordatorioClinico : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RecordatorioClinico(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Ajuste = EEstadoRegla.PorDefecto;
        }


        EEstadoRegla ajuste;
        Regla regla;
        Paciente paciente;

        [Association("Paciente-Recordatorios"), ModelDefault("Caption", "Paciente"), Persistent(nameof(Paciente))]
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


        [DbType("smallint"), Persistent("Ajuste"), XafDisplayName("Ajuste")]
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