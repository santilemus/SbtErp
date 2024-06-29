using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a los Pacientes. Hereda del BO Persona
    /// </summary>
    /// <remarks>
    /// 26/dic/2020 agregado por SELM
    /// Se agrega el atributo [ExplicitLoading] para las propiedades que son referencia a otros BO y que no se 
    /// necesitan en en ListView y que además es poco probable que se usen en el codigo para que se carguen hasta que
    /// se necesiten y reducir el numero de consultas que se ejecutan cuando se carga la opción y por lo tanto aumentar
    /// la velocidad. Evaluar si se aplica a otras propiedades
    /// </remarks>
    [DefaultClassOptions]
    [ModelDefault("Caption", "Paciente"), NavigationItem("Salud"), DefaultProperty(nameof(NombreCompleto)), ImageName("Paciente")]
    [ListViewFilter("Paciente - Esperando Consulta", "[Consultas][[Estado] = 'Espera']", "Pacientes Esperando Consulta")]
    [ListViewFilter("Paciente - Consultas del Dia", "[Consultas][GetDay([Fecha]) = GetDay(CURRENTDATE())]", "Pacientes - Consultas del día")]
    [ListViewFilter("Paciente - Consulta del día Finalizada", "[Consultas][[Estado] = 'Finalizada' && GetDay([Fecha]) = GetDay(CURRENTDATE())]", "Pacientes - Consulta Finalizada")]
    [ListViewFilter("Paciente - Consulta Medica. Todas", "")]
    public class Paciente : Persona
    {
        private ZonaGeografica _nacionalidad;
        private Empresa _empresa;
        private System.Boolean _activo = true;
        private System.String _referencia;
        private Tercero.Module.BusinessObjects.Tercero _patrono;
        private System.String _ocupacion;
        private System.String _lugarNacimiento;
        private System.String _polizaSeguro;
        private Listas _tipoSeguro;
        private Tercero.Module.BusinessObjects.Tercero _aseguradora;
        public Paciente(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FechaIngreso = DateTime.Now;
            if (Empresa != null)   // por defecto asigna el pais de la empresa (OJO...importante esa propiedad de empresa)
            {
                if (Empresa.Pais != null)
                    Pais = Empresa.Pais;
            }
            if (Pais == null)     // si la propiedad empresa no existe, asigna como pais El Salvador (revisar si se deja asi)
            {
                var fPais = Session.GetObjectByKey<ZonaGeografica>("SLV");
                if (fPais != null)
                    Pais = fPais;
            }
            if (Pais != null)
                Nacionalidad = Pais;
        }

        [DbType("int"), Persistent("Empresa"), XafDisplayName("Empresa"), VisibleInListView(false), VisibleInLookupListView(false)]
        [ExplicitLoading]
        public Empresa Empresa
        {
            get
            {
                return _empresa;
            }
            set
            {
                SetPropertyValue("Empresa", ref _empresa, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, Size(60), DbType("varchar(60)"), Persistent("LugarNacimiento"),
            XafDisplayName("Lugar Nacimiento")]
        public System.String LugarNacimiento
        {
            get
            {
                return _lugarNacimiento;
            }
            set
            {
                SetPropertyValue("LugarNacimiento", ref _lugarNacimiento, value);
            }
        }

        [Size(100), DbType("varchar(100)"), Persistent("Ocupacion"), XafDisplayName("Ocupación")]
        public System.String Ocupacion
        {
            get
            {
                return _ocupacion;
            }
            set
            {
                SetPropertyValue("Ocupacion", ref _ocupacion, value);
            }
        }

        [DbType("int"), Persistent("Patrono"), XafDisplayName("Patrono")]
        [ExplicitLoading]
        public Tercero.Module.BusinessObjects.Tercero Patrono
        {
            get
            {
                return _patrono;
            }
            set
            {
                SetPropertyValue("Patrono", ref _patrono, value);
            }
        }

        [DevExpress.Xpo.Size(200), DbType("varchar(200)"), Persistent("Referencia")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, XafDisplayName("Referencia")]
        public System.String Referencia
        {
            get
            {
                return _referencia;
            }
            set
            {
                SetPropertyValue("Referencia", ref _referencia, value);
            }
        }

        [Persistent("Nacionalidad"), XafDisplayName("Nacionalidad")]
        [DataSourceCriteria("[ZonaPadre] is null and [Activa] = true")]
        [ExplicitLoading]
        public ZonaGeografica Nacionalidad
        {
            get
            {
                return _nacionalidad;
            }
            set
            {
                SetPropertyValue("Nacionalidad", ref _nacionalidad, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, Persistent("TipoSeguro"),
            XafDisplayName("Tipo Seguro")]
        [DataSourceCriteria("[Categoria] = 'TipoSeguro'")]
        [ExplicitLoading]
        public Listas TipoSeguro
        {
            get
            {
                return _tipoSeguro;
            }
            set
            {
                SetPropertyValue("TipoSeguro", ref _tipoSeguro, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false),
            Persistent("Aseguradora"), XafDisplayName("Aseguradora")]
        [DataSourceCriteria("[TipoPersona] = 'Juridica' And [Activo] = True And [Roles][[IdRole] = 'Aseguradora' And [Activo] = True]")]
        [ExplicitLoading]
        public Tercero.Module.BusinessObjects.Tercero Aseguradora
        {
            get
            {
                return _aseguradora;
            }
            set
            {
                SetPropertyValue("Aseguradora", ref _aseguradora, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(12), DbType("varchar(12)"), Persistent("PolizaSeguro"), XafDisplayName("Póliza Seguro")]
        public System.String PolizaSeguro
        {
            get
            {
                return _polizaSeguro;
            }
            set
            {
                SetPropertyValue("PolizaSeguro", ref _polizaSeguro, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, DbType("bit"), Persistent("Activo"), XafDisplayName("Activo")]
        public System.Boolean Activo
        {
            get
            {
                return _activo;
            }
            set
            {
                SetPropertyValue("Activo", ref _activo, value);
            }
        }


        [PersistentAlias("DateDiffMonth([FechaNacimiento], Now())")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal EdadMeses
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(EdadMeses))); }
        }

        [DevExpress.Xpo.AssociationAttribute("Parientes-Paciente")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Parientes"), Index(4), DevExpress.Xpo.Aggregated]
        [ToolTip("Parientes, es importante cuando se trata de menores de edad o personas con incapacidad. Además, pueden ser pacientes")]
        public XPCollection<Pariente> Parientes
        {
            get
            {
                return GetCollection<Pariente>("Parientes");
            }
        }

        [DevExpress.Xpo.AssociationAttribute("Medicos-Paciente"), Index(5), DevExpress.Xpo.Aggregated]
        [ToolTip("Medicos que tratan al paciente. Pueden ser diferentes especialistas")]
        public XPCollection<PacienteMedico> Medicos
        {
            get
            {
                return GetCollection<PacienteMedico>("Medicos");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Vacunas-Paciente"), Index(6), DevExpress.Xpo.Aggregated]
        [ToolTip("Registro de las Vacunas aplicadas")]
        public XPCollection<PacienteVacuna> Vacunas
        {
            get
            {
                return GetCollection<PacienteVacuna>("Vacunas");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Consultas-Paciente")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Consultas"), Index(7), DevExpress.Xpo.Aggregated]
        [ToolTip("Historial de las consultas medicas")]
        public XPCollection<Consulta> Consultas
        {
            get
            {
                return GetCollection<Consulta>("Consultas");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Paciente-Citas"), Index(8)]
        [ToolTip("Historial de la programación de citas con el paciente")]
        public XPCollection<Cita> Citas
        {
            get
            {
                return GetCollection<Cita>("Citas");
            }
        }

        [Association("Paciente-Recordatorios"), DevExpress.Xpo.Aggregated, ModelDefault("Caption", "Recordatorios"),
            Index(9)]
        [ToolTip("Recordatorios clínicos. Puede dar clic en la Acción Plan Medico para convertir las reglas del plan en recordatorios clínicos")]
        public XPCollection<RecordatorioClinico> Recordatorios
        {
            get
            {
                return GetCollection<RecordatorioClinico>(nameof(Recordatorios));
            }
        }

        [Association("Paciente-HistoriaFamiliares"), DevExpress.Xpo.Aggregated, XafDisplayName("Historia Familiar"), Index(10)]
        [ToolTip("Historial de padecimientos de los parientes más cercanos ")]
        public XPCollection<HistoriaFamiliar> HistoriaFamiliares
        {
            get
            {
                return GetCollection<HistoriaFamiliar>(nameof(HistoriaFamiliares));
            }
        }

        [Association("Paciente-EstilosVida"), DevExpress.Xpo.Aggregated, XafDisplayName("Estilo Vida"), Index(11)]
        [ToolTip("Actividades que realiza el paciente y que impactan su salud")]
        public XPCollection<EstiloVida> EstilosVida
        {
            get
            {
                return GetCollection<EstiloVida>(nameof(EstilosVida));
            }
        }

        [Association("Paciente-Problemas"), DevExpress.Xpo.Aggregated]
        [XafDisplayName("Problemas Medicos"), Index(13)]
        [ToolTip("Problemas medicos a los cuales es necesario prestar atención. Se deben a: enfermedades, medicación, lesiones, alergías, etc")]
        public XPCollection<ProblemaMedico> Problemas
        {
            get
            {
                return GetCollection<ProblemaMedico>(nameof(Problemas));
            }
        }

        [Association("Paciente_ConsultaNutricion"), DevExpress.Xpo.Aggregated]
        [System.ComponentModel.DisplayName("Nutrición")]
        public XPCollection<ConsultaNutricion> Nutriciones => GetCollection<ConsultaNutricion>(nameof(Nutriciones));

        [Association("Paciente-ArchivosAdjuntos"), DevExpress.Xpo.Aggregated, XafDisplayName("Archivos Adjuntos"), Index(14)]
        [ToolTip("Archivos y documentos relacionados al historial medico del paciente")]
        public XPCollection<PacienteFileData> ArchivosAdjuntos => GetCollection<PacienteFileData>(nameof(ArchivosAdjuntos));
    }
}
