using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Declaración del objeto persistente que Representa lo números de registro fiscal
    /// </summary>
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Registro Fiscal")]
    [DevExpress.Xpo.NonPersistentAttribute]
    public class RegistroFiscal : XPCustomBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Vigente = true;
        }

        private ActividadEconomica actEconomica;
        private System.Boolean vigente;
        private System.String comentario;
        private System.String nrf;
        public RegistroFiscal(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        /// <summary>
        /// Número de registro Fiscal
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("No Registro Fiscal")]
        [Size(12), DbType("varchar(12)"), VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.Xpo.KeyAttribute]
        [RuleRequiredField("RegistroFiscal.Nrf_Requerido", "Save")]
        [RuleUniqueValue("RegistroFiscal.Nrf_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Nrf
        {
            get => nrf;
            set => SetPropertyValue(nameof(Nrf), ref nrf, value);
        }

        [Size(60), DbType("varchar(60)")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Actividad Económica")]
        [RuleRequiredField("RegistroFiscal.ActEconomica_Requerido", DefaultContexts.Save, "Actividad Económica es requerida")]
        public ActividadEconomica ActEconomica
        {
            get => actEconomica;
            set => SetPropertyValue(nameof(ActEconomica), ref actEconomica, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostData(true), DevExpress.ExpressApp.DC.XafDisplayName("Vigente")]
        public System.Boolean Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

    }
}
