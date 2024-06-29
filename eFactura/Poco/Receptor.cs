using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Receptor para los siguientes tipos de Dte: fe-fc (factura)
    /// Para los fe-ccf (crédito fiscal) la estructura del receptor es similar al emisor (tienen las mismas propiedades) y 
    /// por lo tanto solo se define las class que corresponde al emisor
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class Receptor
    {

        [System.Text.Json.Serialization.JsonPropertyName("tipoDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? TipoDocumento { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("numDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public object NumDocumento { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("nrc")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Nrc { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Nombre { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("codActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? CodActividad { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("descActividad")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? DescActividad { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("direccion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public Direccion Direccion { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("telefono")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public object Telefono { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("correo")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public object Correo { get; set; }

    }    
    
}
