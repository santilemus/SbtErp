﻿using DevExpress.ExpressApp.DC;
using System;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum ETipoCuentaBanco
    {
        [XafDisplayName("Ahorros")]
        Ahorros = 1,
        [XafDisplayName("Corriente")]
        Corriente = 2,
        [XafDisplayName("A Plazos")]
        Plazos = 3
    }
}
