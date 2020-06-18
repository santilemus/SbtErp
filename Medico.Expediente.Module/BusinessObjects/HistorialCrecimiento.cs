using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using DevExpress.ExpressApp;
using System.ComponentModel;
using System.Data;
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
    [DefaultClassOptions]
    [ImageName(nameof(HistorialCrecimiento))]
    [DefaultProperty("Fecha"), ModelDefault("Caption", "Historial Crecimiento"), Persistent("HistorialCrecimiento"), NavigationItem(false)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class HistorialCrecimiento : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public HistorialCrecimiento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        decimal bMI;
        string comentario;
        decimal peso;
        decimal estatura;
        CdoPercentilPesoEstaturaBMI tabla_2_20Anios;
        WhoPercentilPesoLong tabla_0_2Anios;
        decimal edadMes;
        DateTime fecha;
        Paciente paciente;

        [Association("Paciente-HistorialCrecimientos")]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DbType("numeric(5,2)")]
        public decimal EdadMes
        {
            get => edadMes;
            set => SetPropertyValue(nameof(EdadMes), ref edadMes, value);
        }


       // [Association("WhoPercentilPesoLong-HistorialCrecimientos")]
        public WhoPercentilPesoLong Tabla_0_2Anios
        {
            get => tabla_0_2Anios;
            set => SetPropertyValue(nameof(Tabla_0_2Anios), ref tabla_0_2Anios, value);
        }


        //[Association("CdoPercentilPesoEstaturaBMI-HistorialCrecimientos")]
        public CdoPercentilPesoEstaturaBMI Tabla_2_20Anios
        {
            get => tabla_2_20Anios;
            set => SetPropertyValue(nameof(Tabla_2_20Anios), ref tabla_2_20Anios, value);
        }

        [DbType("numeric(5,2)"), XafDisplayName("Peso (Kg)")]
        public decimal Peso
        {
            get => peso;
            set => SetPropertyValue(nameof(Peso), ref peso, value);
        }

        [DbType("numeric(5,2)"), XafDisplayName("Estatura (cm)")]
        public decimal Estatura
        {
            get => estatura;
            set => SetPropertyValue(nameof(Estatura), ref estatura, value);
        }

        [DbType("numeric(9,6)"), XafDisplayName("Indice BMI")]
        public decimal BMI
        {
            get => bMI;
            set => SetPropertyValue(nameof(BMI), ref bMI, value);
        }

        [Size(400), DbType("varchar(400)"), Persistent("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}