using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum ETipoDato
    {
        [XafDisplayName("bool")]
        tBoolean = 1,
        [XafDisplayName("int")]
        tInt32  = 2,
        [XafDisplayName("decimal")]
        tDecimal = 3,
        [XafDisplayName("DateTime")]
        tDateTime = 4,
        [XafDisplayName("string")]
        tString = 5,
        [XafDisplayName("object")]
        tObject = 6
    }
}
