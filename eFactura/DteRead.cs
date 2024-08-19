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

    }
}
