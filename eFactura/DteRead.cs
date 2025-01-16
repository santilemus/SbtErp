using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SBT.eFactura.Dte
{
    public class DteRead //<TPoco>
    {
        private string jsonDte = string.Empty;
        private string? tipoDte;
        //public TPoco? data;
        
        public DteRead()
        {

        }

        public DteRead(Stream jsonStream): base()
        {
            DteUtils.LoadDteFromStream(jsonStream);
            jsonDte = DteUtils.JsonDte;
        }

        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public string? TipoDte
        {
            get => DteUtils.TipoDte;
        }

        public string? JsonDte => jsonDte;

        public TPoco? CreateObject<TPoco>()
        {
            // TipoDte es Factura (01), Credito Fiscal (03), Factura Exportacion (11), Factura Sujeto Excluido (14)
            if (TipoDte == "01" || TipoDte == "03" || TipoDte == "11" || TipoDte == "14")
                return System.Text.Json.JsonSerializer.Deserialize<TPoco>(jsonDte, options);
            else
                return default;
        }

        /// <summary>
        /// Buscar la propiedadSelloRecibido como propiedad del root y sino como propiedad de los diferentes nodos y retorna su valor
        /// </summary>
        /// <remarks>
        /// Se implementa por separado porque no todos emisores de Dte ponen la propiedad en el mismo nodo
        /// </remarks>
        /// <returns>El sello de recibido cuando se encuentra la propiedad y cadena vacía cuando  no existe</returns>
        public string? GetSelloRecibido()
        {
            if (string.IsNullOrEmpty(JsonDte))
                return string.Empty;
            using JsonDocument jsonDoc = JsonDocument.Parse(JsonDte);
            JsonElement sello;
            if (!jsonDoc.RootElement.TryGetProperty("SelloRecibido", out sello))
            {
                var xyz = jsonDoc.RootElement.EnumerateObject().Where(x => x.Value.ValueKind == JsonValueKind.Object);
                foreach (var x in xyz)
                    if (x.Value.TryGetProperty("SelloRecibido", out sello))
                        break;
            }
            return (sello.ValueKind != JsonValueKind.Undefined && sello.ValueKind != JsonValueKind.Null) ? sello.GetString() : string.Empty;
        }

    }
}
