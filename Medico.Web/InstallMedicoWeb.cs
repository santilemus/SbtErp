using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SBT.Apps.Medico.Web
{
    [RunInstaller(true)]
    public partial class InstallMedicoWeb : System.Configuration.Install.Installer
    {
        public InstallMedicoWeb()
        {
            InitializeComponent();
        }
    }
}
