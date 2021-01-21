using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Linq;
using DevExpress.Xpo;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Actividad Económica")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Concepto")]
    [DevExpress.Persistent.Base.ImageNameAttribute("business_report")]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Catalogos")]
    public class ActividadEconomica : XPCustomBaseBO
    {

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activa = true;
        }

        private ActividadEconomica _actividadPadre;
        private System.Boolean _activa;
        private System.String _concepto;
        private System.String _codigo;
        public ActividadEconomica(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.SizeAttribute(12), DbType("varchar(12)")]
        [DevExpress.Xpo.KeyAttribute, VisibleInLookupListView(true), Index(0), NonCloneable]
        [RuleRequiredField("ActividadEconomica.Codigo_Requerido", "Save")]
        [RuleUniqueValue("ActividadEconomica.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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
        [DevExpress.Xpo.SizeAttribute(200), Index(1), DbType("varchar(200)"), NonCloneable]
        [RuleRequiredField("ActividadEconomica.Concepto_Requerido", DefaultContexts.Save, "Concepto es requerido")]
        public System.String Concepto
        {
            get
            {
                return _concepto;
            }
            set
            {
                SetPropertyValue("Concepto", ref _concepto, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Actividad Padre"), Index(2)]
        public ActividadEconomica ActividadPadre
        {
            get
            {
                return _actividadPadre;
            }
            set
            {
                SetPropertyValue("ActividadPadre", ref _actividadPadre, value);
            }
        }

        [Index(3), RuleRequiredField("", DefaultContexts.Save)]
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

    }
}
