using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Dirección del emisor del Dte
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class Direccion
    {
        /// <summary>
        /// Dirección Departamento (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("departamento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Departamento { get; set; }

        /// <summary>
        /// Dirección Municipio (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("municipio")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Municipio { get; set; }

        /// <summary>
        /// Dirección complemento (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("complemento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 200)]
        public string? Complemento { get; set; }

    }
}
