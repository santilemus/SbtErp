using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los signos tomados en las Consultas. Es la clase para el objeto de negocios ConsultaSigno
    /// y se utiliza para los signos clinicos del paciente y obtenidos en la consulta
    /// </summary>
    ///[DefaultClassOptions]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Signos")]
    [DevExpress.Persistent.Base.ImageNameAttribute("signo")]
    [NavigationItem(false)]
    [DefaultProperty(nameof(Consulta))]
    public class ConsultaSigno: XPObjectBaseBO
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

        public ConsultaSigno(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        #region Propiedades
        [DevExpress.Xpo.AssociationAttribute("Signos-Consulta"), Index(0), Persistent("Consulta"), XafDisplayName("Consulta"),
            RuleRequiredField("ConsultaSigno.Consulta_Requerido", "Save")]
        public Consulta Consulta
        {
            get => _consulta;
            set => SetPropertyValue(nameof(Consulta), ref _consulta, value);
        }

        [Persistent("Problema"), XafDisplayName("Problema Medico"), Index(1), ]
        [RuleRequiredField("ConsultaSigno.Problema_Requerido", "Save", ResultType = ValidationResultType.Warning)]
        [DataSourceCriteria("[Paciente.Oid] == '@This.Consulta.Paciente.Oid'")]
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
