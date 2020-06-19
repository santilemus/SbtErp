using System.Web.UI.WebControls;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// Configurar el tamaño y estilo por defecto de los popup window
    /// </summary>
    /// <remarks>
    /// mas info en: https://docs.devexpress.com/eXpressAppFramework/113456/task-based-help/miscellaneous-ui-customizations/how-to-adjust-the-size-and-style-of-pop-up-dialogs-asp-net
    /// </remarks>
    public class PopupWindowController: WindowController
    {
        public PopupWindowController()
        {
            this.TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
        }
        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            e.PopupControl.CustomizePopupWindowSize += XafPopupWindowControl_CustomizePopupWindowSize;
        }
        private void XafPopupWindowControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            if (e.ShowViewSource == null || e.ShowViewSource.SourceView == null)
                return;
            if (!e.ShowViewSource.SourceView.ObjectTypeInfo.IsPersistent &&
               (e.ShowViewSource.SourceView.ObjectTypeInfo.Type != typeof(DevExpress.ExpressApp.ReportsV2.NewReportWizardParameters)))
            {
                //e.Width = new Unit(500);
                //e.Height = new Unit(450);
                e.ShowPopupMode = ShowPopupMode.Centered;
            }
            else
                e.ShowPopupMode = ShowPopupMode.ByDefault;

            //if ((e.ShowViewSource.SourceView.ObjectTypeInfo.Type == typeof(DevExpress.Persistent.BaseImpl.ReportDataV2)) || 
            //     (e.ShowViewSource.SourceView.ObjectTypeInfo.Type == typeof(DevExpress.ExpressApp.ReportsV2.NewReportWizardParameters)))
            //{
            //    e.ShowPopupMode = ShowPopupMode.Slide;
            //}
            //else
            //{
            //    e.Width = new Unit(500);
            //    e.Height = new Unit(450);
            //    e.ShowPopupMode = ShowPopupMode.Centered;
            //}
            e.Handled = true;             
        }

        /// <summary>
        /// Borrar si da problemas
        /// </summary>
        protected override void OnDeactivated()
        {
            ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
            base.OnDeactivated(); 
        }
    }
}
