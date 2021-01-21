using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum EBugSistemaOperativo
    {
        [XafDisplayName("Windows 7")]
        Windows7 = 0,
        [XafDisplayName("Windows 8.x")]
        Windows8 = 1,
        [XafDisplayName("Windows 10")]
        Windows10 = 2,
        Linux = 3,
        Osx = 4,
        [XafDisplayName("Windows Server")]
        WindowServer,
        Android,
        Otro
    }
}