using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win.Templates;
using SBT.Apps.Base.Module.BusinessObjects;
using System.Drawing;

namespace SBT.Apps.Erp.Module.Win.Controllers
{
    public class LoginController: ViewController
    {
        public LoginController() 
        {
            TargetObjectType = typeof(CustomLogonParameters);
            TargetViewId = @"CustomLogonParameters_DetailView";

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            //PopupFormBase form = Frame.Template as PopupFormBase;
            LogonPopupForm form = Frame.Template as LogonPopupForm;
            if (form != null)
            {
                form.AutoShrink = false;
                form.Size = new Size(650, 330);
            }
        }
    }
}
