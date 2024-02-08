using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con los tipos de compra
    /// </summary>
    public enum ETipoCompra
    {
        Servicio = 0,
        [XafDisplayName("Inventario")]
        Inventario = 1,
        [XafDisplayName("Activo Fijo")]
        Activo = 2,
        [XafDisplayName("Intangibles")]
        Intangible = 3
    }
}
