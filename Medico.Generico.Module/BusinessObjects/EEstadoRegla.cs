using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    public enum EEstadoRegla
    {
       [XafDisplayName("Por Defecto")]
       PorDefecto = 0,
       [XafDisplayName("Activa")]
       Activa = 1,
       [XafDisplayName("Inactiva")]
       Inactiva = 2
    }
}
