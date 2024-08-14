using System.ComponentModel.DataAnnotations;

namespace SBT.eFactura.Dte.Poco.Receive
{
    /// <summary>
    /// POCO class para construir fe-fse-v1.json
    /// </summary>
    /// <remarks>
    /// Clase para construir el json que corresponde a una <b>factura de sujeto excluída emitida para un proveedor excluído</b> y enviar 
    /// al módulo de compras
    /// </remarks>
    public class FeFse
    {

        [System.Text.Json.Serialization.JsonPropertyName("identificacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Identificacion? Identificacion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("emisor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Emisor? Emisor { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("sujetoExcluido")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public SujetoExcluido? SujetoExcluido { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("cuerpoDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public ICollection<CuerpoDocumento>? CuerpoDocumento { get; set; }

        /// <summary>
        /// Resumen del dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("resumen")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public ResumenFSE? Resumen { get; set; }


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
