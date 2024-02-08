using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con los tipos de tabla del impuesto sobre la renta (ISR)
    /// </summary>
    public enum ETablaISR
    {
        [XafDisplayName("Mensual")]
        Mensual = 0,
        [XafDisplayName("Quincenal")]
        Quincenal = 1,
        [XafDisplayName("Semanal")]
        Semanal = 2,
        [XafDisplayName("Junio")]
        Junio = 3,
        [XafDisplayName("Diciembre")]
        Diciembre = 4
    }
}
