namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Enumeración para identificar los tipos de partidas contables
    /// <br>Apertura = 0</br>
    /// <br>Diario = 1</br>
    /// <br>Ingreso = 2</br>
    /// <br>Egreso = 3</br>
    /// <br>Liquidacion = 4</br>
    /// <br>Cierre = 5</br>
    /// </summary>
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
