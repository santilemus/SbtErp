using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;

namespace SBT.Apps.Erp.Module.Actions
{
    /// <summary>
    /// Crear el MemoryStream con los datos en formato Csv
    /// </summary>
    public class ExportAction
    {
        public ExportAction()
        {

        }

        /// <summary>
        /// Crear un Memory stream de los datos recibidos en el parametro
        /// </summary>
        /// <typeparam name="T">El Tipo de datos de IList</typeparam>
        /// <param name="datos">IList con los datos a exportar a Csv</param>
        /// <returns>MemoryStream con los datos exportados a formato Csv</returns>
        public MemoryStream CreateCsvStream<T>(IList<T> datos)
        {
            using MemoryStream ms = new MemoryStream();
            UTF8Encoding utf8 = new UTF8Encoding();
            using StreamWriter writer = new StreamWriter(ms, utf8);
            using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords<T>(datos);
            }
            return ms;
            //return ms.ToArray();

            /*
            using var objectSpace = objectSpaceFactory.CreateObjectSpace<Listas>();
            IQueryable<Listas> qData = objectSpace.GetObjectsQuery<Listas>();
            var resultado = qData.Where(p => p.Categoria == CategoriaLista.CompraVenta).Select(o => new { o.Codigo, o.Nombre }).ToList();
            var a = tipo.GetType();
            */
        }
    }
}
