﻿using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a las recetas emitiidas en las Consultas. Es la clase para el objeto de negocios de Recetas
    /// </summary>
    [NavigationItem(false)]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Receta")]
    [DevExpress.Persistent.Base.ImageNameAttribute("BO_Contract")]
    [RuleCombinationOfPropertiesIsUnique("ConsultaReceta.ConsultaMedicamentoUnico", DefaultContexts.Save, "Consulta,Medicamento", SkipNullOrEmptyValues = false)]
    public class ConsultaReceta : XPObject
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
        private MedicamentoVia _viaAdministracion;
        public ConsultaReceta(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [RuleRequiredField("ConsultaReceta.Medicamento_Requerido", "Save"), ImmediatePostData(true), XafDisplayName("Medicamento")]
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
        [DataSourceProperty("Medicamento.Vias")]
        [ExplicitLoading]
        public MedicamentoVia ViaAdministracion
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
        [VisibleInListView(false)]
        [DataSourceCriteria("[TipoPersona] == 2 && [Activo] == True && [Roles][[IdRole] In (4, 8) And [Activo] == True]")]
        [ExplicitLoading]
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
        [DbType("varchar(100)")]
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
        [DbType("varchar(100)")]
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
        [DevExpress.Xpo.SizeAttribute(250), DbType("varchar(250)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [VisibleInListView(false)]
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
        [VisibleInListView(false)]
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
        [DbType("numeric(12,2)")]
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
