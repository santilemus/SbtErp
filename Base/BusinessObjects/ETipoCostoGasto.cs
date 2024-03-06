using DevExpress.Xpo;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Tipo de Costo/Gasto: Según la clasificación que se detalle en el formulario F-11 del impuesto
    /// sobre la renta. Pueden ser los de la siguiente lista
    /// </summary>
    /// <remarks>
    /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
    /// y pago a cuenta de febrero del 2024.
    /// </remarks>
    public enum ETipoCostoGasto
    {
        /// <summary>
        /// Cuando se trata de períodos tributarios anteriores a febrero del 2024
        /// </summary>
        Anterior = 0,
        /// <summary>
        /// Gasto de Venta sin Donación
        /// </summary>
        [DisplayName("Gasto de Venta")]
        GastoVenta = 1,
        /// <summary>
        /// Gasto de Administración sin Donación
        /// </summary>
        [DisplayName("Gasto de Administración")]
        GastoAdministracíon = 2,
        /// <summary>
        /// Gastos Financieros sin Donación
        /// </summary>
        [DisplayName("Gasto Financiero")]
        GastoFinanciero = 3,
        /// <summary>
        /// Costo Artículos producidos / comprados importaciones / internaciones
        /// </summary>
        [DisplayName("Costo Artículos Producidos: Importación / Internación")]
        CostoImportacion = 4,
        /// <summary>
        /// Costo Artículos producidos/ comprados interno
        /// </summary>    
        [DisplayName("Costo Artículos Producidos: Compra Interna")]
        CostoCompraLocal = 5,
        /// <summary>
        /// Costos indirectos de fabricación
        /// </summary>
        [DisplayName("Costo Indirecto de Fabricación")]
        CostoIndirecto = 6,
        /// <summary>
        /// Mano de obra
        /// </summary>
        [DisplayName("Mano de Obra")]
        ManoDeObra = 7
    }
}
