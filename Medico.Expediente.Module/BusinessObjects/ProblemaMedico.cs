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
    [DefaultClassOptions, ModelDefault("Caption", "Problema Medico"), NavigationItem(false), XafDefaultProperty(nameof(Diagnostico)), 
        Persistent("ProblemaMedico")]
    [ImageName(nameof(ProblemaMedico))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProblemaMedico : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProblemaMedico(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            var obj = Session.GetObjectByKey<MedicoLista>("PM01");
            if (obj != null)
                Tipo = obj;
            obj = Session.GetObjectByKey<MedicoLista>("PMF07");
            if (obj != null)
                Frecuencia = obj;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        MedicoLista resultado;
        MedicoLista reaccion;
        MedicoLista gravedad;
        string titulo;
        MedicoLista tipo;
        string comentario;
        MedicoLista frecuencia;
        DateTime fechaFin;
        DateTime fechaInicio;
        Enfermedad diagnostico;
        Paciente paciente;

        [Association("Paciente-Problemas"), XafDisplayName("Paciente"), Persistent("Paciente"), Index(0)]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }

        [DbType("varchar(10)"), Persistent("Tipo"), XafDisplayName("Tipo"), RuleRequiredField("ProblemaMedico.Tipo_Requerido", DefaultContexts.Save)]
        [DataSourceCriteria("[Categoria] = 10 And [Activo] = true"), Index(1)]   // Problema Medico
        [ToolTip("Tipo de Problema Medico")]
        public MedicoLista Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [XafDisplayName("Diagnostico"), Index(2), Persistent("Diagnostico")]
        public Enfermedad Diagnostico
        {
            get => diagnostico;
            set
            {
                var oldValue = Diagnostico;
                bool changed = SetPropertyValue(nameof(Diagnostico), ref diagnostico, value);
                if (!IsLoading && !IsSaving && changed)
                    Titulo = value.Nombre.Substring(1, 100);
            }
        }

        [Size(150), DbType("varchar(100)"), Persistent("Titulo"), XafDisplayName("Titulo"),
            RuleRequiredField("ProblemaMedico.Titulo_Requerido", "Save"), VisibleInLookupListView(true), Index(3)]
        public string Titulo
        {
            get => titulo;
            set => SetPropertyValue(nameof(Titulo), ref titulo, value);
        }

        [DbType("datetime"), Persistent("FechaInicio"), XafDisplayName("Fecha Inicio"),
            ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), Index(4)]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        [DbType("datetime"), Persistent("FechaFin"), XafDisplayName("Fecha Fin"),
         ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), Index(5)]
        public DateTime FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        [Persistent("Frecuencia"), XafDisplayName("Frecuencia")]
        [DataSourceCriteria("[Categoria] = 14 And [Activo] = true")]  // lista de valores de Periodicidad
        public MedicoLista Frecuencia
        {
            get => frecuencia;
            set => SetPropertyValue(nameof(Frecuencia), ref frecuencia, value);
        }


        [DbType("varchar(10)"), Persistent("Gravedad"), XafDisplayName("Gravedad")]
        [DataSourceCriteria("[Categoria] = 11 And [Activo] = true")]  // lista de valores de Gravedad
        public MedicoLista Gravedad
        {
            get => gravedad;
            set => SetPropertyValue(nameof(Gravedad), ref gravedad, value);
        }


        [DbType("varchar(10)"), Persistent("Reaccion"), XafDisplayName("Reacción")]
        [DataSourceCriteria("[Categoria] = 12 And [Activo] = true")]   // lista de valores de reaccion
        public MedicoLista Reaccion
        {
            get => reaccion;
            set => SetPropertyValue(nameof(Reaccion), ref reaccion, value);
        }

        [Size(200)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        
        [DbType("varchar(10)"), Persistent("Resultado"), XafDisplayName("Resultado")]
        [DataSourceCriteria("[Categoria] = 13 And [Activo] = true")]   // lista de valores de resultado de problemas medico
        public MedicoLista Resultado
        {
            get => resultado;
            set => SetPropertyValue(nameof(Resultado), ref resultado, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}