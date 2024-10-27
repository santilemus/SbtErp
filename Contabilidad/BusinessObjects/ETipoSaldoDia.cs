using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. Tipos de Saldos Contables
    /// <br>Apertura = 0</br>
    /// <br>Operaciones del Ejercicio = 1</br>
    /// <br>Liquidacion = 2</br>
    /// <br>Cierre = 3</br>
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
