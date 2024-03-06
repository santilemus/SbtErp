namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Tipo Operación: Se refiere a la naturaleza de la compra, ya sea como costo o gasto y que,
    /// para efectos de la ley de impuesto sobre la renta, puede ser cualquiera de los valores 
    /// de la lista
    /// </summary>
    /// <remarks>
    /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
    /// y pago a cuenta de febrero del 2024.
    /// </remarks>
    public enum ETipoOperacionCompra
    {
        /// <summary>
        /// Cuando se trata de períodos tributarios anteriores a febrero del 2024
        /// </summary>
        Anterior = 0,
        Gravada = 1,
        /// <summary>
        /// No Gravada o Exenta
        /// </summary>
        Exenta = 2,
        /// <summary>
        /// Excluído o no constituye renta
        /// </summary>
        Excluido = 3
    }
}
