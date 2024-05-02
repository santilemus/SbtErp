using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Templates;
using System.Drawing;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class LoginController : WindowController
    {
        public LoginController(): base()
        {
            
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            Window.TemplateChanged += Window_TemplateChanged;
        }

        private void Window_TemplateChanged(object sender, EventArgs e)
        {
            if (Window.Template is LogonWindowTemplate
                && Window.View.Id == "CustomLogonParameters_DetailView")
            {
                Window.Template.IsSizeable = true;
                /*
                size.MaxWidth = "100vw";
                size.Width = "1900px";
                size.MaxHeight = "100vh";
                size.Height = "700px";
                */
            }
        }

        protected override void OnDeactivated()
        {
            Window.TemplateChanged -= Window_TemplateChanged;
            base.OnDeactivated();
        }
    }
}
