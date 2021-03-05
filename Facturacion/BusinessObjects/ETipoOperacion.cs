using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.CxC.Module.BusinessObjects
{
    public enum ETipoOperacion
    {
        Nulo = 0,
        [XafDisplayName("Cargo")]
        Cargo = 1,
        [XafDisplayName("Abono")]
        Abono = 2,
    }
}