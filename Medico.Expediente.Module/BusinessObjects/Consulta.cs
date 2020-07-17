using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a las Consultas. Es la clase para el objeto de negocios de Consultas
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Consulta")]
    [DevExpress.Persistent.Base.ImageNameAttribute("planning-customer")]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Salud")]
    [RuleIsReferenced("Consulta_Referencia", DefaultContexts.Delete, typeof(Consulta), nameof(Oid),
       MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
       InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [RuleCombinationOfPropertiesIsUnique("Consulta_PacienteMedicoFechaUnico", DefaultContexts.Save, "Paciente,Medico,Fecha", SkipNullOrEmptyValues = false)]
    [RuleCriteria("Consulta.ProximaCitaValida", DefaultContexts.Save, "ProximaCita >= Fecha", "Fecha de próxima cita debe ser mayor o igual a Fecha")]
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
            Empresa = EmpresaDeSesion();
        }

        private Generico.Module.BusinessObjects.Medico _medico;
        private Paciente _paciente;
        private System.String _unidadDeRemision;
        private EmpresaUnidad _consultorio;
        private System.Boolean _realizarExamenes;
        private System.DateTime _proximaCita;
        private Empresa _empresa;
        private System.String _diagnostico;
        private System.DateTime _fecha;
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
        [DevExpress.Xpo.SizeAttribute(1000)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Consulta.Diagnostico_Requerido", "Save")]
        [DevExpress.ExpressApp.Model.ModelDefault("RowCount", "6")]
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

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Institución Médica")]
        [DevExpress.Xpo.NonPersistentAttribute]
        [RuleRequiredField("Consulta.IdEmpresa_Requerido", "Save")]
        public Empresa Empresa
        {
            get => _empresa;
            set => SetPropertyValue("Empresa", ref _empresa, value);
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Unidad de Remisión")]
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
        [DevExpress.Xpo.AssociationAttribute("Exámenes-Consulta"), DevExpress.Xpo.Aggregated]
        public XPCollection<ConsultaExamen> Exámenes
        {
            get
            {
                return GetCollection<ConsultaExamen>("Exámenes");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("ExamenesFisicos-Consulta"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Exámenes Físicos")]
        public XPCollection<ConsultaExamenFisico> ExamenesFisicos
        {
            get
            {
                return GetCollection<ConsultaExamenFisico>("ExamenesFisicos");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Signos-Consulta"), DevExpress.Xpo.Aggregated]
        public XPCollection<ConsultaSigno> Signos
        {
            get
            {
                return GetCollection<ConsultaSigno>("Signos");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Sintomas-Consulta"), DevExpress.Xpo.Aggregated]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Síntomas")]
        public XPCollection<ConsultaSintoma> Sintomas
        {
            get
            {
                return GetCollection<ConsultaSintoma>("Sintomas");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Receta-Consulta"), DevExpress.Xpo.Aggregated]
        public XPCollection<ConsultaReceta> Receta
        {
            get
            {
                return GetCollection<ConsultaReceta>("Receta");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Incapacidades-Consulta"), DevExpress.Xpo.Aggregated]
        public XPCollection<ConsultaIncapacidad> Incapacidades
        {
            get
            {
                return GetCollection<ConsultaIncapacidad>("Incapacidades");
            }
        }

    }
}
