using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Declaracion del objeto persistente que representa un numero de telefono
    /// </summary>
    [DefaultClassOptions, NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfono")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("phone")]
    [DefaultProperty(nameof(Numero))]
    public class Telefono : XPCustomObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
        }

        TipoTelefono tipo = TipoTelefono.Movil;
        private System.Boolean activo;
        private System.String numero;
        public Telefono(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        /// <summary>
        /// Oid de telefono
        /// </summary>
        [Size(14), DbType("varchar(14)")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Número de teléfono")]
        [DevExpress.Persistent.Base.DataSourcePropertyAttribute("No Télefono")]
        [DevExpress.Xpo.KeyAttribute, VisibleInListView(true), VisibleInLookupListView(true)]
        [RuleRequiredField("Telefono.Numero_Requerido", "Save")]
        [RuleUniqueValue("Telefono.Numero_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        [DbType("smallint"), XafDisplayName("Tipo"), RuleRequiredField("Telefono.Tipo_Requerido", DefaultContexts.Save)]
        [VisibleInLookupListView(true)]
        public TipoTelefono Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        /// <summary>
        /// Indica si el telefono se encuentra activo
        /// </summary>
        [DevExpress.Persistent.Base.ToolTipAttribute("Indica sí el télefono esta activo")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, VisibleInListView(true)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Telefono.Activo_Requerido", "Save")]
        public System.Boolean Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

    }
}
