namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Clasificación: El contribuyente deberá de identificar a qué tipo de erogación corresponden 
    /// las deducciones del impuesto sobre la renta, respecto de cada compra de bienes o servicios
    /// que realice en cada período tributario, que podrán ser considerados de acuerdo a la lista 
    /// que se proporciona
    /// </summary>
    /// <remarks>
    /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
    /// y pago a cuenta de febrero del 2024.
    /// </remarks>
    public enum EClasificacionRenta
    {
        /// <summary>
        /// Cuando se trata de períodos tributarios anteriores a febrero del 2024
        /// </summary>
        Anterior = 0,
        Costo = 1,
        Gasto = 2
    }
}
