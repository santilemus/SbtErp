using System;
using System.ComponentModel.DataAnnotations;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Venta por cuenta de tercero
    /// </summary>
    public class VentaTercero
    {
        /// <summary>
        /// NIT por cuenta de Terceros
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nit")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? Nit { get; set; }

        /// <summary>
        /// Nombre, denominación o razón social del Tercero
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Length(3, 200)]
        public string? Nombre { get; set; }
    }
}
