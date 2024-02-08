using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using System.IO;
using System.Windows.Forms;

namespace SBT.Apps.Medico.Module.Win.Controllers
{
    /// <summary>
    /// View controller para la plataforma WinForm de la aplicacion
    /// </summary>
    public class WindowControllerMain : WindowController
    {
        private SimpleAction saAyuda;
        public WindowControllerMain()
        {
            TargetWindowType = WindowType.Main;
            saAyuda = new SimpleAction(this, "AyudaUrlAction", DevExpress.Persistent.Base.PredefinedCategory.Tools);
            saAyuda.ImageName = "help";
            saAyuda.Caption = "Ayuda";
            saAyuda.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            saAyuda.ToolTip = "Mostrar la ayuda del sistema";
            saAyuda.QuickAccess = true;
            //saAyuda.TargetViewType = ViewType.ListView
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            saAyuda.Execute += saAyuda_Execute;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            saAyuda.Execute -= saAyuda_Execute;
        }

        protected override void Dispose(bool disposing)
        {
            saAyuda.Dispose();
            base.Dispose(disposing);
        }

        private void saAyuda_Execute(object Sender, SimpleActionExecuteEventArgs e)
        {
            var frm = (Form)Application.MainWindow.Template;
            if (frm == null)
                return;
            string sArchivo = $"{Path.GetDirectoryName(Application.GetType().Assembly.Location)}\\Medico.chm";
            if (File.Exists(sArchivo))
            {
                System.Windows.Forms.Help.ShowHelp(frm, sArchivo);
            }
        }
    }
}
