using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace SBT.Apps.Medico.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = FrameworkSettingsCompatibilityMode.Latest; //DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.v20_1;
            SecurityAdapterHelper.Enable();
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            medicoWindowsFormsApplication winApplication = new medicoWindowsFormsApplication();
            // Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument2680 help article for more details on how to provide a custom splash form.
            //winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
            if (ConfigurationManager.ConnectionStrings["medico"] != null)
            {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["medico"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            try
            {
                winApplication.SplashScreen = new MedicoSplash();
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }
}
