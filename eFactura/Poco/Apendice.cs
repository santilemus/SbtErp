using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Apendice
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class Apendice
    {
        /// <summary>
        /// Nombre del campo
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("campo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(2, 25)]
        public string? Campo { get; set; }

        /// <summary>
        /// Descripción del campo, de su contenido o uso
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("etiqueta")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(3, 50)]
        public string? Etiqueta { get; set; }

        /// <summary>
        /// Valor/Dato
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("valor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 150)]
        public string? Valor { get; set; }

    }
}
