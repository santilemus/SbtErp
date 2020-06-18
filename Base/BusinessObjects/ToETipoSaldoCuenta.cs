using System;
using System.Collections.Generic;
using DevExpress.Xpo.Metadata;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    /// <summary>
    /// Clase que implementa la conversión de tipo de una propiedad del objeto persistente al tipo de dato
    /// en la base de datos, en este caso es un varchar(1)
    /// </summary>
    public class ToETipoSaldoCuenta: ValueConverter
    {
        /// <summary>
        /// Convierte una propiedad persistente de tipo entero, al tipo que debe guardarse en la base de datos (en este caso varchar(1))
        /// </summary>
        /// <param name="value">Valor que será convertido a varchar para guardarlo en la base de datos</param>
        /// <returns>Retorna cadena varchar(1) en la base de datos, con los posibles valores para la columna en la base de datos</returns>
        public override object ConvertToStorageType(object value)
        {
            return Convert.ToString(value)[0];
        }

        /// <summary>
        /// Convierte un valor de campo obtenido de la base de datos (varchar) en el tipo del objeto persistente (en este caso ETipoSaldoCuenta)
        /// </summary>
        /// <param name="value">Valor que será convertido al tipo de dato de la propiedad del objeto persistente</param>
        /// <returns>Valor válido para la propiedad a partir del dato correspondiente en la base de datos</returns>
        public override object ConvertFromStorageType(object value)
        {
            return (Convert.ToString(value) == "D") ? ETipoSaldoCuenta.Deudor : ETipoSaldoCuenta.Acreedor;
        }

        /// <summary>
        /// Obtiene el tipo de datos al cual el valor de la propiedad es convertido cuando se guarda en la base de datos
        /// </summary>
        public override Type StorageType
        {
            get { return typeof(string); }
        }
    }
}
