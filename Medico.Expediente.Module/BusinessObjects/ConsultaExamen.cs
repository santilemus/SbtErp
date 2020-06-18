using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los examenes por consulta. Es la clase para el objeto de negocios ConsultaExamen
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Exámenes")]
    [DevExpress.Persistent.Base.ImageNameAttribute("electrocardiograma")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [RuleCombinationOfPropertiesIsUnique("ConsultaExamen_ExamenFechaUnico", DefaultContexts.Save, "Examen,Fecha", SkipNullOrEmptyValues = false)]  
    [RuleCriteria("ConsultaExamen.FechaPresentacionValida", DefaultContexts.Save, "Not(IsNull([FechaPresentacion])) And FechaPresentacion >= Fecha", "Fecha Presentación debe ser mayor o igual a Fecha")]
    [RuleObjectExists("ConsultaExamen.IdLaboratorioValido",DefaultContexts.Save,"Not(IsNull([FechaPresentacion])) And IsNull([Laboratorio])", InvertResult = true,
     CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues=false, MessageTemplateMustExist = "Debe indicar el laboratorio donde realizó los exámenes")]
    public class ConsultaExamen : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Fecha = DateTime.Now;
            Presentado = false;
        }

        private Tercero.Module.BusinessObjects.Tercero _laboratorio;
        private System.DateTime _fechaPresentacion;
        private Consulta _consulta;
        private DevExpress.Persistent.BaseImpl.FileData _documento;
        private System.String _resultado;
        private System.Boolean _presentado;
        private System.DateTime _fecha;
        private Examen _examen;
        public ConsultaExamen(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Examen")]
        [RuleRequiredField("ConsultaExamen.Examen_Requerido", "Save")]
        public Examen Examen
        {
            get => _examen;
            set => SetPropertyValue("Examen", ref _examen, value);
        }

        [RuleRequiredField("ConsultaExamen.Fecha_Requerido", "Save")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Fecha Presentación")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.DateTime FechaPresentacion
        {
            get
            {
                return _fechaPresentacion;
            }
            set
            {
                SetPropertyValue("FechaPresentacion", ref _fechaPresentacion, value);
            }
        }
        public Tercero.Module.BusinessObjects.Tercero Laboratorio
        {
            get
            {
                return _laboratorio;
            }
            set
            {
                SetPropertyValue("Laboratorio", ref _laboratorio, value);
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("ConsultaExamen.Presentado_Requerido", "Save")]
        public System.Boolean Presentado
        {
            get
            {
                return _presentado;
            }
            set
            {
                SetPropertyValue("Presentado", ref _presentado, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(400)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String Resultado
        {
            get
            {
                return _resultado;
            }
            set
            {
                SetPropertyValue("Resultado", ref _resultado, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public DevExpress.Persistent.BaseImpl.FileData Documento
        {
            get
            {
                return _documento;
            }
            set
            {
                SetPropertyValue("Documento", ref _documento, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Exámenes-Consulta")]
        public Consulta Consulta
        {
            get
            {
                return _consulta;
            }
            set
            {
                SetPropertyValue("Consulta", ref _consulta, value);
            }
        }

    }
}
