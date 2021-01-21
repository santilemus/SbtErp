using System;
using System.Linq;


namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    public enum ETipoMovimientoInventario
    {
        Inicial = 0,
        Compra = 1,
        Ingreso = 2,
        Devolucion = 3,
        Facturado = 4,
        Egreso = 5
    }
}