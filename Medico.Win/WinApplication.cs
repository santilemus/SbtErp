using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using System.Collections.Generic;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.SystemModule;
using SBT.Apps.Base.Module.BusinessObjects;
using DevExpress.ExpressApp.Security;

namespace SBT.Apps.Medico.Win {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/DevExpressExpressAppWinWinApplicationMembersTopicAll
    public partial class medicoWindowsFormsApplication : WinApplication {
        public medicoWindowsFormsApplication() {
            InitializeComponent();
            //SetLanguage("es");
            //SetFormattingCulture("es");
            //CustomizeFormattingCulture += Instance_CustomizeFormattingCulture; 
            LastLogonParametersReading += LastLogonParametersReadingEvent;
            LastLogonParametersWriting += LastLogonParametersWritingEvent;
            
            LoggedOn += LoggedOnEvent;
            // agregado para permitir la seleccion de empresas y sucursales en el login
            
            ((SecurityStrategy)Security).AnonymousAllowedTypes.Add(typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa));
            ((SecurityStrategy)Security).AnonymousAllowedTypes.Add(typeof(SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad));
            
            //LoggedOff += LoggedOffEvent;
        }

        //static void Instance_CustomizeFormattingCulture(object sender, CustomizeFormattingCultureEventArgs e)
        //{
        //    e.FormattingCulture.NumberFormat.CurrencySymbol = "$";
        //    e.FormattingCulture.NumberFormat.CurrencyPositivePattern = 0;
        //    e.FormattingCulture.NumberFormat.CurrencyNegativePattern = 1;
        //    e.FormattingCulture.NumberFormat.CurrencyDecimalDigits = 2;
        //    e.FormattingCulture.NumberFormat.CurrencyDecimalSeparator = ".";
        //    e.FormattingCulture.NumberFormat.CurrencyGroupSeparator = ",";
        //    e.FormattingCulture.NumberFormat.PercentDecimalSeparator = ".";
        //    e.FormattingCulture.NumberFormat.PercentDecimalDigits = 2;
        //    e.FormattingCulture.NumberFormat.NumberDecimalSeparator = ".";
        //    e.FormattingCulture.NumberFormat.NumberGroupSeparator = ",";
        //    e.FormattingCulture.NumberFormat.NumberDecimalDigits = 2;
        //}

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection, false);
        }
        private void medicoWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
                e.Languages.Add(userLanguageName);
            }
        }
        private void medicoWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if(System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else {
                throw new InvalidOperationException(
                    "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the 'Update Application and Database Versions' help topic at http://help.devexpress.com/#Xaf/CustomDocument2795 " +
                    "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
            }
#endif
        }

        /// <summary>
        /// Guardar los parámetros de la ventana de logon, justo despues que el login ha sido satisfactorio
        /// (Se guardan en una cookie). Agregar más adelante, los párametros de empresa, sucursal, bodega
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del logon que se desean guardar</param>
        private void LastLogonParametersWritingEvent(object sender, LastLogonParametersWritingEventArgs e)
        {
            if (((CustomLogonParameters)e.LogonObject).UserName != "Admin")
            {
                e.SettingsStorage.SaveOption(string.Empty, "UserName", ((CustomLogonParameters)e.LogonObject).UserName);
            }

            System.Globalization.CultureInfo cci = System.Globalization.CultureInfo.CurrentCulture;
            e.SettingsStorage.SaveOption(string.Empty, "Empresa", ((CustomLogonParameters)e.LogonObject).OidEmpresa.ToString(cci.NumberFormat));
            e.SettingsStorage.SaveOption(string.Empty, "Agencia", ((CustomLogonParameters)e.LogonObject).Agencia.Oid.ToString(cci.NumberFormat));
            e.Handled = true;
        }

        /// <summary>
        /// Leer los parametros de la sesion anterior y sugerirlos en la ventana de log on, justo antes de que se muestre
        /// (se guardan en una cookie).
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Parametros del logon que podriamos inicializar con los parametros de la sesion anterior</param>
        private void LastLogonParametersReadingEvent(object sender, LastLogonParametersReadingEventArgs e)
        {
            System.Globalization.CultureInfo cci = System.Globalization.CultureInfo.CurrentCulture;
            if (string.IsNullOrEmpty(e.SettingsStorage.LoadOption(string.Empty, "UserName")))
            {
                e.SettingsStorage.SaveOption(string.Empty, "UserName", "Chamba");
            }
            if (string.IsNullOrEmpty(e.SettingsStorage.LoadOption(string.Empty, "Empresa")))
            {
                e.SettingsStorage.SaveOption(string.Empty, "Empresa", "1");
            }
            else
            {
                ((CustomLogonParameters)e.LogonObject).OidEmpresa = Convert.ToInt16(e.SettingsStorage.LoadOption(string.Empty, "Empresa"), cci.NumberFormat);
            }
            if (!string.IsNullOrEmpty(e.SettingsStorage.LoadOption(string.Empty, "Agencia")))
                ((CustomLogonParameters)e.LogonObject).OidSucursal = Convert.ToInt16(e.SettingsStorage.LoadOption(string.Empty, "Agencia"), cci.NumberFormat);
            else
                e.SettingsStorage.SaveOption(string.Empty, "Agencia", "1");
            e.Handled = true;
        }

        private void LoggedOnEvent(object sender, LogonEventArgs e)
        {
            /// utilizar el username como sesion id en el helper class, puede ser un problema si un mismo usuario abre dos sessiones
            /// Evaluar utilizar el session id de la aplicacion y combinarlo con el UserName, en un unico ID o distintos, para evitar
            /// ese problema. Sin embargo, evaluar como se va acceder a los parámetros correctos desde los modules con BO, porque allí
            /// no se conoce el ID de la sesion de la aplicación
            /// Session.SessionID
            string sIdSesion = ((CustomLogonParameters)e.LogonParameters).UserName;
            SesionDataHelper.Inicializar(sIdSesion);
            SesionDataHelper.Agregar("OidEmpresa", ((CustomLogonParameters)e.LogonParameters).OidEmpresa);
            SesionDataHelper.Agregar("OidSucursal", ((CustomLogonParameters)e.LogonParameters).OidSucursal);
            // agregar aqui otros variables globales para la sesion, por ejemplo: sucursal, bodega, caja, fecha de trabajo etc

            // las siguientes dos lienas son por si se necesitara hacer algo con la informacion del equipo remoto (caso de POS, para identificar las cajas)
            //Sanrey.Erp.Base.BusinessObjects.SesionDataHelper.Agregar("UserHostName", Request.UserHostName);
            //Sanrey.Erp.Base.BusinessObjects.SesionDataHelper.Agregar("UserHostAddress", Request.UserHostAddress);
            var INavItems = ((IModelApplicationNavigationItems)this.Model).NavigationItems;
            INavItems.StartupNavigationItem = null;
            //((IModelRootNavigationItems)INavItems).ShowNavigationOnStart = true;

            Model.AboutInfoString = "{0:ProductName} - {0:Description}<br>{0:Version}<br>{0:Copyright}";
            if (!Model.Title.Contains(((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial))
                Model.Title  =  Model.Title + " - " + ((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial;
            Model.Description = ((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial;
        }
    }
}
