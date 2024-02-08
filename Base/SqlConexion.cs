using System.Configuration;
using System.Data.SqlClient;

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
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Erp"].ConnectionString.Replace("XpoProvider=MSSqlServer;", string.Empty);
            return new SqlConnection(cadenaConexion);
        }
    }
}
