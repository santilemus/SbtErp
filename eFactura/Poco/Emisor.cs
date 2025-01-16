using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Información del Emisor del Dte. Falta heredarlo de EmisorReceptorBase con las propiedades comunes
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class Emisor
    {
        /// <summary>
        /// Nit del Emisor del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nit")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? Nit { get; set; }

        /// <summary>
        /// NRC del Emisor del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nrc")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? Nrc { get; set; }

        /// <summary>
        /// Nombre, denominación o razón social del contribuyente (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Length(3, 200)]
        public string? Nombre { get; set; }

        /// <summary>
        /// Código de Actividad Económica (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? CodigoActividad { get; set; }

        /// <summary>
        /// Actividad Económica (Emisor)
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        [Length(1, 150)]
        public string? DescripcionActividad { get; set; }

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

        /// <summary>
        /// Dirección del Emisor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("direccion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public Direccion? Direccion { get; set; }

        /// <summary>
        /// Teléfono del emisor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("telefono")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Required]
        public string? Telefono { get; set; }

        /// <summary>
        /// Correo electrónico del emisor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("correo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Correo { get; set; }

        /// <summary>
        /// Código del establecimiento asignado por el MH
        /// </summary>
        /// <remarks>
        /// No va para NC y otros documentos
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("codEstableMH")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? CodEstableMH { get; set; }

        /// <summary>
        /// Código del establecimiento asignado por el contribuyente
        /// </summary>
        /// <remarks>
        /// No va para NC y otros documentos
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("codEstable")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public object? CodEstable { get; set; }

        /// <summary>
        /// Código del Punto de Venta (Emisor) Asignado por el MH
        /// </summary>
        /// <remarks>
        /// No va para NC  otros documentos
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("codPuntoVentaMH")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? CodigoPuntoVentaMH { get; set; }

        /// <summary>
        /// Código del Punto de Venta (Emisor) asignado por el contribuyente
        /// </summary>
        /// <remarks>
        /// No va para NC y otros documentos
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("codPuntoVenta")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? CodigoPuntoVenta { get; set; }

    }
}
