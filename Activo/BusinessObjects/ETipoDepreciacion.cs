using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    public enum ETipoDepreciacion
    {
        [XafDisplayName("Depreciación del Activo")]
        Activo = 0,
        [XafDisplayName("Ajuste Depreciación")]
        Ajuste = 1,
        [XafDisplayName("Depreciación Versión")]
        Version = 2,
        [XafDisplayName("Depreciación de Mejora")]
        Mejora = 3
    }
}