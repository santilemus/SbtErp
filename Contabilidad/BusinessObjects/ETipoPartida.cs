using System;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    public enum ETipoPartida
    {
        Apertura = 0,       // A
        Diario = 1,         // D
        Ingreso = 2,        // I
        Egreso = 3,         // E
        Liquidacion = 4,    // L
        Cierre = 5,         // C
    }
}
