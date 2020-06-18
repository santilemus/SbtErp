using DevExpress.Persistent.Base;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los números de registro fiscal asociados a un tercero
    /// </summary>
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Registro Fiscal")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("bill-key")]
    public class TerceroNrf : RegistroFiscal
    {
        private Tercero tercero;
        public TerceroNrf(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.AssociationAttribute("Tercero-Nrcs")]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

    }
}
