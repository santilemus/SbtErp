using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Security.Resources;
using DevExpress.Web;
using DevExpress.XtraReports.Security;
using DevExpress.XtraReports.UI;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Configuration;

namespace SBT.Apps.Medico.Web
{
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e)
        {
            SecurityAdapterHelper.Enable();
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = FrameworkSettingsCompatibilityMode.Latest; //DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.v20_1;
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            WebApplication.EnableMultipleBrowserTabsSupport = true;

            // Style sheets cannot be loaded from file directories; only XML report layout serialization format is allowed
            DevExpress.Security.Resources.AccessSettings.ReportingSpecificResources.SetRules(DirectoryAccessRule.Deny(), SerializationFormatRule.Deny(SerializationFormat.Xml));

#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new medicoAspNetApplication());
            SecurityStrategy security = (SecurityStrategy)WebApplication.Instance.Security;
            security.RegisterXPOAdapterProviders();
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.SwitchToNewStyle();
            if (ConfigurationManager.ConnectionStrings["medico"] != null)
            {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["medico"].ConnectionString;
            }
            WebApplication.Instance.LastLogonParametersWriting += new EventHandler<LastLogonParametersWritingEventArgs>(Instance_LastLogonParametersWriting);
            WebApplication.Instance.LastLogonParametersReading += new EventHandler<LastLogonParametersReadingEventArgs>(Instance_LastLogonParametersReading);
            WebApplication.Instance.LoggedOn += new EventHandler<LogonEventArgs>(Instance_LoggedOn);

#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
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
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e)
        {
        }

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
        /// Guardar los parametros de la ventana de logon, justo despues que el login ha sido satisfactorio
        /// (Se guardan en una cookie). Agregar más adelante, los párametros de empresa, sucursal, bodega
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">parámetros del logon que se desean guardar</param>
        private void Instance_LastLogonParametersWriting(object sender, LastLogonParametersWritingEventArgs e)
        {
            if (string.Compare(((CustomLogonParameters)e.LogonObject).UserName, "Admin", StringComparison.Ordinal) != 0)
            {
                e.SettingsStorage.SaveOption(string.Empty, "UserName", ((CustomLogonParameters)e.LogonObject).UserName);
            }

            System.Globalization.CultureInfo cci = System.Globalization.CultureInfo.CurrentCulture;
            e.SettingsStorage.SaveOption(string.Empty, "Empresa", ((CustomLogonParameters)e.LogonObject).Empresa.Oid.ToString(cci.NumberFormat));
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

            // las siguientes dos lineas son por si se necesitara hacer algo con la informacion del equipo remoto (caso de POS, para identificar las cajas)
            //Sanrey.Erp.Base.BusinessObjects.SesionDataHelper.Agregar("UserHostName", Request.UserHostName);
            //Sanrey.Erp.Base.BusinessObjects.SesionDataHelper.Agregar("UserHostAddress", Request.UserHostAddress);

            if (WebApplication.Instance.Model != null)
            {
                var INavItems = ((IModelApplicationNavigationItems)WebApplication.Instance.Model).NavigationItems;
                INavItems.StartupNavigationItem = null;
                ((IModelRootNavigationItemsWeb)INavItems).ShowNavigationOnStart = true;

                // la siguiente linea esta a prueba desde el 06/agosto/2020, porque las collecciones de detalle no se muestran cuando la aplicación
                // se despliega en modo release, pero si en modo debug. La idea es hacer un despliegue release con esta linea, para ver que sucede
                ((IModelOptionsWeb)WebApplication.Instance.Model.Options).CollectionsEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;

                //WebApplication.Instance.Model.AboutInfoString = "{0:ProductName} - {0:Description}<br>{0:Version}<br>{0:Copyright}";
                WebApplication.Instance.Model.AboutInfoString = "{0:ProductName}, {0:Version}<br>{0:Description}";

                //WebApplication.Instance.Title += WebApplication.Instance.Title + " [" + ((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial + "]";
                // WebApplication.Instance.Model.Description = ((CustomLogonParameters)e.LogonParameters).Empresa.RazonSocial;
            }
        }


        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
