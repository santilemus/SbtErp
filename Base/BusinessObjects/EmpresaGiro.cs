using DevExpress.Persistent.Base;
using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Registro Fiscal")]
    [DevExpress.Persistent.Base.ImageNameAttribute("bill-key")]
    [Persistent(nameof(EmpresaGiro))]
    public class EmpresaGiro : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        bool vigente = true;
        private Empresa empresa;
        private ActividadEconomica actEconomica;

        public EmpresaGiro(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Xpo.AssociationAttribute("Empresa-Giros")]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [VisibleInListView(true), VisibleInLookupListView(true)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Actividad Económica")]
        [RuleRequiredField("RegistroFiscal.ActEconomica_Requerido", DefaultContexts.Save, "Actividad Económica es requerida")]
        public ActividadEconomica ActEconomica
        {
            get => actEconomica;
            set => SetPropertyValue(nameof(ActEconomica), ref actEconomica, value);
        }

        [DbType("bit"), DevExpress.ExpressApp.DC.XafDisplayName("Vigente")]
        public bool Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

    }

}
