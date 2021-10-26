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
        [XafDisplayName("Producto Inventario")]
        Producto = 1,
        [XafDisplayName("Bienes o Activo Fijo")]
        Bienes = 2,
        [XafDisplayName("Intangibles")]
        Intangible = 3
    }
}
