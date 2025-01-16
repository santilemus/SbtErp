using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Receptor para los siguientes tipos de Dte: fe-fc (factura)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class ReceptorFC: EmisorReceptorBase
    {
        [System.Text.Json.Serialization.JsonPropertyName("tipoDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? TipoDocumento { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("numDocumento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? NumDocumento { get; set; }
    }    
    
}
