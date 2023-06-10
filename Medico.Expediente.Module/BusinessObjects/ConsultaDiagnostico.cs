using DevExpress.ExpressApp.DC;
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
    /// Objeto Persistente que corresponde a los signos tomados en las Consultas. Es la clase para el objeto de negocios ConsultaSigno
    /// y se utiliza para los signos clinicos del paciente y obtenidos en la consulta
    /// </summary>
    ///[DefaultClassOptions]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Consulta Diagnóstico")]
    [DevExpress.Persistent.Base.ImageNameAttribute("signo")]
    [NavigationItem(false)]
    [DefaultProperty(nameof(Consulta))]
    public class ConsultaDiagnostico : XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Consulta _consulta;
        private System.String _descripcion;
        private ProblemaMedico _problema;

        public ConsultaDiagnostico(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        #region Propiedades
        [DevExpress.Xpo.AssociationAttribute("Diagnostico-Consulta"), Index(0), Persistent("Consulta"), XafDisplayName("Consulta"),
            RuleRequiredField("ConsultaSigno.Consulta_Requerido", "Save")]
        [VisibleInListView(false)]
        public Consulta Consulta
        {
            get => _consulta;
            set => SetPropertyValue(nameof(Consulta), ref _consulta, value);
        }

        [Persistent("Problema"), XafDisplayName("Problema Medico"), Index(1),]
        [RuleRequiredField("ConsultaSigno.Problema_Requerido", "Save", ResultType = ValidationResultType.Warning)]
        [DataSourceCriteria("[Paciente.Oid] == '@This.Consulta.Paciente.Oid'")]
        [ExplicitLoading]
        public ProblemaMedico Problema
        {
            get => _problema;
            set => SetPropertyValue(nameof(Problema), ref _problema, value);
        }

        [DevExpress.Xpo.SizeAttribute(250), DbType("varchar(250)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Descripción")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), Index(2)]
        [RuleRequiredField("ConsultaSigno.Descripcion_Requerido", "Save")]
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

        #endregion 
    }
}
