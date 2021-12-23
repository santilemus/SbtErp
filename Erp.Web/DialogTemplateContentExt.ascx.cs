using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Templates;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace SBT.Apps.Erp.Web
{
    public partial class DialogTemplateContentExt : TemplateContent, IXafPopupWindowControlContainer
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.ClientScript.RegisterClientScriptResource(typeof(WebWindow), "DevExpress.ExpressApp.Web.Resources.JScripts.PopupTemplate.js");
            if (WebWindow.CurrentRequestWindow != null)
            {
                WebWindow.CurrentRequestWindow.PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);
            }
        }
        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            WebWindow window = (WebWindow)sender;
            window.RegisterStartupScript("Init", "Init();");
        }
        protected override void OnUnload(EventArgs e)
        {
            if (WebWindow.CurrentRequestWindow != null)
            {
                WebWindow.CurrentRequestWindow.PagePreRender -= new EventHandler(CurrentRequestWindow_PagePreRender);
            }
            base.OnUnload(e);
        }
        public override void SetStatus(ICollection<string> statusMessages)
        {
        }
        public override IActionContainer DefaultContainer
        {
            get { return null; }
        }
        public override object ViewSiteControl
        {
            get { return VSC; }
        }
        public XafPopupWindowControl XafPopupWindowControl
        {
            get { return PopupWindowControl; }
        }
        public override void BeginUpdate()
        {
            base.BeginUpdate();
            this.SAC.BeginUpdate();
        }
        public override void EndUpdate()
        {
            this.SAC.EndUpdate();
            base.EndUpdate();
        }
    }
}
