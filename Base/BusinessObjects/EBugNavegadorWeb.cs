using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum EBugNavegadorWeb
    {
        Chrome = 0,
        FireFox = 1,
        [XafDisplayName("Internet Explorer")]
        IExplorer = 2,
        [XafDisplayName("Microsoft Edge")]
        Edge = 3,
        Opera = 4,
        Otro = 5
    }
}