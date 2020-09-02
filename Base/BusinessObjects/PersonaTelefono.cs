using DevExpress.Persistent.Base;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Télefonos")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    public class PersonaTelefono : Telefono
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Persona _persona;
        public PersonaTelefono(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.AssociationAttribute("Telefonos-Persona")]
        public Persona Persona
        {
            get => _persona;
            set =>SetPropertyValue("Persona", ref _persona, value);
        }

    }
}
