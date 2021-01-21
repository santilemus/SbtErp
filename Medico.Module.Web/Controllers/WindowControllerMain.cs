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

namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// WindowController que se aplica a la pagina principal de la aplicación cuando es plataforma web
    /// </summary>
    /// <remarks>
    /// or more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    /// </remarks>
    public class WindowControllerMain : WindowController
    {
        private ActionUrl ayudaUrl;
        public WindowControllerMain()
        {
            ayudaUrl = new ActionUrl(this, "AyudaUrlAction", "Security");
            ayudaUrl.ImageName = "help";
            ayudaUrl.Caption = "Ayuda";
            ayudaUrl.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            ayudaUrl.ToolTip = "Mostrar la ayuda del sistema";
            ayudaUrl.UrlFormatString = "ayuda/index.html";
            ayudaUrl.TargetViewType = ViewType.Any;
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        protected override void Dispose(bool disposing)
        {
            ayudaUrl.Dispose();
            base.Dispose(disposing);
        }
    }

}
