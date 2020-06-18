using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Declaracion del objeto persistente que representa un numero de telefono
    /// </summary>
    [DefaultClassOptions, NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfono")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("phone")]
    public class Telefono : XPCustomBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
        }

        private System.Boolean _activo;
        private System.String _descripcion;
        private System.String _numero;
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
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Xpo.KeyAttribute, VisibleInListView(true), VisibleInLookupListView(true)]
        [RuleRequiredField("Telefono.Numero_Requerido", "Save")]
        [RuleUniqueValue("Telefono.Numero_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Numero
        {
            get
            {
                return _numero;
            }
            set
            {
                SetPropertyValue("Oid", ref _numero, value);
            }
        }

        /// <summary>
        /// Descripcion del numero de telefono
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Descripción"), DbType("varchar(100)")]
        [DevExpress.Persistent.Base.ToolTipAttribute("Descripción o comentario del teléfono")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, VisibleInListView(true), VisibleInLookupListView(true)]
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

        /// <summary>
        /// Indica si el telefono se encuentra activo
        /// </summary>
        [DevExpress.Persistent.Base.ToolTipAttribute("Indica sí el télefono esta activo")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, VisibleInListView(true)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("Telefono.Activo_Requerido", "Save")]
        public System.Boolean Activo
        {
            get
            {
                return _activo;
            }
            set
            {
                SetPropertyValue("Activo", ref _activo, value);
            }
        }

    }
}
