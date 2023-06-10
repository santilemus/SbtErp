using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace SBT.Apps.Base.Module
{
    /// <summary>
    /// Clase para crear instancias de conexion a la base de datos, cuando no se puede utilizar ObjectSpace,
    /// por ejemplo para un SqlDataReader
    /// </summary>
    public class SqlConexion
    {
        public SqlConexion()
        {

        }

        public SqlConnection CreateConnection()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Erp"].ConnectionString.Replace("XpoProvider=MSSqlServer;", "");
            return new SqlConnection(cadenaConexion);
        }
    }
}
