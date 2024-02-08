using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using SBT.Apps.Base.Module;
using SBT.Apps.Base.Module.BusinessObjects;
using DevExpress.ExpressApp.Model;

namespace SBT.Apps.Erp.Module.Win.Controllers
{
    /// <summary>
    /// Window Controller que aplica a las personalizaciones de la ventana principal de la aplicacion WinForm
    /// </summary>
    /// <remarks>
    /// INfo en: https://docs.devexpress.com/eXpressAppFramework/113252/task-based-help/miscellaneous-ui-customizations/how-to-customize-a-window-caption
    /// NOTA: 07/07/2023 migracion a NET6. Revisar despues porque se agrego este mismo controller en el proyecto SBT.Apps.Erp.Module. Si se necesita para
    /// otras personalizaciones en todo caso debe de heredar del controllador agregado en el proyecto independiente de la plataforma.
    /// </remarks>
    public class WindowsMainController: WindowController
    {
        public WindowsMainController()
        {
            TargetWindowType = WindowType.Main;
            
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            WindowTemplateController controller = Frame.GetController<WindowTemplateController>();
            controller.CustomizeWindowCaption += Controller_CustomizeWindowCaption;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            WindowTemplateController controller = Frame.GetController<WindowTemplateController>();
            controller.CustomizeWindowCaption -= Controller_CustomizeWindowCaption;
        }

        private void Controller_CustomizeWindowCaption(object sender, CustomizeWindowCaptionEventArgs e)
        {           
            e.WindowCaption.Text = $"{Application.Model.Title} - {((Usuario)SecuritySystem.CurrentUser).Empresa.RazonSocial}";
        }
    }
}
