using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Buffers.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [Serializable, DomainComponent, ModelDefault("Caption", "Test Consumir Servicio Rest Mayan")]
    public class MayanDocumentsReadResponse
    {
        public MayanDocumentsReadResponse()
        {
            tipoDocumento = new DocumentType();
            ultimaversion = new LatestVersion();
        }

        DocumentType tipoDocumento;
        LatestVersion ultimaversion;

        [JsonPropertyName("date_added")]
        public string FechaAgrego { get; set; }
        [JsonPropertyName("description")]
        public string Descripcion { get; set; }
        [JsonPropertyName("document_type")]
        public DocumentType TipoDocumento
        {
            get => tipoDocumento ;
            set { tipoDocumento = value; }
        }
        [JsonPropertyName("document_type_change_url")]
        public string UrlCambioTipoDocumento { get; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("label")]
        public string Etiqueta { get; set; }
        [JsonPropertyName("language")]
        public string Idioma { get; set; }
        [JsonPropertyName("latest_version")]
        public LatestVersion UltimaVersion
        {
            get => ultimaversion;
            set { ultimaversion = value; }
        }

        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
        public int pk { get; set; }
        [JsonPropertyName("versions_url")]
        public string VersionesUrl { get; set; }

    }

    [Serializable]
    public class DocumentType
    {
        public DocumentType()
        {

        }

        [JsonPropertyName("delete_time_period")]
        public int PeriodoTiempoBorrado { get; set; }
        [JsonPropertyName("delete_time_unit")]
        public string UnidadTiempoBorrado { get; set; }
        [JsonPropertyName("documents_url")]
        public string UrlDocumentos { get; set; }
        [JsonPropertyName("documents_count")]
        public int CantidadDocumentos { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("label")]
        public string Etiqueta { get; set; }
        //[JsonIgnore]
        public string[] filenames { get; set; }
        [JsonPropertyName("trash_time_period")]
        public int ? PeriodoParaEnvioPapelera { get; set; }
        [JsonPropertyName("trash_time_unit")]
        public string UnidadTiempoEnvioPapelera { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    [Serializable]
    public class LatestVersion
    {
        public LatestVersion()
        {

        }

        [JsonPropertyName("checksum")]
        public string HashComprobacion { get; set; }
        [JsonPropertyName("comment")]
        public string Comentario { get; set; }
        [JsonPropertyName("document_url")]
        public string UrlDocumento { get; set; }
        [JsonPropertyName("download_url")]
        public string DescargaUrl { get; set; }
        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }
        [JsonPropertyName("file")]
        public string Archivo { get; set;}
        [JsonPropertyName("mimetype")]
        public string TipoMedio { get; set; }
        [JsonPropertyName("pages_url")]
        public string UrlPaginas { get; set; }
        [JsonPropertyName("size")]
        public int Tamanio { get; set; }
        [JsonPropertyName("timestamp")]
        public string timestamp { get; set;  }
        [JsonPropertyName("url")]
        public string Url { get; set;  }

    }

    public class ToStringJsonConverter: JsonConverter<string>
    {
        public ToStringJsonConverter()
        {

        }

        public override string Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Number &&
                type == typeof(String))
                return reader.GetString();

            ReadOnlySpan<byte> span = null; // reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan; // FALLA version no incluye ToArray()

            if (Utf8Parser.TryParse(span, out long number, out var bytesConsumed) && span.Length == bytesConsumed)
                return number.ToString();

            var data = reader.GetString();

            throw new InvalidOperationException($"'{data}' is not a correct expected value!")
            {
                Source = "LongToStringJsonConverter"
            };
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
