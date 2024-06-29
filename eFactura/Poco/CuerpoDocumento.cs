using System;
using System.ComponentModel.DataAnnotations;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Cuerpo del Documento. Son las líneas de detalle del Dte
    /// </summary>
    public class CuerpoDocumento
    {
        /// <summary>
        /// No de ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("numItem")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Range(1, 2000)]
        public int NumeroItem { get; set; }

        /// <summary>
        /// Tipo de ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoItem")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Range(1, 4)]
        public int TipoItem { get; set; }

        /// <summary>
        /// Número de documento relacionado. Es el Id del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("numeroDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 36)]
        [Required]
        public string? NumeroDocumento { get; set; }

        /// <summary>
        /// Código del item 
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codigo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 25)]
        public string? Codigo { get; set; }

        /// <summary>
        /// Tributo sujeto a cálculo de IVA.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codTributo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(2, 2)]
        public string? CodTributo { get; set; }

        /// <summary>
        /// Descripción del ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descripcion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 1000)]
        [Required]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Cantidad del ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("cantidad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("uniMedida")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Range(1, 99)]
        public int UnidadMedida { get; set; }

        /// <summary>
        /// Precio Unitario
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("precioUni")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal PrecioUnidad { get; set; }

        /// <summary>
        /// Descuento, Bonificación, Rebajas por ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("montoDescu")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal MontoDescuento { get; set; }

        /// <summary>
        /// Ventas no sujetas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ventaNoSuj")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal VentaNoSujeta { get; set; }

        /// <summary>
        /// Ventas exentas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ventaExenta")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal VentaExenta { get; set; }

        /// <summary>
        /// Ventas Gravadas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ventaGravada")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal VentaGravada { get; set; }

        /// <summary>
        /// Otros tributos aplicados al ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tributos")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<string>? Tributos { get; set; }

        /// <summary>
        /// Precio sugerido de venta
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("psv")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Psv { get; set; }

        /// <summary>
        /// Cargos / Abonos que no afectan la base imponible
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("noGravado")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal NoGravado { get; set; }
    }
}
