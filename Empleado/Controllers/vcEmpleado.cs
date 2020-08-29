using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

using SBT.Apps.Empleado.Module.BusinessObjects;

namespace SBT.Apps.Empleado.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class vcEmpleado : ViewController
    {
        private PopupWindowShowAction pwsaDocumentosDigitales;
        private MayanDocumentsReadResponse mayanDocumento;  /// solo para pruebas, borrar despues

        public vcEmpleado()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            pwsaDocumentosDigitales = new PopupWindowShowAction(this, "idDocumentosDigitales", PredefinedCategory.RecordEdit);
            pwsaDocumentosDigitales.TargetObjectType = typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado);
            pwsaDocumentosDigitales.TargetViewType = ViewType.DetailView;
            pwsaDocumentosDigitales.Caption = "Documentos";
            pwsaDocumentosDigitales.ToolTip = "Prueba de consumir servicios del gestor de documentos";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (View.ObjectTypeInfo.Type == typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado))
            {
                if (View.GetType().Name == "ListView")
                    ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = new BinaryOperator("Empresa", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
            }
            pwsaDocumentosDigitales.CustomizePopupWindowParams += pwsaDocumentosDigitales_CustomizePopupWindowsParams;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            pwsaDocumentosDigitales.CustomizePopupWindowParams -= pwsaDocumentosDigitales_CustomizePopupWindowsParams;
        }

        private void pwsaDocumentosDigitales_CustomizePopupWindowsParams(Object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace();
            if (mayanDocumento == null)
                mayanDocumento = os.CreateObject<MayanDocumentsReadResponse>();

            GetDocument();
            e.View = Application.CreateDetailView(os, mayanDocumento);
        }

        private async void GetDocument()
        {
            // clavados para probar
            string sWebUrl = $"http://192.168.1.104:8000/api/auth/token/obtain/";
            string sToken = $"";
            using (var client = new System.Net.Http.HttpClient())
            {
                var pp = new Dictionary<string, string>();
                pp.Add("username", "slemus");
                pp.Add("password", "Dumbo#2020");
                var encodedContent = new System.Net.Http.FormUrlEncodedContent(pp);
                client.BaseAddress = new Uri(sWebUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.ConnectionClose = true;
                Dictionary<string, string> token = new Dictionary<string, string>();
                using (var response = await client.PostAsync(sWebUrl, encodedContent).ConfigureAwait(false))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        sToken = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        token = JsonSerializer.Deserialize<Dictionary<string, string>>(sToken, new JsonSerializerOptions() { AllowTrailingCommas = true, IgnoreNullValues = true });
                        //token = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sToken);
                    }
                }
                if (string.IsNullOrEmpty(sToken))
                    return;
                // ahora intentamos recuperar un documento
                //sWebUrl = $"http://192.168.1.104:8000/api/documents/{id}/";
                sWebUrl = $"http://192.168.1.104:8000/api/documents/4/";
                client.DefaultRequestHeaders.Add("Authorization", "Token " + token["token"]);
                MayanDocumentsReadResponse mayandocumentread = new MayanDocumentsReadResponse();
                using (var response = await client.GetAsync(sWebUrl).ConfigureAwait(false))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    { 
                        //string respuesta = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        string respuesta = response.Content.ReadAsStringAsync().Result;
                        // mayandocumentread = Newtonsoft.Json.JsonConvert.DeserializeObject<MayanDocumentsReadResponse>(respuesta);
                        mayanDocumento = JsonSerializer.Deserialize<MayanDocumentsReadResponse>(respuesta,
                            new JsonSerializerOptions() { AllowTrailingCommas = true, IgnoreNullValues = true });
                    }
                }
            }
        }

    }

    //public class Token
    //{
    //    public string Key { get; set; }
    //    public string Value { get; set; }
    //}
}
