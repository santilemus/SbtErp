using System;
using DevExpress.Xpo.Metadata;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Convertir propiedades Persitentes del tipo ETablaISR a varchar, antes de guardar en la base de datos y convertir de
    /// varchar a ETablaISR cuando se lee de la BD y antes de escribir en la propiedad
    /// </summary>
    public class ToETablaISR: ValueConverter
    {
        /// <summary>
        /// Convierte una propiedad persistente de tipo ETablaISR, al tipo que debe guardarse en la base de datos (en este caso varchar)
        /// </summary>
        /// <param name="value">Valor que será convertido a varchar para guardarlo en la base de datos</param>
        /// <returns>Cadena con Valor válido para la base de datos tomando en cuenta las restricciones que se aplican</returns>
        public override object ConvertToStorageType(object value)
        {
            return Enum.GetName(typeof(ETablaISR), value);
        }

        /// <summary>
        /// Convierte un Valor de campo obtenido de la base de datos (varchar) en el tipo del objeto persistente (en este caso ETablaISR)
        /// </summary>
        /// <param name="value">Valor que será convertido al tipo de dato de la propiedad del objeto persistente</param>
        /// <returns>Valor válido para la propiedad a partir del dato correspondiente en la base de datos</returns>
        public override object ConvertFromStorageType(object value)
        {
            if ((value != null) && (value.ToString() != ""))
                return Enum.Parse(typeof(ETablaISR), value.ToString());
            else
                return ETablaISR.Mensual;
        }

        /// <summary>
        /// Obtiene el tipo de datos al cual el Valor de la propiedad es convertido cuando se guarda en la base de datos
        /// </summary>  
        public override Type StorageType
        {
            get { return typeof(string); }
        }

    }
}
