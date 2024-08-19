using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using SBT.eFactura.Dte.Poco;
using System.Diagnostics.CodeAnalysis;

namespace SBT.eFactura.Dte.Serialization
{
    /// <summary>
    /// Convertir un Poco Object que representa un dte al correspondiente json
    /// </summary>
    /// <typeparam name="TPoco"></typeparam>
    public class JSonConverterObject<TPoco>: JsonConverter<TPoco>
    {
        public override TPoco? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TPoco value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

    }
}
