using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text;

namespace SBT.eFactura.Dte
{
    public class DteRead<TPoco>
    {
        private string jsonDte = string.Empty;
        private string? tipoDte;
        //public TPoco? data;
        
        public DteRead()
        {

        }

        //public DteRead(string jsonDte): base()
        //{
        //    this.jsonDte = jsonDte;
        //    //GetTipoDte(jsonDte);
        //}

        public DteRead(Stream jsonStream): base()
        {
            DteUtils.LoadDteFromStream(jsonStream);
            jsonDte = DteUtils.JsonDte;
            
            //LoadDteFromStream(jsonStream);
        }

        /*
        public DteRead(TPoco data, string? jsonDte)
        {
            this.data = data;
            this.jsonDte = jsonDte;
        }
        */

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
                return DteUtils.TipoDte;


                //if (!string.IsNullOrEmpty(jsonDte))
                //    GetTipoDte(jsonDte);
                //return tipoDte;
            }
        }

        //private void GetTipoDte(string jsonDte)
        //{
        //    var dteDocument = JsonDocument.Parse(jsonDte);
        //    var ident = dteDocument.RootElement.GetProperty("identificacion");
        //    tipoDte = ident.GetProperty("tipoDte").GetString();
        //}

        public string? JsonDte => jsonDte;

        public TPoco? CreateObject()
        {
            // TipoDte es Factura (01), Credito Fiscal (03), Factura Exportacion (11), Factura Sujeto Excluido (14)
            if (TipoDte == "01" || TipoDte == "03" || TipoDte == "11" || TipoDte == "14")
                return System.Text.Json.JsonSerializer.Deserialize<TPoco>(jsonDte, options);
            else
                return default;
        }

        /*
        public void LoadDteFromStream(Stream stream)
        {
            if (stream != null && stream.Length > 0)
            {
                stream.Position = 0;
                using StreamReader rd = new StreamReader(stream, Encoding.UTF8);
                jsonDte = rd.ReadToEnd();

                //GetTipoDte(jsonDte);
            }
            else
                throw new EndOfStreamException(@"El stream del parámetro esta vacío o es nulo");
        }

        public void LoadDteFromFile(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                jsonDte = System.IO.File.ReadAllText(fileName, Encoding.UTF8);

                //GetTipoDte(jsonDte);
            }
            else
            {
                // logger aquí indicando que se genera excepcion porque no existe el archivo
                throw new System.IO.FileNotFoundException(fileName);
            }              
        }

        */
    }

    public class DteUtils
    {
        private static string jsonDte = string.Empty;

        public DteUtils() 
        {
            
        }

        public static string JsonDte => jsonDte;

        public static void LoadDteFromStream(Stream stream)
        {
            if (stream != null && stream.Length > 0)
            {
                stream.Position = 0;
                using StreamReader rd = new StreamReader(stream, Encoding.UTF8);
                jsonDte = rd.ReadToEnd();
            }
            else
                throw new EndOfStreamException(@"El stream del parámetro esta vacío o es nulo");
        }

        public static void LoadDteFromFile(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                jsonDte = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
            }
            else
            {
                // logger aquí indicando que se genera excepcion porque no existe el archivo
                throw new System.IO.FileNotFoundException(fileName);
            }
        }

        public static string TipoDte
        {
            get
            {
                var jsonDoc = System.Text.Json.JsonDocument.Parse(jsonDte);
                JsonElement elementIdentificacion;
                if (jsonDoc.RootElement.TryGetProperty("identificacion", out elementIdentificacion))
                    return elementIdentificacion.GetProperty("tipoDte").GetString() ?? string.Empty;
                else
                    return string.Empty;
            }
        }
    }
}
