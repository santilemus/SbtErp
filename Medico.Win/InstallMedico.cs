using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Configuration;

namespace SBT.Apps.Medico.Win
{
    [RunInstaller(true)]
    public partial class InstallMedico : System.Configuration.Install.Installer
    {
        private string sourcePath;
        public InstallMedico()
        {
            InitializeComponent();
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
            string sExe = Context.Parameters["assemblypath"];
            //System.Diagnostics.Debugger.Break();
            sourcePath = Context.Parameters["srcdir"];
            if (!System.IO.Directory.Exists(sourcePath + @"Scripts"))
            {
                MessageBox.Show(string.Format("La creación y carga de datos no se realizará, porque no se encontro la carpeta {0} o esta vacía", sourcePath + @"Scripts"), 
                    "Registro Electrónico Medico");
                return; // no hace nada no existe la carpeta con los recursos para crear la bd y llenar los catalogos
            }
            FormInstallDlg frmInstallDlg = new FormInstallDlg(sourcePath, sExe);
            frmInstallDlg.ShowDialog();
            frmInstallDlg.Dispose();

        }

    }
}
