using System.Text.Json;
using System.Text;

namespace SBT.eFactura.Dte
{
    /// <summary>
    /// Clase que proporciona las funciones para cargar un dte desde un stream o desde un archivo e identificar el tipo previo a 
    /// serializarlo
    /// </summary>
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
