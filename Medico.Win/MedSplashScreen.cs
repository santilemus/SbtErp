using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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