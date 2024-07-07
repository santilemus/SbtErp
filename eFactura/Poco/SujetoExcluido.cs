using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Sujeto Excluido receptor de la factura electrónica de sujeto excluído
    /// </summary>
    public class SujetoExcluido
    {
        /// <summary>
        /// Tipo de documento de identificación (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [MaxLength(2)]
        public string? TipoDocumento { get; set; }

        /// <summary>
        /// Número de documento de Identificación (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("numDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 20)]
        public string? NumDocumento { get; set; }

        /// <summary>
        /// Nombre, denominación o razón social del contribuyente (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 250)]
        public string? Nombre { get; set; }

        /// <summary>
        /// Código de Actividad Económica (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? CodActividad { get; set; }

        /// <summary>
        /// Actividad Económica (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 150)]
        public string? DescripcionActividad { get; set; }

        /// <summary>
        /// Dirección (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("direccion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Direccion? Direccion { get; set; }

        /// <summary>
        /// Teléfono (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("telefono")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Telefono { get; set; }

        /// <summary>
        /// Correo electrónico (Receptor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("correo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Correo { get; set; }
    }
}
