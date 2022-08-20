using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a las Consultas. Es la clase para el objeto de negocios de Consultas
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Consulta")]
    [DevExpress.Persistent.Base.ImageNameAttribute("planning-customer")]
    [DevExpress.Persistent.Base.NavigationItem("Salud")]
    [DefaultProperty(nameof(Paciente))]
    [RuleIsReferenced("Consulta_Referencia", DefaultContexts.Delete, typeof(Consulta), nameof(Oid),
       MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
       InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [RuleCombinationOfPropertiesIsUnique("Consulta_PacienteMedicoFechaUnico", DefaultContexts.Save, "Paciente,Medico,Fecha", SkipNullOrEmptyValues = false)]
    [RuleCriteria("Consulta.ProximaCitaValida", DefaultContexts.Save, "ProximaCita >= Fecha", "Fecha de próxima cita debe ser mayor o igual a Fecha")]
    [Appearance("Ultrasonografia_Hide", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide,
        Context = "DetailView", Criteria = "[Paciente.Genero] != 1 && [Paciente.Edad] >= 10", TargetItems = "UltrasonografiaObstetricas;UltrasonografiaPelvicas")]

    [ListViewFilter("Consulta Medica. En Espera", "[Estado] == 'Espera'", "Esperando Turno con Medico", "Consultas procesadas por recepción y esperando turno con medico asignado")]
    [ListViewFilter("Consulta Medica. Iniciada", "[Estado] == 'Iniciada'", "Pacientes en consultorios", "Pacientes que estan siendo atendidos")]
    [ListViewFilter("Consulta Medica. Paciente siendo atendido", "[Estado] == 'Iniciada' && [Medico.Oid] == EmpleadoActualOid()", 
        "Paciente con su Medico", "Consulta atendida por medico de la sesión")]
    [ListViewFilter("Consulta Medica. Finalizada", "[Estado] == 'Finalizada'", "Consultas finalizadas", "Consultas que el medico ha dado por finalizadas")]
    [ListViewFilter("Consulta Medica. Cancelada", "[Estado] == 'Cancelada'", "Consultas Canceladas", "Pacientes que se retiraron antes de pasar consulta")]
    [ListViewFilter("Consulta Medica. Del dia de hoy", "GetDate([Fecha]) == Today()", "Consultas del dia de hoy")]
    [ListViewFilter("Consulta Medica. De la Semana", "IsThisWeek([Fecha])", "Consultas de la Semana Actual")]
    [ListViewFilter("Consulta Medica. Del Mes Actual", "IsThisMonth([Fecha])", "Consultas del Mes Actual")]
    [ListViewFilter("Consulta Medica. Todas", "")]
    public class Consulta : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Fecha = DateTime.Now;
            RealizarExamenes = true;
            if (((Usuario)SecuritySystem.CurrentUser).Agencia != null)
            {
                // se  hace de esta forma, no solo se asigna la agencia al consultorio porque da error. La informacion del usuario es una sesion diferente
                int idConsultorio = ((Usuario)SecuritySystem.CurrentUser).Agencia.Oid;
                Consultorio = Session.GetObjectByKey<EmpresaUnidad>(idConsultorio);
            }
            // para obtener el medico de la sesion y asignarlo en caso que se trate de un medico
            var ci = ((Usuario)SecuritySystem.CurrentUser).ClassInfo;
            if (ci.FindMember("Empleado") == null)
                return;
            Empleado.Module.BusinessObjects.Empleado empleado = (((Usuario)SecuritySystem.CurrentUser).GetMemberValue("Empleado") as Empleado.Module.BusinessObjects.Empleado);
            if (empleado == null)
                return;   // la propiedad existe, pero no tiene valor
            Generico.Module.BusinessObjects.Medico doc = Session.GetObjectByKey<Generico.Module.BusinessObjects.Medico>(empleado.Oid);
            if (doc != null)
                Medico = doc;
            estado = EEstadoConsulta.Espera;
        }

        //   SBT.Apps.Producto.Module.BusinessObjects.ProductoPrecio precio;
        //   SBT.Apps.Producto.Module.BusinessObjects.Producto producto;
        private Generico.Module.BusinessObjects.Medico _medico;
        private Paciente _paciente;
        private System.String _unidadDeRemision;
        private EmpresaUnidad _consultorio;
        private System.Boolean _realizarExamenes;
        private System.DateTime _proximaCita;
        private Empresa empresa;
        private System.String _diagnostico;
        private System.DateTime _fecha;
        private EEstadoConsulta estado;

        public Consulta(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.AssociationAttribute("Consultas-Paciente")]
        [RuleRequiredField("Consulta.Paciente_Requerido", "Save")]
        public Paciente Paciente
        {
            get
            {
                return _paciente;
            }
            set
            {
                SetPropertyValue("Paciente", ref _paciente, value);
            }
        }

        [RuleRequiredField("Consulta.Medico_Requerido", "Save")]
        [ExplicitLoading]
        public Generico.Module.BusinessObjects.Medico Medico
        {
            get => _medico;
            set => SetPropertyValue("Medico", ref _medico, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Consulta.Fecha_Requerido", "Save")]
        public System.DateTime Fecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                SetPropertyValue("Fecha", ref _fecha, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(1000), DbType("varchar(1000)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Consulta.Diagnostico_Requerido", "Save")]
        [DevExpress.ExpressApp.Model.ModelDefault("RowCount", "5")]
        public System.String Diagnostico
        {
            get
            {
                return _diagnostico;
            }
            set
            {
                SetPropertyValue("Diagnostico", ref _diagnostico, value);
            }
        }

        [VisibleInDetailView(false), VisibleInListView(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Institución Médica")]
        [Persistent(nameof(Empresa))]
        [ExplicitLoading]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Próxima Cita")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.DateTime ProximaCita
        {
            get
            {
                return _proximaCita;
            }
            set
            {
                SetPropertyValue("ProximaCita", ref _proximaCita, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Realizar Exámenes")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.Boolean RealizarExamenes
        {
            get
            {
                return _realizarExamenes;
            }
            set
            {
                SetPropertyValue("RealizarExamenes", ref _realizarExamenes, value);
            }
        }

        [RuleRequiredField("Consulta.Consultorio_Requerido", "Save")]
        [DataSourceCriteria("[Role] == 2")]
        [ExplicitLoading]
        public EmpresaUnidad Consultorio
        {
            get
            {
                return _consultorio;
            }
            set
            {
                SetPropertyValue("Consultorio", ref _consultorio, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Unidad de Remisión"), DbType("varchar(100)")]
        public System.String UnidadDeRemision
        {
            get
            {
                return _unidadDeRemision;
            }
            set
            {
                SetPropertyValue("UnidadDeRemision", ref _unidadDeRemision, value);
            }
        }

        [XafDisplayName("Estado"), DbType("smallint")]
        public EEstadoConsulta Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }


        //[DevExpress.ExpressApp.DC.XafDisplayName("Producto")]
        //[DataSourceCriteria("[Categoria.Clasificacion] == 4 && [Categoria.EsGrupo] == False && [Categoria.Activa] == true && [Activo] == True")]
        //public SBT.Apps.Producto.Module.BusinessObjects.Producto Producto
        //{
        //    get => producto;
        //    set => SetPropertyValue(nameof(Producto), ref producto, value);
        //}

        //[DevExpress.ExpressApp.DC.XafDisplayName("Precio")]
        //[DataSourceProperty("Precios")]
        //public SBT.Apps.Producto.Module.BusinessObjects.ProductoPrecio Precio
        //{
        //    get => precio;
        //    set => SetPropertyValue(nameof(Precio), ref precio, value);
        //}

        [DevExpress.Xpo.AssociationAttribute("Exámenes-Consulta"), DevExpress.Xpo.Aggregated]
        [ToolTip("Exámenes practicados al paciente, pueden ser: físicos, pruebas de laboratorio o pruebas medicas")]
        public XPCollection<ConsultaExamen> Exámenes
        {
            get
            {
                return GetCollection<ConsultaExamen>("Exámenes");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("ExamenesFisicos-Consulta"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Exámenes Físicos")]
        [ToolTip("Exámenes físicos practicados al paciente para la consulta actual")]
        public XPCollection<ConsultaExamenFisico> ExamenesFisicos
        {
            get
            {
                return GetCollection<ConsultaExamenFisico>("ExamenesFisicos");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Diagnostico-Consulta"), DevExpress.Xpo.Aggregated]
        [ToolTip("Diagnosticos o padecimientos del paciente e identificados en la consulta")]
        public XPCollection<ConsultaDiagnostico> Diagnosticos
        {
            get
            {
                return GetCollection<ConsultaDiagnostico>(nameof(Diagnosticos));
            }
        }

        [Association("ConsultaSigno-Consulta"), DevExpress.Xpo.Aggregated]
        [ToolTip("Registro de los signos tomados al paciente en la consulta")]
        public XPCollection<ConsultaSigno> ConsultaSignos => GetCollection<ConsultaSigno>(nameof(ConsultaSignos));

        [DevExpress.Xpo.AssociationAttribute("Sintomas-Consulta"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Síntomas")]
        [ToolTip("Sintomas experimentados por el paciente")]
        public XPCollection<ConsultaSintoma> Sintomas
        {
            get
            {
                return GetCollection<ConsultaSintoma>("Sintomas");
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Receta-Consulta"), DevExpress.Xpo.Aggregated]
        [ToolTip("Medicamentos recetados")]
        public XPCollection<ConsultaReceta> Receta
        {
            get
            {
                return GetCollection<ConsultaReceta>("Receta");
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Incapacidades-Consulta"), DevExpress.Xpo.Aggregated]
        [ToolTip("Incapacidades otorgadas")]
        public XPCollection<ConsultaIncapacidad> Incapacidades
        {
            get
            {
                return GetCollection<ConsultaIncapacidad>("Incapacidades");
            }
        }

        [Association("Consulta-UltraSonografiaObstetricas"), DevExpress.Xpo.Aggregated, XafDisplayName("Ecografía Obstetrica")]
        [ToolTip("Información de las ultrasonografías obstetricas practicadas a paciente femenino")]
        public XPCollection<Ginecologia.UltraSonografiaObstetrica> UltrasonografiaObstetricas => GetCollection<Ginecologia.UltraSonografiaObstetrica>(nameof(UltrasonografiaObstetricas));

        [Association("Consulta-UltrasonografiaPelvicas"), DevExpress.Xpo.Aggregated, XafDisplayName("Ecografía Pélvica")]
        [ToolTip("Información de las ultrasonografías pelvicas practicadas a paciente femenino")]
        public XPCollection<Ginecologia.UltrasonografiaPelvica> UltrasonografiaPelvicas => GetCollection<Ginecologia.UltrasonografiaPelvica>(nameof(UltrasonografiaPelvicas));

        #region Metodos

        #endregion
    }
}
