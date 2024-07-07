using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SBT.eFactura.Dte.Poco;

namespace SBT.eFactura.Dte
{
    public class DteBase<T>
    {
        private string jsonDte;
        private string? tipoDte;
        public T data;

        public DteBase(T data, string jsonDte)
        {
            this.data = data;
            this.jsonDte = jsonDte;
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
            get
            {
                var dteDocument = JsonDocument.Parse(jsonDte);
                var ident = dteDocument.RootElement.GetProperty("identificacion");
                return ident.GetProperty("tipoDte").GetString();
            }
        }

        public T LoadFromJson(string jsonDte)
        {
            if (tipoDte == "03")
                return LoadCcfFromJson(jsonDte);
            else
                return null;
        }

        private FeCcf? LoadCcfFromJson(string jsonDte)
        {
            return System.Text.Json.JsonSerializer.Deserialize<FeCcf>(jsonDte, options);
        }

        private FeFactura? LoadFacturaFromJson(string jsonDte)
        {
            return System.Text.Json.JsonSerializer.Deserialize<FeFactura>(jsonDte, options);
        }
    }
}
