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
    /// Objeto Persistente que corresponde a las recetas emitiidas en las Consultas. Es la clase para el objeto de negocios de Recetas
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Receta")]
    [DevExpress.Persistent.Base.ImageNameAttribute("BO_Contract")]
    [RuleCombinationOfPropertiesIsUnique("ConsultaReceta.ConsultaMedicamentoUnico", DefaultContexts.Save, "Consulta,Medicamento", SkipNullOrEmptyValues = false)]
    public class ConsultaReceta: XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Cantidad = 0.0m;
            MuestraMedica = false;
        }

        private System.Decimal _cantidad;
        private Medicamento _medicamento;
        private Consulta _consulta;
        private System.Boolean _muestraMedica;
        private System.String _precaucion;
        private System.String _frecuencia;
        private System.String _dosis;
        private Tercero.Module.BusinessObjects.Tercero _farmaceutica;
        private MedicoListas _viaAdministracion;
        public ConsultaReceta(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [RuleRequiredField("ConsultaReceta.Medicamento_Requerido", "Save")]
        public Medicamento Medicamento
        {
            get
            {
                return _medicamento;
            }
            set
            {
                SetPropertyValue("Medicamento", ref _medicamento, value);
            }
        }

        [RuleRequiredField("ConsultaReceta.ViaAdministracion_Requerido", "Save")]
        public MedicoListas ViaAdministracion
        {
            get
            {
                return _viaAdministracion;
            }
            set
            {
                SetPropertyValue("ViaAdministracion", ref _viaAdministracion, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Farmáceutica")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DataSourceCriteria("[TipoPersona] == 2 && [Activo] == True && [Roles][[IdRole] In (4, 8) And [Activo] == True]")]
        public Tercero.Module.BusinessObjects.Tercero Farmaceutica
        {
            get
            {
                return _farmaceutica;
            }
            set
            {
                SetPropertyValue("Farmaceutica", ref _farmaceutica, value);
            }
        }
        [RuleRequiredField("ConsultaReceta.Dosis_Requerido", "Save")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String Dosis
        {
            get
            {
                return _dosis;
            }
            set
            {
                SetPropertyValue("Dosis", ref _dosis, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("ConsultaReceta.Frecuencia_Requerido", "Save")]
        public System.String Frecuencia
        {
            get
            {
                return _frecuencia;
            }
            set
            {
                SetPropertyValue("Frecuencia", ref _frecuencia, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Precaución")]
        [DevExpress.Xpo.SizeAttribute(250)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String Precaucion
        {
            get
            {
                return _precaucion;
            }
            set
            {
                SetPropertyValue("Precaucion", ref _precaucion, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Muestra Médica")]
        [RuleRequiredField("ConsultaReceta.MuestraMedica_Requerido", "Save")]
        public System.Boolean MuestraMedica
        {
            get
            {
                return _muestraMedica;
            }
            set
            {
                SetPropertyValue("MuestraMedica", ref _muestraMedica, value);
            }
        }
        [RuleRequiredField("ConsultaReceta.Cantidad_Requerido", "Save")]
        public System.Decimal Cantidad
        {
            get
            {
                return _cantidad;
            }
            set
            {
                SetPropertyValue("Cantidad", ref _cantidad, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Receta-Consulta")]
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
