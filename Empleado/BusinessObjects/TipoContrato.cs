using DevExpress.ExpressApp.DC;
using System;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    public enum TipoContrato 
    {
        [XafDisplayName("Indefinido")]
        Indefinido = 0,
        [XafDisplayName("Plazo")]
        Plazo = 1,
        [XafDisplayName("SubContrato")]
        SubContratado = 2,
        [XafDisplayName("Otro")]
        Otro = 3
    }
}
