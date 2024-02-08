using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfonos")]
    [DevExpress.Persistent.Base.ImageNameAttribute("phone")]
    [RuleCombinationOfPropertiesIsUnique("EmpresaTelefono.NumeroUnico", DefaultContexts.Save, "Empresa;Telefono",
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
    [DefaultProperty(nameof(Telefono))]
    public class EmpresaTelefono : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        Telefono telefono;
        private Empresa _empresa;
        public EmpresaTelefono(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Xpo.AssociationAttribute("Telefonos-Empresa")]
        public Empresa Empresa
        {
            get
            {
                return _empresa;
            }
            set
            {
                SetPropertyValue("Empresa", ref _empresa, value);
            }
        }


        [XafDisplayName("Telefono")]
        [ExplicitLoading]
        public Telefono Telefono
        {
            get => telefono;
            set => SetPropertyValue(nameof(Telefono), ref telefono, value);
        }

    }
}
