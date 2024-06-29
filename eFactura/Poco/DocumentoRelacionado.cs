using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Documentos relacionados al Dte
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class DocumentoRelacionado
    {
        /// <summary>
        /// Tipo de Documento Tributario Relacionado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? TipoDocumento { get; set; }

        /// <summary>
        /// Tipo de Generación del Documento Tributario relacionado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoGeneracion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int TipoGeneracion { get; set; }

        /// <summary>
        /// Número de documento relacionado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("numeroDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? NumeroDocumento { get; set; }

        /// <summary>
        /// Fecha de Generación del Documento Relacionado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("fechaEmision")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset FechaEmision { get; set; }

    }
}
