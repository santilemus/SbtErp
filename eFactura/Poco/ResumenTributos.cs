using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Resumen de Tributos
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class ResumenTributos
    {
        /// <summary>
        /// Código del tributo
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codigo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Length(2, 2)]
        public string? Codigo { get; set; }

        /// <summary>
        /// Nombre del Tributo
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descripcion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(2, 150)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Valor del tributo
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("valor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Valor { get; set; }

    }
}
