using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Extensión Dte
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class Extension
    {
        /// <summary>
        /// Nombre del responsable que Genera el DTE
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombEntrega")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 100)]
        public string? NombreEntrega { get; set; }

        /// <summary>
        /// Documento de identificación de quien genera el DTE
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("docuEntrega")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 25)]
        public string? DocumentoEntrega { get; set; }

        /// <summary>
        /// Nombre del responsable de la operación por parte del receptor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("nombRecibe")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 100)]
        public string? NombreRecibe { get; set; }

        /// <summary>
        /// Documento de identificación del responsable de la operación por parte del receptor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("docuRecibe")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(1, 25)]
        public string? DocumentoRecibe { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("observaciones")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(0, 3000)]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Placa de vehículo
        /// </summary>
        /// <remarks>
        /// Para las nota de crédito, no va PlacaVehiculo. Si da problemas se tendrá que generar una clase separada porque al momento de generar el
        /// json, en el caso del noto Extension de la nota de crédito esta propiedad no existe y por lo tanto no se genera
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("placaVehiculo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(2, 10)]
        public string? PlacaVehiculo { get; set; }

    }
}
