using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los Sintomas presentados por el paciente en una consulta. Es la clase para el objeto de negocios de ConsultaSintoma
    /// </summary>
    //[DefaultClassOptions]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("list-info")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Síntomas")]
    [XafDefaultProperty(nameof(Descripcion))]
    [NavigationItem(false)]
    [RuleCriteria("ConsultaSintoma.FechaFin_Valida", DefaultContexts.Save, "Not(IsNull([FechaFin])) And FechaFin >= FechaInicio", "Fecha Fin debe ser mayor o igual Fecha Inicio")]
    public class ConsultaSintoma : XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FechaInicio = DateTime.Now;
        }

        private Consulta _consulta;
        private Nullable<System.DateTime> _fechaFin;
        private System.DateTime _fechaInicio;
        private System.String _descripcion;
        private System.String _nombre;
        private MedicoLista _intensidad;
        public ConsultaSintoma(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [RuleRequiredField("ConsultaSintoma.Intensidad_Requerido", "Save")]
        [DataSourceCriteria("[Categoria] == 2")]
        [ExplicitLoading]
        public MedicoLista Intensidad
        {
            get
            {
                return _intensidad;
            }
            set
            {
                SetPropertyValue("Intensidad", ref _intensidad, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(250), DbType("varchar(250)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("ConsultaSintoma.Nombre_Requerido", "Save")]
        public System.String Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                SetPropertyValue("Nombre", ref _nombre, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Descripción")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DbType("varchar(100)")]
        public System.String Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                SetPropertyValue("Descripcion", ref _descripcion, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
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
        public Nullable<System.DateTime> FechaFin
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
        [DevExpress.Xpo.AssociationAttribute("Sintomas-Consulta")]
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
