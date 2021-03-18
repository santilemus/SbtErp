using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Moneda")]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Catalogos")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    [DevExpress.Persistent.Base.ImageNameAttribute("money")]
    [RuleIsReferenced("Moneda_Referencia", DefaultContexts.Delete, typeof(Moneda), nameof(Codigo),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]

    public class Moneda : XPCustomObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            FactorCambio = 0.0m;
            Activa = true;
        }

        private System.Decimal _factorCambio;
        private System.Boolean _activa;
        private System.String _plural;
        private System.String _nombre;
        private System.String _codigo;
        public Moneda(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        #region Propiedades
        [DevExpress.Xpo.SizeAttribute(3), DbType("varchar(3)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [DevExpress.Xpo.KeyAttribute, VisibleInLookupListView(true)]
        [RuleUniqueValue("Moneda.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [RuleRequiredField("Moneda.CodMoneda_Requerido", "Save")]
        public System.String Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                SetPropertyValue("Codigo", ref _codigo, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Nombre")]
        [DevExpress.Xpo.SizeAttribute(60), DbType("varchar(60)")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Nombre de la moneda")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Moneda.Nombre_Requerido", "Save")]
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
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ToolTipAttribute("Factor de cambio o valor con respecto a la moneda base")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Moneda.FactorCambio_Requerido", "Save")]
        [DbType("numeric(12,2)")]
        public System.Decimal FactorCambio
        {
            get
            {
                return _factorCambio;
            }
            set
            {
                SetPropertyValue("FactorCambio", ref _factorCambio, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(25), DbType("varchar(25)")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.ToolTipAttribute("Plural de la moneda")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Moneda.Plural_Requerido", "Save")]
        public System.String Plural
        {
            get
            {
                return _plural;
            }
            set
            {
                SetPropertyValue("Plural", ref _plural, value);
            }
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Moneda.Activa_Requerido", "Save")]
        public System.Boolean Activa
        {
            get
            {
                return _activa;
            }
            set
            {
                SetPropertyValue("Activa", ref _activa, value);
            }
        }

        #endregion

        #region Metodos

        #endregion 

    }
}
