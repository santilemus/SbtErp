using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.CxC.Module.BusinessObjects
{
    public enum ETipoOperacion
    {
        [XafDisplayName("Cargo")]
        Cargo = 0,
        [XafDisplayName("Abono")]
        Abono = 1
    }
}