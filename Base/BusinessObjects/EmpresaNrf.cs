using DevExpress.Persistent.Base;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Registro Fiscal")]
    [DevExpress.Persistent.Base.ImageNameAttribute("bill-key")]
    public class EmpresaNrf : RegistroFiscal
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //Vigente = true;
        }

        private Empresa _empresa;
        public EmpresaNrf(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.AssociationAttribute("RegistroFiscal-Empresa")]
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
