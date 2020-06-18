using DevExpress.ExpressApp.DC;
using System;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    public enum ClasificacionAplicarImpuesto
    {
        [XafDisplayName("Compras")]
        Compras = 1,
        [XafDisplayName("Ventas")]
        Ventas = 2,
        [XafDisplayName("Compra y Venta")]
        CompraVenta = 3
    }
}