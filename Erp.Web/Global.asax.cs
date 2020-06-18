﻿using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Web.Routing;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.SystemModule;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Erp.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            RouteTable.Routes.RegisterXafRoutes();
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new ErpAspNetApplication());
            SecurityStrategy security = (SecurityStrategy)WebApplication.Instance.Security;
            security.RegisterXPOAdapterProviders();
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.SwitchToNewStyle();
            if(ConfigurationManager.ConnectionStrings["Erp"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["Erp"].ConnectionString;

            }
            WebApplication.Instance.LastLogonParametersWriting += new EventHandler<LastLogonParametersWritingEventArgs>(Instance_LastLogonParametersWriting);
            WebApplication.Instance.LastLogonParametersReading += new EventHandler<LastLogonParametersReadingEventArgs>(Instance_LastLogonParametersReading);
            WebApplication.Instance.LoggedOn += new EventHandler<LogonEventArgs>(Instance_LoggedOn);
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif

            DevExpress.ExpressApp.Web.Templates.ActionContainers.NavigationActionContainer.ShowImages = true;
            WebApplicationStyleManager.EnableUpperCase = false;
            WebApplication.Instance.SetLanguage("es");
            WebApplication.Instance.SetFormattingCulture("es");
            WebApplication.Instance.CustomizeFormattingCulture += Instance_CustomizeFormattingCulture;

            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }

        #region Eventos para Personalizar Sistema
        static void Instance_CustomizeFormattingCulture(object sender, CustomizeFormattingCultureEventArgs e)
        {
            e.FormattingCulture.NumberFormat.CurrencySymbol = "$";
            e.FormattingCulture.NumberFormat.CurrencyPositivePattern = 0;
            e.FormattingCulture.NumberFormat.CurrencyNegativePattern = 1;
            e.FormattingCulture.NumberFormat.CurrencyDecimalDigits = 2;
            e.FormattingCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            e.FormattingCulture.NumberFormat.CurrencyGroupSeparator = ",";
            e.FormattingCulture.NumberFormat.PercentDecimalSeparator = ".";
            e.FormattingCulture.NumberFormat.PercentDecimalDigits = 2;
            e.FormattingCulture.NumberFormat.NumberDecimalSeparator = ".";
            e.FormattingCulture.NumberFormat.NumberGroupSeparator = ",";
            e.FormattingCulture.NumberFormat.NumberDecimalDigits = 2;
        }

        /// <summary>
        /// Guardar los parámetros de la ventana de logon, justo despues que el login ha sido satisfactorio
        /// (Se guardan en una cookie). Agregar más adelante, los párametros de empresa, sucursal, bodega
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del logon que se desean guardar</param>
        private void Instance_LastLogonParametersWriting(object sender, LastLogonParametersWritingEventArgs e)
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
        private void Instance_LastLogonParametersReading(object sender, LastLogonParametersReadingEventArgs e)
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

        private void Instance_LoggedOn(object sender, LogonEventArgs e)
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

            if (WebApplication.Instance.Model != null)
            {
                var INavItems = ((IModelApplicationNavigationItems)WebApplication.Instance.Model).NavigationItems;
                INavItems.StartupNavigationItem = null;
                ((IModelRootNavigationItemsWeb)INavItems).ShowNavigationOnStart = true;
                WebApplication.Instance.Model.AboutInfoString = "{0:ProductName} - {0:Description}<br>{0:Version}<br>{0:Copyright}";
                WebApplication.Instance.Title += WebApplication.Instance.Title + " - " + ((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial;
                WebApplication.Instance.Model.Description = ((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial;
            }
        }


        #endregion


        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
