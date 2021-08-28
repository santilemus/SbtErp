using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a las incapacidades emitidas en las consultas. Es la clase para el objeto de negocios incapacidades
    /// </summary>
    [NavigationItem(false)]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("BO_Lead")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Incapacidades")]
    [RuleCombinationOfPropertiesIsUnique("ConsultaIncapacidad_ConsultaFechaInicioFechaFin", DefaultContexts.Save, "Consulta,FechaInicio,FechaFin", SkipNullOrEmptyValues = false)]
    [RuleCriteria("ConsultaIncapacidad.FechaFinValida", DefaultContexts.Save, "Not(IsNull([FechaFin])) And FechaFin >= FechaInicio", "Fecha Fin debe ser mayor o igual a Fecha Inicio")]
    public class ConsultaIncapacidad : XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FechaInicio = DateTime.Now;
            FechaFin = DateTime.Now;
        }

        private Consulta _consulta;
        private System.String _motivo;
        private System.DateTime _fechaFin;
        private System.DateTime _fechaInicio;
        public ConsultaIncapacidad(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [RuleRequiredField("ConsultaIncapacidad.FechaInicio_Requerido", "Save")]
        public System.DateTime FechaInicio
        {
            get
            {
                return _fechaInicio;
            }
            set
            {
                SetPropertyValue("FechaInicio", ref _fechaInicio, value);
            }
        }
        [RuleRequiredField("ConsultaIncapacidad.FechaFin_Requerido", "Save")]
        public System.DateTime FechaFin
        {
            get
            {
                return _fechaFin;
            }
            set
            {
                SetPropertyValue("FechaFin", ref _fechaFin, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(300), DbType("varchar(300)")]
        [RuleRequiredField("ConsultaIncapacidad.Motivo_Requerido", "Save")]
        public System.String Motivo
        {
            get
            {
                return _motivo;
            }
            set
            {
                SetPropertyValue("Motivo", ref _motivo, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Incapacidades-Consulta"), VisibleInLookupListView(true)]
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
