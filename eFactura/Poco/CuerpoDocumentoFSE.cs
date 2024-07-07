using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Cuerpo del Documento de factura de sujeto excluido. Son las líneas de detalle del Dte
    /// </summary>
    public class CuerpoDocumentoFSE
    {
        /// <summary>
        /// No de ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("numItem")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Range(1, 2000)]
        public int NumItem { get; set; }

        /// <summary>
        /// Tipo de Item
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoItem")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Range(1, 3)]
        public int TipoItem { get; set; }

        /// <summary>
        /// Cantidad del ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("cantidad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Cantidad { get; set; }

        /// <summary>
        /// Código del item 
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codigo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 25)]
        public string? Codigo { get; set; }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("uniMedida")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Range(1, 99)]
        public int UnidadMedida { get; set; }

        /// <summary>
        /// Descripción del ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descripcion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 1000)]
        [Required]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Precio Unitario
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("precioUni")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal? PrecioUnidad { get; set; }

        /// <summary>
        /// Descuento, Bonificación, Rebajas por ítem
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("montoDescu")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal MontoDescuento { get; set; }

        /// <summary>
        /// Monto de la Venta (del sujeto excluido) y compra para el emisor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("compra")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public decimal? Compra { get; set; }

    }

}
