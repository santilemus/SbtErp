using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Identificacion del Dte
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class Identificacion
    {
        /// <summary>
        /// Versión de la especificación técnica para construir el Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("version")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int Version { get; set; }

        /// <summary>
        /// Ambiente de destino
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ambiente")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Ambiente { get; set; }

        /// <summary>
        /// Tipo de documento
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoDte")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? TipoDte { get; set; }

        /// <summary>
        /// Número de Control
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("numeroControl")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? NumeroControl { get; set; }

        /// <summary>
        /// Código de generación
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codigoGeneracion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? CodigoGeneracion { get; set; }

        /// <summary>
        /// Modelo de Facturación
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoModelo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int TipoModelo { get; set; }

        /// <summary>
        /// Tipo de transmisión
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoOperacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int TipoOperacion { get; set; }

        /// <summary>
        /// Tipo de Contingencia
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoContingencia")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int TipoContingencia { get; set; }

        /// <summary>
        /// Motivo de Contingencia
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("motivoContin")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public object? MotivoContingencia { get; set; }

        /// <summary>
        /// Fecha de Generación
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("fecEmi")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset FechaEmision { get; set; }

        /// <summary>
        /// Hora de Generación
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("horEmi")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.TimeSpan HoraEmision { get; set; }

        /// <summary>
        /// Tipo de Moneda
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tipoMoneda")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? TipoMoneda { get; set; }

    }
}
