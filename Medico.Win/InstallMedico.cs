using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

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
            if (!System.IO.Directory.Exists($@"{sourcePath}Scripts"))
            {
                MessageBox.Show($"La creación y carga de datos no se realizará, porque no se encontro la carpeta {sourcePath}Scripts o esta vacía", 
                    "Registro Electrónico Medico");
                return; // no hace nada no existe la carpeta con los recursos para crear la bd y llenar los catalogos
            }
            FormInstallDlg frmInstallDlg = new FormInstallDlg(sourcePath, sExe);
            frmInstallDlg.ShowDialog();
            frmInstallDlg.Dispose();

        }

    }
}
