using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Erp.Module.Controllers
{
    /// <summary>
    /// Window Controller independiente de la plataforma. Poner aqui las acciones y el codigo que es
    /// comun a las diferentes plataformas 
    /// </summary>
    public class WindowsMainController: WindowController
    {
        private readonly SimpleAction saPurgeRecord;
        public WindowsMainController()
        {
            TargetWindowType = WindowType.Main;
            saPurgeRecord = new(this, "saPurgarRegistrosEliminados", DevExpress.Persistent.Base.PredefinedCategory.Tools.ToString())
            {
                ToolTip = @"Eliminar de la base de datos los objetos borrados de la aplicación con la caraçterística de eliminación diferida",
                Caption = "Purgar",
                ImageName = "trash-bin"
            };
            saPurgeRecord.Execute += SaPurgeRecord_Execute;
            saPurgeRecord.ConfirmationMessage = @"Esta seguro de borrar permanentemente los registros eliminados (Y/N)";
        }

        private void SaPurgeRecord_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            using (IObjectSpace os = Application.ObjectSpaceProvider.CreateObjectSpace())
            {
                var res = (os as XPObjectSpace).Session.PurgeDeletedObjects();
                Application.ShowViewStrategy.ShowMessage($@"Son {res.Purged} los objetos eliminados permanentemente, {res.NonPurged} no se pudieron eliminar");
            }
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
            if (SecuritySystem.CurrentUser != null && ((Usuario)SecuritySystem.CurrentUser).Empresa != null) 
                e.WindowCaption.Text = $"{Application.Model.Title} - {((Usuario)SecuritySystem.CurrentUser).Empresa.RazonSocial}";
        }
    }
}
