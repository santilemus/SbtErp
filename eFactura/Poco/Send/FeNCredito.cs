using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco.Send
{
    /// <summary>
    /// POCO class para construir fe-nc-v3.json
    /// </summary>
    /// <remarks>
    /// Clase para construir el json que corresponde a una <b>nota de crédito a emitir</b> y enviar 
    /// a la facturación electrónica
    /// </remarks>
    public class FeNCredito
    {
        /// <summary>
        /// Identificación del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("identificacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public Identificacion? Identificacion { get; set; }

        /// <summary>
        /// Documentos relacionados al Dte, es opcional
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("documentoRelacionado")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public ICollection<DocumentoRelacionado>? DocumentoRelacionado { get; set; }

        /// <summary>
        /// Información del emisor del Dte, es requerido
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("emisor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public Emisor? Emisor { get; set; }

        /// <summary>
        /// Información del receptor del Dte, es requerido
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("receptor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public Emisor? Receptor { get; set; }

        /// <summary>
        /// Ventas por cuenta de terceros
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ventaTercero")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public Emisor? VentaTercero { get; set; }

        /// <summary>
        /// Cuerpo del documento, corresponde a las líneas de detalle del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("cuerpoDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public ICollection<CuerpoDocumento>? CuerpoDocumento { get; set; }

        /// <summary>
        /// Resumen del Dte. Son los totales del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("resumen")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
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
        public Apendice? Apendice { get; set; }

        /// <summary>
        /// Respuesta generada por la plataforma de facturación electrónica del MH
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("responseMH")]
        public ResponseMH? ResponseMH { get; set; }
    }
}
