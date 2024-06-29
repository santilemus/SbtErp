using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Cuando el emisor del Dte esta relacionado al sector saludo
    /// </summary>
    public class Medico
    {
        /// <summary>
        /// Nombre de médico que presta el Servicio
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(0, 100)]
        public string? Nombre { get; set; }

        /// <summary>
        /// NIT de médico que presta el Servicio
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nit")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Nit { get; set; }

        /// <summary>
        /// Documento de identificación de médico no domiciliados
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("docIdentificacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? DocumentoIdentificacion { get; set; }

        /// <summary>
        /// Código del Servicio realizado
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoServicio")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int TipoServicio { get; set; }

    }
}
