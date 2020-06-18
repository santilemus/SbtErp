using DevExpress.ExpressApp.DC;
using System;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum TipoGenero
    {
        Masculino = 0,
        Femenino = 1,
        [XafDisplayName("No Aplica")]
        NoAplica = 2
    }
}