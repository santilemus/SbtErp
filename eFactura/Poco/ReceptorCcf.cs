using System.ComponentModel.DataAnnotations;

namespace SBT.eFactura.Dte.Poco
{
    public class ReceptorCcf : EmisorReceptorBase
    {
        /// <summary>
        /// Nit del Emisor del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nit")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? Nit { get; set; }

        /// <summary>
        /// Nombre Comercial (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombreComercial")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 150)]
        public object? NombreComercial { get; set; }

        /// <summary>
        /// Tipo de establecimiento (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoEstablecimiento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? TipoEstablecimiento { get; set; }
    }



}
