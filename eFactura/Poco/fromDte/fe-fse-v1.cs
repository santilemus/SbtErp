//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.3.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------


namespace MyNamespace
{
    #pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class Identificacion
    {

        [System.Text.Json.Serialization.JsonPropertyName("version")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int Version { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("ambiente")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Ambiente { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("tipoDte")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string TipoDte { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("numeroControl")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string NumeroControl { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codigoGeneracion")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string CodigoGeneracion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("tipoModelo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int TipoModelo { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("tipoOperacion")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int TipoOperacion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("tipoContingencia")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int TipoContingencia { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("motivoContin")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object MotivoContin { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("fecEmi")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset FecEmi { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("horEmi")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string HorEmi { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("tipoMoneda")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string TipoMoneda { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class Emisor
    {

        [System.Text.Json.Serialization.JsonPropertyName("nit")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Nit { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("nrc")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Nrc { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("nombre")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Nombre { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codActividad")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string CodActividad { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("descActividad")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string DescActividad { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("direccion")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Direccion Direccion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("telefono")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Telefono { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codEstableMH")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object CodEstableMH { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codEstable")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object CodEstable { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codPuntoVentaMH")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string CodPuntoVentaMH { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codPuntoVenta")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object CodPuntoVenta { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("correo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Correo { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class Direccion
    {

        [System.Text.Json.Serialization.JsonPropertyName("departamento")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Departamento { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("municipio")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Municipio { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("complemento")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Complemento { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class SujetoExcluido
    {

        [System.Text.Json.Serialization.JsonPropertyName("tipoDocumento")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string TipoDocumento { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("numDocumento")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string NumDocumento { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("nombre")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Nombre { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codActividad")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string CodActividad { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("descActividad")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string DescActividad { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("direccion")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Direccion Direccion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("telefono")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Telefono { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("correo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object Correo { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class CuerpoDocumento
    {

        [System.Text.Json.Serialization.JsonPropertyName("numItem")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int NumItem { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("tipoItem")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int TipoItem { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("cantidad")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Cantidad { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codigo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Codigo { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("uniMedida")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int UniMedida { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("descripcion")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Descripcion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("precioUni")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string PrecioUni { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("montoDescu")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string MontoDescu { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("compra")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Compra { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class Resumen
    {

        [System.Text.Json.Serialization.JsonPropertyName("totalCompra")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double TotalCompra { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("descu")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double Descu { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("totalDescu")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double TotalDescu { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("subTotal")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double SubTotal { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("ivaRete1")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double IvaRete1 { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("reteRenta")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double ReteRenta { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("totalPagar")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double TotalPagar { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("totalLetras")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string TotalLetras { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("condicionOperacion")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int CondicionOperacion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("pagos")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<Pago> Pagos { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("observaciones")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object Observaciones { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class Pago
    {

        [System.Text.Json.Serialization.JsonPropertyName("codigo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Codigo { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("montoPago")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public double MontoPago { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("referencia")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Referencia { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("plazo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Plazo { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("periodo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object Periodo { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public partial class FeFseV1
    {

        [System.Text.Json.Serialization.JsonPropertyName("identificacion")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Identificacion Identificacion { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("emisor")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Emisor Emisor { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("sujetoExcluido")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public SujetoExcluido SujetoExcluido { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("cuerpoDocumento")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<CuerpoDocumento> CuerpoDocumento { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("resumen")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Resumen Resumen { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("apendice")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object Apendice { get; set; }



        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    internal class DateFormatConverter : System.Text.Json.Serialization.JsonConverter<System.DateTimeOffset>
    {
        public override System.DateTimeOffset Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var dateTime = reader.GetString();
            if (dateTime == null)
            {
                throw new System.Text.Json.JsonException("Unexpected JsonTokenType.Null");
            }

            return System.DateTimeOffset.Parse(dateTime);
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, System.DateTimeOffset value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}