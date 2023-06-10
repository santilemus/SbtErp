using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Télefonos")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [RuleCombinationOfPropertiesIsUnique("PersonaTelefono.NumeroUnico", DefaultContexts.Save, "Persona;Telefono",
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
    [DefaultProperty(nameof(Telefono))]
    public class PersonaTelefono : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        Telefono telefono;
        private Persona _persona;
        public PersonaTelefono(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.AssociationAttribute("Telefonos-Persona")]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue("Persona", ref _persona, value);
        }

        [XafDisplayName("Telefono")]
        public Telefono Telefono
        {
            get => telefono;
            set
            {
                SetPropertyValue(nameof(Telefono), ref telefono, value);
            }
        }

    }
}
