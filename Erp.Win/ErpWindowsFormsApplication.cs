using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using System.Collections.Generic;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.SystemModule;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Erp.Win {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Win.WinApplication._members
    public partial class ErpWindowsFormsApplication : WinApplication {
        #region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
        static ErpWindowsFormsApplication() {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
			DevExpress.ExpressApp.Utils.ImageLoader.Instance.UseSvgImages = true;
        }
        private void InitializeDefaults() {
            LinkNewObjectToParentImmediately = false;
            OptimizedControllersCreation = true;
            UseLightStyle = true;
			SplashScreen = new DXSplashScreen(typeof(XafSplashScreen), new DefaultOverlayFormOptions());
			ExecuteStartupLogicBeforeClosingLogonWindow = true;
        }
        #endregion
        public ErpWindowsFormsApplication() {
            InitializeComponent();
			InitializeDefaults();

            LastLogonParametersReading += LastLogonParametersReadingEvent;
            LastLogonParametersWriting += LastLogonParametersWritingEvent;
            LoggedOn += LoggedOnEvent;
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProviders.Add(new SecuredObjectSpaceProvider((SecurityStrategyComplex)Security, XPObjectSpaceProvider.GetDataStoreProvider(args.ConnectionString, args.Connection, true), false));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
            // la siguiente linea es para permitir ejecutar en la aplicacion direct queries and stored procedure (Ej: Session.ExecuteNonQuery...)
            // si se comentaria o se borra la linea se obtiene el mensaje: Transferring requests via ICommandChannel is prohibited within the security engine
            // cuando se trata de ejecutar una sentencia SQL con update, insert, delete o un procedimiento almacenado
            ((SecuredObjectSpaceProvider)args.ObjectSpaceProviders[0]).AllowICommandChannelDoWithSecurityContext = true;
        }
        private void ErpWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
            string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if(string.Compare(userLanguageName, "en-US", StringComparison.Ordinal) != 0 && e.Languages.IndexOf(userLanguageName) == -1) {
                e.Languages.Add(userLanguageName);
            }
        }
        private void ErpWindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if(System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else {
				string message = "The application cannot connect to the specified database, " +
					"because the database doesn't exist, its version is older " +
					"than that of the application or its schema does not match " +
					"the ORM data model structure. To avoid this error, use one " +
					"of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

				if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
					message += $"\r\n\r\nInner exception: {e.CompatibilityError.Exception.Message}";
				}
				throw new InvalidOperationException(message);
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
            if (string.Compare(((CustomLogonParameters)e.LogonObject).UserName, "Admin", StringComparison.Ordinal) != 0)
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
            Model.Description = ((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial;
        }
    }
}
