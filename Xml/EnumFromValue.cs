using System;
using System.Xml.Linq;

namespace SBT.Apps.Xml
{
    static class PgaExtensions
    {
        public static T EnumFromValue<T>(this XAttribute attribute)
        {
            string value = attribute.Value;
            if (string.IsNullOrEmpty(value))
                return default(T);

            try
            {
                T converted = (T)Enum.Parse(typeof(T), value, true);
                return converted;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

    }
}
