using System.ComponentModel.DataAnnotations;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Propiedades comunes del Emisor y Receptor del DTE.
    /// </summary>
    public class EmisorReceptorBase
    {
        /// <summary>
        /// NRC del Emisor o Receptor del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nrc")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? Nrc { get; set; }

        /// <summary>
        /// Nombre, denominación o razón social del contribuyente
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Length(3, 200)]
        public string? Nombre { get; set; }

        /// <summary>
        /// Código de Actividad Económica del emisor o receptor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? CodigoActividad { get; set; }

        /// <summary>
        /// Actividad Económica del emisor o receptor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Length(1, 150)]
        public string? DescripcionActividad { get; set; }

        /// <summary>
        /// Dirección del Emisor o receptor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("direccion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Direccion? Direccion { get; set; }

        /// <summary>
        /// Teléfono del emisor o receptor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("telefono")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? Telefono { get; set; }

        /// <summary>
        /// Correo electrónico del emisor o receptor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("correo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Correo { get; set; }
    }



}
