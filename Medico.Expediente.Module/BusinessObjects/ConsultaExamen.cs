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
using DevExpress.ExpressApp.Model;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los examenes por consulta. Es la clase para el objeto de negocios ConsultaExamen
    /// </summary>
    [NavigationItem(false), CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Exámenes")]
    [DevExpress.Persistent.Base.ImageNameAttribute("electrocardiograma")]
    [RuleCombinationOfPropertiesIsUnique("ConsultaExamen_ExamenFechaUnico", DefaultContexts.Save, "Examen,Fecha", SkipNullOrEmptyValues = false)]  
    public class ConsultaExamen : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Fecha = DateTime.Today;
        }

        private Tercero.Module.BusinessObjects.Tercero laboratorio;
        private Nullable<System.DateTime> fechaPresentacion;
        private Consulta consulta;
        private PacienteFileData documento;
        private System.String resultado;
        private System.DateTime fecha;
        private Examen examen;
        public ConsultaExamen(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Examen"), ImmediatePostData(true)]
        [RuleRequiredField("ConsultaExamen.Examen_Requerido", "Save")]
        public Examen Examen
        {
            get => examen;
            set => SetPropertyValue(nameof(Examen), ref examen, value);
        }

        [RuleRequiredField("ConsultaExamen.Fecha_Requerido", "Save")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public System.DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Fecha Presentación")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleValueComparison("ConsultaExamen.FechaPresentacion >= Fecha", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "[Fecha]", ParametersMode.Expression,
            TargetCriteria = "[Examen.Categoria.Codigo] != 'EX001'")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public Nullable<System.DateTime> FechaPresentacion
        {
            get => fechaPresentacion;
            set => SetPropertyValue(nameof(FechaPresentacion), ref fechaPresentacion, value);
        }

        [XafDisplayName("Laboratorio")]
        [DataSourceCriteria("[Roles][[IdRole] In (4, 5)]")]
        [RuleRequiredField("ConsultaExamen.Laboratorio_Requerido", DefaultContexts.Save, TargetCriteria = "[Examen.Categoria.Codigo] != 'EX001'")]
        public Tercero.Module.BusinessObjects.Tercero Laboratorio
        {
            get => laboratorio;
            set => SetPropertyValue(nameof(Laboratorio), ref laboratorio, value);
        }

        [DevExpress.Xpo.SizeAttribute(400), DbType("varchar(400)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String Resultado
        {
            get => resultado;
            set => SetPropertyValue(nameof(Resultado), ref resultado, value);
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public PacienteFileData Documento
        {
            get => documento;
            set => SetPropertyValue(nameof(Documento), ref documento, value);
        }
        [DevExpress.Xpo.AssociationAttribute("Exámenes-Consulta")]
        public Consulta Consulta
        {
            get => consulta;
            set => SetPropertyValue("Consulta", ref consulta, value);
        }

    }
}
