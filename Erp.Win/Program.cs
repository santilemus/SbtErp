﻿using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Runtime.Versioning;

namespace SBT.Apps.Erp.Win {

    [SupportedOSPlatform("windows")]
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.v20_1
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.Latest;
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            WindowsFormsSettings.LoadApplicationSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            if(Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder) {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            ErpWindowsFormsApplication winApplication = new ErpWindowsFormsApplication();
            // Refer to the https://docs.devexpress.com/eXpressAppFramework/112680 help article for more details on how to provide a custom splash form.
            //winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen("YourSplashImage.png");
			SecurityStrategy security = (SecurityStrategy)winApplication.Security;
            security.RegisterXPOAdapterProviders();
            security.PermissionsReloadMode = PermissionsReloadMode.CacheOnFirstAccess;  // agregado el 27/07/2022 por selm para reducir los accesos a la bd por permisos
            if(ConfigurationManager.ConnectionStrings["Erp"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["Erp"].ConnectionString;
            }
            // agregado para permitir la seleccion de empresas y sucursales en el login
            ((SecurityStrategy)security).AnonymousAllowedTypes.Add(typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa));
            ((SecurityStrategy)security).AnonymousAllowedTypes.Add(typeof(SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad));
            // agregados el 11/03/2021 para optimizar la carga de la app. Deshabilitarlos cuando se hagan pruebas unitarias
            DevExpress.ExpressApp.Model.ModelXmlReader.UseXmlReader = true;
            DevExpress.ExpressApp.Model.Core.ModelNode.UseDefaultValuesCache = true;
            DevExpress.ExpressApp.Core.ControllersManager.UseParallelBatchControllerCreation = true;
            DevExpress.ExpressApp.ApplicationModulesManager.UseParallelTypesCollector = true;
            DevExpress.ExpressApp.ApplicationModulesManager.UseStaticCache = false;
            DevExpress.Persistent.Base.ReflectionHelper.UseAssemblyResolutionCache = false;
            winApplication.UseLightStyle = true;
            winApplication.UseOldTemplates = false;
            //---
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            try {
                winApplication.Setup();
                winApplication.Start();
            }
            catch(Exception e) {
                winApplication.HandleException(e);
            }
        }
    }
}
