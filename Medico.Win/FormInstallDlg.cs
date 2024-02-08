using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SBT.Apps.Medico.Win
{
    public partial class FormInstallDlg : Form
    {
        private string fSourceDir;
        private string fExeName;
        private string fCadenaConexionMasterDB;

        public FormInstallDlg()
        {
            InitializeComponent();
        }

        public FormInstallDlg(string sourceDir, string exeName) : base()
        {
            InitializeComponent();
            fSourceDir = sourceDir.Replace(@"\\", @"\");
            fExeName = exeName;
            Configuration config = ConfigurationManager.OpenExeConfiguration(fExeName);
            txtCadenaConexion.Text = config.ConnectionStrings.ConnectionStrings["Medico"].ConnectionString;
            fCadenaConexionMasterDB = ConnectionStringMasterDb(txtCadenaConexion.Text);
            txtDataBaseScriptFile.Text = $@"{fSourceDir}scripts\MedicoDB.sql";
            txtScriptCatalogos.Text = $@"{fSourceDir}scripts\Medico_datos.sql";
            txtCatalogoEnfermedad.Text = $@"{fSourceDir}scripts\medico_enfermedad.sql";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WriteToLog(string Msg)
        {
            mmLog.Text += $"{Msg}{Environment.NewLine}";
            mmLog.Update();
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
            var arreglo = txtCadenaConexion.Text.Split(new char[] { ';' });
            string sName = arreglo.FirstOrDefault<string>(x => x.Contains(@"Initial Catalog"));
            if (string.IsNullOrEmpty(sName))
            {
                WriteToLog($@"La cadena de conexión no contiene {sName}");
                return false;
            }
            arreglo = sName.Split(new char[] { '=' });
            if (arreglo.Length == 1)
            {
                WriteToLog("La cadena de conexión es incorrecta o no contiene el nombre de la base de datos a crear");
                return false;
            }
            sName = arreglo[1].Trim();
            using (SqlConnection conn = new SqlConnection(fCadenaConexionMasterDB))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                {
                    WriteToLog(@"La conexión no se pudo abrir, la base de datos no se podrá crear");
                    return false;
                }
                WriteToLog(@"*** Crear la Base de Datos ***");
                using (SqlCommand cmd = new SqlCommand($@"select name from master.sys.databases WHERE name = N'{sName}'", conn))
                {
                    object databaseName = cmd.ExecuteScalar();
                    if (!string.IsNullOrEmpty(Convert.ToString(databaseName)))
                    {
                        WriteToLog($@"Ya existe una base de datos con el nombre {sName}. No se va a crear nada");
                        return false;
                    }
                    WriteToLog($@"Creando la base de datos {sName}");
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
                    WriteToLog(@"La base de datos se creo satisfactoriamente");
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
                WriteToLog($@"No se encontró el script {sFileName}");
                return 0; // no existe el script para crear los objetos de la bd
            }
            string sSql = System.IO.File.ReadAllText(sFileName, System.Text.Encoding.Default);
            string[] saScript = Regex.Split(sSql, @"\bGO\r\n", RegexOptions.IgnoreCase);
            if (saScript.Length == 0)
            {
                WriteToLog($@"El Script en {sFileName} esta vacío, no hay nada que ejecutar");
                return 0;
            }
            progressBar.Position = 0;
            progressBar.Properties.Maximum = saScript.Count();
            progressBar.Update();
            WriteToLog($"Cantidad de Sentencias a ejecutar: {saScript.Count():N}");
            int x = 0;
            foreach (string cmdText in saScript)
            {
                try
                {
                    if (!string.IsNullOrEmpty(cmdText))
                        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                            x += cmd.ExecuteNonQuery();
                    progressBar.Position++;
                    progressBar.Update();
                }
                catch
                {
                    WriteToLog(string.Format($"La siguiente sentencia no se pudo ejecutar:\r\n {{0}}{Environment.NewLine}", cmdText));
                    throw;
                }
            }
            return x;
        }

        private void sbtnEjecutar_Click(object sender, EventArgs e)
        {
            mmLog.Text = "";
            mmLog.Update();
            WriteToLog($"Hora Inicio: {DateTime.Now:G}");
            mmLog.Update();
            try
            {
                fCadenaConexionMasterDB = ConnectionStringMasterDb(txtCadenaConexion.Text);
                if (!CrearDatabase())
                {
                    WriteToLog(@"El proceso finalizará sin crear la base de datos");
                    return;
                }
                using (SqlConnection conn = new SqlConnection(txtCadenaConexion.Text))
                {
                    conn.Open();
                    if (conn.State != ConnectionState.Open)
                    {
                        WriteToLog(@"No se pudo abrir la conexión a la base de datos creada, debe crear los objetos y cargar los datos de forma manual usando el Managment Studio");
                        return;
                    }
                    int x = ExecuteCommand(conn, txtDataBaseScriptFile.Text);
                    WriteToLog($@"Se han ejecutado {x:N0} sentencias para la creación de objetos en la base de datos\r\n");
                    WriteToLog(@"Se procede a crear datos en los catalogos, espere un momento...");
                    x = ExecuteCommand(conn, txtScriptCatalogos.Text);
                    WriteToLog($@"Finalizo la carga de los datos. Se han cargado {x:N0} registros en los catálogos\r\n");
                    if (!string.IsNullOrEmpty(txtCatalogoEnfermedad.Text))
                    {
                        WriteToLog(@"Se procede a cargar los diagnosticos o enfermedades, según la clasificación CIE10 (ICD10)");
                        x = ExecuteCommand(conn, txtCatalogoEnfermedad.Text);
                        WriteToLog($@"Finalizó la carga del catálogo de enfermedades CIE10. Se han cargado {x:N0} registros");
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                mmLog.Text += $"{ex.Message}\r\n";
                WriteToLog("El Proceso no se completó y finalizará debido al error anterior");
            }
            WriteToLog($"Hora Fin: {DateTime.Now:G}");
        }

        private void txtCadenaConexion_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                fCadenaConexionMasterDB = ConnectionStringMasterDb(txtCadenaConexion.Text);
                using (SqlConnection conn = new SqlConnection(fCadenaConexionMasterDB))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        WriteToLog("Conexión Satisfactoria");
                        MessageBox.Show("Conexión Satisfactoria");
                        conn.Close();
                    }
                    else
                        WriteToLog(@"La conexión no se pudo abrir. Revisarla");
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex.Message);
            }
        }

        private void txtDataBaseScriptFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "archivos sql (*.sql)|*.SQL";
                dlg.DefaultExt = "sql";
                dlg.Title = "Script para Crear Base de Datos";
                txtDataBaseScriptFile.EditValue = (dlg.ShowDialog() == DialogResult.OK) ? dlg.FileName : "";
            }
        }

        private void txtScriptDatos_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "archivos sql (*.sql)|*.SQL";
                dlg.DefaultExt = "sql";
                dlg.Title = "Script con Catálogos";
                txtScriptCatalogos.EditValue = (dlg.ShowDialog() == DialogResult.OK) ? dlg.FileName : "";
            }
        }


        private void txtCadenaConexion_EditValueChanged(object sender, EventArgs e)
        {
            fCadenaConexionMasterDB = ConnectionStringMasterDb(txtCadenaConexion.Text);
        }
    }
}
