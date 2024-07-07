using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// POCO para construir fe-ccf-v3.json
    /// </summary>
    public class FeCcf
    {
        /// <summary>
        /// Identificación del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("identificacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Identificacion? Identificacion { get; set; }

        /// <summary>
        /// Documentos relacionados al Dte, es opcional
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("documentoRelacionado")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<DocumentoRelacionado>? DocumentoRelacionado { get; set; }

        /// <summary>
        /// Información del emisor del Dte, es requerido
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("emisor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Emisor? Emisor { get; set; }

        /// <summary>
        /// Información del receptor del Dte, es requerido
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("receptor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Emisor? Receptor { get; set; }

        /// <summary>
        /// Documentos asociados al Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("otrosDocumentos")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<OtrosDocumento>? OtrosDocumentos { get; set; }

        /// <summary>
        /// Ventas por cuenta de terceros
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ventaTercero")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public VentaTercero? VentaTercero { get; set; }

        /// <summary>
        /// Cuerpo del documento, corresponde a las líneas de detalle del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("cuerpoDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public System.Collections.Generic.ICollection<CuerpoDocumento>? CuerpoDocumento { get; set; }

        /// <summary>
        /// Resumen del Dte. Son los totales del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("resumen")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Resumen? Resumen { get; set; }

        /// <summary>
        /// Extensión del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("extension")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public Extension? Extension { get; set; }

        /// <summary>
        /// Apendice del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("apendice")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<Apendice>? Apendice { get; set; }


    }
}
