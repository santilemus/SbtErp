using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SBT.Apps.Medico.Web
{
    [RunInstaller(true)]
    public partial class InstallMedicoWeb : System.Configuration.Install.Installer
    {
        private string sourcePath;
        private string connectString;
        private string source = "SBT.Apps.Medico.Web";
        private string fCadenaConexionMasterDB;

        private string dbScriptFile;
        private string scriptCatalogosFile;
        private string scriptEnfermedadFile;

        public InstallMedicoWeb()
        {
            InitializeComponent();
        }

        private void WriteEventLog(string message, EventLogEntryType tipo)
        {
            using (EventLog eventlog = new EventLog(@"Application"))
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, @"Application");
                eventlog.Source = source;
                eventlog.WriteEntry(message, tipo);
            }
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        protected override void OnCommitted(IDictionary savedState)
        {
            base.OnCommitted(savedState);
            //System.Diagnostics.Debugger.Break();
            sourcePath = Context.Parameters["srcdir"];
            if (!System.IO.Directory.Exists($@"{sourcePath}Scripts"))
            {
                WriteEventLog($"La creación y carga de datos no se realizará, porque no se encontro la carpeta {sourcePath}Scripts o esta vacía",
                    EventLogEntryType.Warning);
                return; // no hace nada no existe la carpeta con los recursos para crear la bd y llenar los catalogos
            }
            connectString = Context.Parameters["Conexion"];
            if (string.IsNullOrEmpty(connectString))
            {
                WriteEventLog("La cadena de conexión esta vacía o no existe, no se podrá crear la base de datos ni cargar datos", EventLogEntryType.Error);
                return;
            }
            sourcePath = sourcePath.Replace(@"\\", @"\");
            fCadenaConexionMasterDB = ConnectionStringMasterDb(connectString);
            dbScriptFile = $@"{sourcePath}scripts\MedicoDB.sql";
            scriptCatalogosFile = $@"{sourcePath}scripts\Medico_datos.sql";
            scriptEnfermedadFile  = $@"{sourcePath}scripts\medico_enfermedad.sql";
        }

        private string ConnectionStringMasterDb(string sConnectionString)
        {
            string[] sa = sConnectionString.Split(new char[] { ';' });
            int c = Array.FindIndex(sa, x => x.Contains(@"Initial Catalog"));
            if (c >= 0)
                sa[c] = @"Initial Catalog=master";
            return string.Join(";", sa);
        }

        private bool CrearDatabase()
        {
            var arreglo = connectString.Split(new char[] { ';' });
            string sName = arreglo.FirstOrDefault<string>(x => x.Contains(@"Initial Catalog"));
            if (string.IsNullOrEmpty(sName))
            {
                WriteEventLog($@"La cadena de conexión no contiene {sName}", EventLogEntryType.Warning);
                return false;
            }
            arreglo = sName.Split(new char[] { '=' });
            if (arreglo.Length == 1)
            {
                WriteEventLog("La cadena de conexión es incorrecta o no contiene el nombre de la base de datos a crear", EventLogEntryType.Warning);
                return false;
            }
            sName = arreglo[1].Trim();
            using (SqlConnection conn = new SqlConnection(fCadenaConexionMasterDB))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                {
                    WriteEventLog(@"La conexión no se pudo abrir, la base de datos no se podrá crear", EventLogEntryType.Warning);
                    return false;
                }
                WriteEventLog(@"*** Crear la Base de Datos ***", EventLogEntryType.Information);
                using (SqlCommand cmd = new SqlCommand($@"select name from master.sys.databases WHERE name = N'{sName}'", conn))
                {
                    object databaseName = cmd.ExecuteScalar();
                    if (!string.IsNullOrEmpty(Convert.ToString(databaseName)))
                    {
                        WriteEventLog($@"Ya existe una base de datos con el nombre {sName}. No se va a crear nada", EventLogEntryType.Information);
                        return false;
                    }
                    ExecuteCommand(cmd, $@"create database {sName} collate SQL_Latin1_General_CP1_CI_AS");
                    ExecuteCommand(cmd, $@"alter database {sName} SET COMPATIBILITY_LEVEL = 140"); // sql server 2017
                    StringBuilder sb = new StringBuilder($@"IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')) {Environment.NewLine}");
                    sb.Append($@"begin {Environment.NewLine}");
                    sb.Append($"exec [{sName}].[dbo].[sp_fulltext_database] @action = 'enable' {Environment.NewLine}");
                    sb.Append($@"end {Environment.NewLine}");
                    ExecuteCommand(cmd, sb.ToString());
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET ANSI_NULL_DEFAULT OFF ");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET ANSI_NULLS OFF ");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET ANSI_PADDING OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET ANSI_WARNINGS OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET ARITHABORT OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET AUTO_CLOSE OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET AUTO_SHRINK  OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET AUTO_UPDATE_STATISTICS ON");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET CURSOR_CLOSE_ON_COMMIT OFF ");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET CURSOR_DEFAULT GLOBAL");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET CONCAT_NULL_YIELDS_NULL OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET NUMERIC_ROUNDABORT OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET QUOTED_IDENTIFIER OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET RECURSIVE_TRIGGERS OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET ENABLE_BROKER");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET DATE_CORRELATION_OPTIMIZATION OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET TRUSTWORTHY OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET ALLOW_SNAPSHOT_ISOLATION OFF ");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET PARAMETERIZATION SIMPLE");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET READ_COMMITTED_SNAPSHOT OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET HONOR_BROKER_PRIORITY OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET RECOVERY FULL");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET MULTI_USER");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET PAGE_VERIFY CHECKSUM");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET DB_CHAINING OFF");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF)");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET TARGET_RECOVERY_TIME = 60 SECONDS");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET DELAYED_DURABILITY = DISABLED");
                    ExecuteCommand(cmd, $@"sys.sp_db_vardecimal_storage_format N'{sName}', N'ON'");
                    ExecuteCommand(cmd, $@"ALTER DATABASE [{sName}] SET QUERY_STORE = OFF");
                    ExecuteCommand(cmd, $@"USE [{sName}]");
                    ExecuteCommand(cmd, @"ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF");
                    ExecuteCommand(cmd, @"ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0");
                    ExecuteCommand(cmd, @"ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON");
                    ExecuteCommand(cmd, @"ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF");
                    WriteEventLog(@"La base de datos se creo satisfactoriamente", EventLogEntryType.Information);
                }
            }
            return true;
        }

        private int ExecuteCommand(SqlCommand cmd, string commandText)
        {
            cmd.CommandText = commandText;
            return cmd.ExecuteNonQuery();
        }

        private int ExecuteCommand(SqlConnection conn, string sFileName)
        {
            if (!System.IO.File.Exists(sFileName))
            {
                WriteEventLog($@"No se encontró el script {sFileName}", EventLogEntryType.Warning);
                return 0; // no existe el script para crear los objetos de la bd
            }
            string sSql = System.IO.File.ReadAllText(sFileName, System.Text.Encoding.Default);
            string[] saScript = Regex.Split(sSql, @"\bGO\r\n", RegexOptions.IgnoreCase);
            if (saScript.Length == 0)
            {
                WriteEventLog($@"El Script en {sFileName} esta vacío, no hay nada que ejecutar", EventLogEntryType.Warning);
                return 0;
            }
            WriteEventLog($"Cantidad de Sentencias a ejecutar: {saScript.Count():N}", EventLogEntryType.Information);
            int x = 0;
            foreach (string cmdText in saScript)
            {
                try
                {
                    if (!string.IsNullOrEmpty(cmdText))
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                            x += cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    WriteEventLog(string.Format($"La siguiente sentencia no se pudo ejecutar:\r\n {{0}}{Environment.NewLine}", cmdText), EventLogEntryType.Error);
                    throw ex;
                }
            }
            return x;
        }

        private void CreateDatabaseAndData()
        {
            WriteEventLog($"Hora Inicio: {DateTime.Now:G}", EventLogEntryType.Information);
            try
            {
                fCadenaConexionMasterDB = ConnectionStringMasterDb(connectString);
                if (!CrearDatabase())
                {
                    WriteEventLog(@"El proceso finalizará sin crear la base de datos", EventLogEntryType.Warning);
                    return;
                }
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    if (conn.State != ConnectionState.Open)
                    {
                        WriteEventLog(@"No se pudo abrir la conexión a la base de datos, debe crear los objetos y cargar los datos de forma manual usando el Managment Studio",
                            EventLogEntryType.Error);
                        return;
                    }
                    int x = ExecuteCommand(conn, dbScriptFile);
                    WriteEventLog($@"Se han ejecutado {x:N0} sentencias para la creación de objetos en la base de datos\r\n", EventLogEntryType.Information);
                    WriteEventLog(@"Se procede a crear datos en los catalogos, espere un momento...", EventLogEntryType.Information);
                    x = ExecuteCommand(conn, scriptCatalogosFile);
                    WriteEventLog($@"Finalizo la carga de los datos. Se han cargado {x:N0} registros en los catálogos\r\n", EventLogEntryType.Information);
                    if (!string.IsNullOrEmpty(scriptEnfermedadFile))
                    {
                        WriteEventLog(@"Se procede a cargar los diagnosticos o enfermedades, según la clasificación CIE10 (ICD10)", EventLogEntryType.Information);
                        x = ExecuteCommand(conn, scriptEnfermedadFile);
                        WriteEventLog($@"Finalizó la carga del catálogo de enfermedades CIE10. Se han cargado {x:N0} registros", EventLogEntryType.Information);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                WriteEventLog($@"El Proceso no se completó y finalizará debido al error {ex.Message}", EventLogEntryType.Error);
            }
            WriteEventLog($"Hora Fin: {DateTime.Now:G}", EventLogEntryType.Information);

        }
    }
}
