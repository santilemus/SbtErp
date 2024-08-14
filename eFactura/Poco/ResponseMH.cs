using System;


namespace SBT.eFactura.Dte.Poco
{
    public class ResponseMH
    {
        public ResponseMH()
        {
            Estado = string.Empty;
            ClasificacionMensaje = string.Empty;
            CodigoGeneracion = string.Empty;
            CodigoMensaje = string.Empty;
            DescripcionMensaje = string.Empty;
            NumeroControl = string.Empty;
        }

        [System.Text.Json.Serialization.JsonPropertyName("version")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int? Version { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("ambiente")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? Ambiente { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("versionApp")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int? VersionApp { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("estado")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string Estado { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("codigoGeneracion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string CodigoGeneracion { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("numeroControl")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string NumeroControl { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("selloRecibido")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? SelloRecibido { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("fhProcesamiento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? FechaHoraProcesamiento { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("clasificaMsg")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string ClasificacionMensaje { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("codigoMsg")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string CodigoMensaje { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("descripcionMsg")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string DescripcionMensaje { get; set; }


        [System.Text.Json.Serialization.JsonPropertyName("observaciones")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<string>? Observaciones { get; set; }

    }
}
