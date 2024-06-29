using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Documentos Asociados al Dte
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class OtrosDocumento
    {
        /// <summary>
        /// Documento asociado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codDocAsociado")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        //[AllowedValues([1, 2, 3, 4])]
        public int CodigoDocumentoAsociado { get; set; }

        /// <summary>
        /// Identificación del documento asociado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(0, 100)]
        public string? DescripcionDocumento { get; set; }

        /// <summary>
        /// Descripción de documento asociado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("detalleDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(0, 300)]
        public string? DetalleDocumento { get; set; }

        /// <summary>
        /// Información del médico que presta el servicio
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("medico")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public Medico? Medico { get; set; }

    }
}
