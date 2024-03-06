namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Sector: Es el rubro o actividad económica genérica a que se dedica el sujeto pasivo
    /// </summary>
    /// <remarks>
    /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
    /// y pago a cuenta de febrero del 2024.
    /// </remarks>
    public enum ESectorSujetoPasivo
    {
        /// <summary>
        /// Cuando se trata de períodos tributarios anteriores a febrero del 2024
        /// </summary>
        Anterior = 0,
        Industria = 1,
        Comercio = 2,
        Agropecuaria = 3,
        /// <summary>
        /// Servicios, profesiones, artes y oficios
        /// </summary>
        Servicio = 4
    }
}
