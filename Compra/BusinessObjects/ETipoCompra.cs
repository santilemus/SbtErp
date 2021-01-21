using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con los tipos de compra
    /// </summary>
    public enum ETipoCompra
    {
        Servicio = 0,
        [XafDisplayName("Produto Inventario")]
        Producto = 1,
        [XafDisplayName("Activo Fijo e Intangibles")]
        ActivoFijo = 2
    }
}
