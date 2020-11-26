using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Configuration;

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
            txtDataBaseScriptFile.Text = fSourceDir + @"scripts\MedicoDB.sql";
            txtScriptCatalogos.Text = fSourceDir + @"scripts\Medico_datos.sql";
            txtCatalogoEnfermedad.Text = fSourceDir + @"scripts\medico_enfermedad.sql";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WriteToLog(string Msg)
        {
            mmLog.Text += Msg + Environment.NewLine;
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
                WriteToLog(string.Format(@"La cadena de conexión no contiene {0}", sName));
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
                using (SqlCommand cmd = new SqlCommand(string.Format(@"select name from master.sys.databases WHERE name = N'{0}'", sName), conn))
                {
                    object databaseName = cmd.ExecuteScalar();
                    if (!string.IsNullOrEmpty(Convert.ToString(databaseName)))
                    {
                        WriteToLog(string.Format(@"Ya existe una base de datos con el nombre {0}. No se va a crear nada", sName));
                        return false;
                    }
                    WriteToLog(string.Format(@"Creando la base de datos {0}", sName));
                    ExecuteCommand(cmd, string.Format(@"create database {0} collate SQL_Latin1_General_CP1_CI_AS", sName));
                    ExecuteCommand(cmd, string.Format(@"alter database {0} SET COMPATIBILITY_LEVEL = 140", sName)); // sql server 2017
                    StringBuilder sb = new StringBuilder(@"IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')) " + Environment.NewLine);
                    sb.Append(@"begin " + Environment.NewLine);
                    sb.Append(string.Format(@"exec [{0}].[dbo].[sp_fulltext_database] @action = 'enable' ", sName) + Environment.NewLine);
                    sb.Append(@"end " + Environment.NewLine);
                    ExecuteCommand(cmd, sb.ToString());
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET ANSI_NULL_DEFAULT OFF ", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET ANSI_NULLS OFF ", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET ANSI_PADDING OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET ANSI_WARNINGS OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET ARITHABORT OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET AUTO_CLOSE OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET AUTO_SHRINK  OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET AUTO_UPDATE_STATISTICS ON", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET CURSOR_CLOSE_ON_COMMIT OFF ", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET CURSOR_DEFAULT GLOBAL", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET CONCAT_NULL_YIELDS_NULL OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET NUMERIC_ROUNDABORT OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET QUOTED_IDENTIFIER OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET RECURSIVE_TRIGGERS OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET ENABLE_BROKER", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET DATE_CORRELATION_OPTIMIZATION OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET TRUSTWORTHY OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET ALLOW_SNAPSHOT_ISOLATION OFF ", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET PARAMETERIZATION SIMPLE", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET READ_COMMITTED_SNAPSHOT OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET HONOR_BROKER_PRIORITY OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET RECOVERY FULL", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET MULTI_USER", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET PAGE_VERIFY CHECKSUM", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET DB_CHAINING OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF)", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET TARGET_RECOVERY_TIME = 60 SECONDS", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET DELAYED_DURABILITY = DISABLED", sName));
                    ExecuteCommand(cmd, string.Format(@"sys.sp_db_vardecimal_storage_format N'{0}', N'ON'", sName));
                    ExecuteCommand(cmd, string.Format(@"ALTER DATABASE [{0}] SET QUERY_STORE = OFF", sName));
                    ExecuteCommand(cmd, string.Format(@"USE [{0}]", sName));
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
                WriteToLog(string.Format(@"No se encontró el script {0}", sFileName));
                return 0; // no existe el script para crear los objetos de la bd
            }
            string sSql = System.IO.File.ReadAllText(sFileName, System.Text.Encoding.Default);
            string[] saScript = Regex.Split(sSql, @"\bGO\r\n", RegexOptions.IgnoreCase);
            if (saScript.Length == 0)
            {
                WriteToLog(string.Format(@"El Script en {0} esta vacío, no hay nada que ejecutar", sFileName));
                return 0;
            }
            progressBar.Position = 0;
            progressBar.Properties.Maximum = saScript.Count();
            progressBar.Update();
            WriteToLog(string.Format("Cantidad de Sentencias a ejecutar: {0:N}", saScript.Count()));
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
                catch (Exception ex)
                {
                    WriteToLog(string.Format("La siguiente sentencia no se pudo ejecutar:\r\n {0}" + Environment.NewLine, cmdText) );
                    throw ex;
                }
            }
            return x;
        }

        private void sbtnEjecutar_Click(object sender, EventArgs e)
        {
            mmLog.Text = "";
            mmLog.Update();
            WriteToLog(string.Format("Hora Inicio: {0:G}", DateTime.Now));
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
                        WriteToLog(@"No se pudo abrir la conexión a la base de datos creada, debe crear los objetos y cargarlos datos de forma manual usando el Managment Studio");
                        return;
                    }
                    int x = ExecuteCommand(conn, txtDataBaseScriptFile.Text);
                    WriteToLog(string.Format(@"Se han ejecutado {0:N0} sentencias para la creación de objetos en la base de datos\r\n", x));
                    WriteToLog(@"Se procede a crear datos en los catalogos, espere un momento...");
                    x = ExecuteCommand(conn, txtScriptCatalogos.Text);
                    WriteToLog(string.Format(@"Finalizo la carga de los datos. Se han cargado {0:N0} registros en los catálogos\r\n", x));
                    if (!string.IsNullOrEmpty(txtCatalogoEnfermedad.Text))
                    {
                        WriteToLog(@"Se procede a cargar los diagnosticos o enfermedades, según la clasificación CIE10 (ICD10)");
                        x = ExecuteCommand(conn, txtCatalogoEnfermedad.Text);
                        WriteToLog(string.Format(@"Finalizó la carga del catálogo de enfermedades CIE10. Se han cargado {0:N0} registros", x));
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                mmLog.Text += ex.Message + "\r\n";
                WriteToLog("El Proceso no se completó y finalizará debido al error anterior");
            }
            WriteToLog(string.Format("Hora Fin: {0:G}", DateTime.Now));
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
