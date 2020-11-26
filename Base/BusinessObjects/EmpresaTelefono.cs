using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Teléfonos")]
    [DevExpress.Persistent.Base.ImageNameAttribute("phone")]
    public class EmpresaTelefono : Telefono
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Tipo = TipoTelefono.Pbx;
        }

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

    }
}
