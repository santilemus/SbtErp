﻿using System;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    public enum EEstadoDepreciacion
    {
        [XafDisplayName("En Depreciación")]
        Depreciando = 0,
        [XafDisplayName("Depreciado")]
        Depreciado = 1,
        Otros = 2
    }
}
