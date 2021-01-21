﻿using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    public enum EEstadoOrdenCompra
    {
        Digitada = 0,
        [XafDisplayName("Facturas Emitidas")]
        Factura = 1,
        [XafDisplayName("Pagada Totalmente")]
        Pagada = 2,
        Anulada = 3
    }
}