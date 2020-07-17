using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.Web.ASPxRichEdit;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ViewController Web BugReport. Para agilizar el control aspxrichtexteditor al ocultar el  Ribbon
    /// </summary>
    public class vcBugReportShowRibbonController: ObjectViewController<ListView, SBT.Apps.Base.Module.BusinessObjects.BugReport>
    {
        public vcBugReportShowRibbonController()
        {
            // cuando es listview
            SimpleAction showRibbonAction = new SimpleAction(this, "Mostrar Ribbon", PredefinedCategory.Edit);
            showRibbonAction.Execute += Action_Execute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View != null && View.EditView != null)
            {
                foreach (var editor in View.EditView.GetItems<ASPxRichTextPropertyEditor>())
                {
                    editor.MenuManagerType = WebMenuManagerType.None;
                }
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void Action_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            foreach (var editor in View.EditView.GetItems<ASPxRichTextPropertyEditor>())
            {
                editor.SetRibbonMode(RichEditRibbonMode.OneLineRibbon);
                // optionally, you can display only the Home tab to speed up Ribbon loading
                editor.ASPxRichEditControl.RibbonTabs.ForEach(rt => { if (!(rt is DevExpress.Web.ASPxRichEdit.RERHomeTab)) rt.Visible = false; });
            }
        }

    }
}
