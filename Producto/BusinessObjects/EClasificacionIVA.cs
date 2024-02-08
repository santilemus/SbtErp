namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion EClasificacioIVA
    /// Clasificacion de los bienes y servicios de acuerdo a la base imponible del IVA establecida por la administracion tributaria
    /// </summary>
    public enum EClasificacionIVA
    {
        /// <summary>
        /// Gravado con el Impuesto al Valor Agregado (tasa > 0)
        /// </summary>
        Gravado = 0,
        /// <summary>
        /// Exentos del impuesto al valor agregado. El calculo de Cantidad * precio va en columna Exenta(o)
        /// </summary>
        Exento = 1,
        /// <summary>
        /// Aquellos que por disposicion legal no estan gravados. Para efectos practicos en ES exento y excluidos se comportan igual
        /// </summary>
        Excluido = 2
    }
}
