using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. Tipos de Saldos Contables
    /// </summary>
    public enum ETipoSaldoDia
    {
        Apertura = 0,
        [XafDisplayName("Operaciones del Ejercicio")]
        Operaciones = 1,
        [XafDisplayName("Liquidación")]
        Liquidacion = 2,
        Cierre = 3
    }
}
