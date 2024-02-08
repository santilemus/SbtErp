using System.Windows.Forms;
//using DevExpress.XtraSplashScreen;

namespace SBT.Apps.Medico.Win
{
    public partial class MedSplashScreen : Form
    {
        public MedSplashScreen()
        {
            InitializeComponent();
        }


        public enum SplashScreenCommand
        {
        }

        internal void UpdateInfo(string info)
        {
            lblCargando.Text = info;
        }
    }
}