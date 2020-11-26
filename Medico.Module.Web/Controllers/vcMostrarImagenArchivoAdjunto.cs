
using System.IO;
using System.Web;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using SBT.Apps.Medico.Module.Web.Classes;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx
    /// https://supportcenter.devexpress.com/ticket/details/t751790/how-to-open-a-picture-in-a-separate-window-when-a-user-clicks-an-action
    /// https://supportcenter.devexpress.com/ticket/details/t751219/show-png-file-or-pdf-file-in-browser-window-in-xaf-web-application
    /// </remarks>
    public class vcMostrarImagenArchivoAdjunto: ViewController<ListView>
    {
        private SimpleAction saMostrarAdjunto;

        public vcMostrarImagenArchivoAdjunto(): base()
        {
            DoInitializeComponent();
        }

        private void DoInitializeComponent()
        {
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.ArchivoAdjunto);     
            saMostrarAdjunto = new SimpleAction(this, "saMostrarAdjunto", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            saMostrarAdjunto.Caption = "Ver Documento";
            //saMostrarAdjunto.Category = "RecordEdit";
            saMostrarAdjunto.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            saMostrarAdjunto.ImageName = "DocumentStatistics";
            saMostrarAdjunto.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.ArchivoAdjunto);
            saMostrarAdjunto.TargetViewType = ViewType.ListView;
            saMostrarAdjunto.ToolTip = "Mostrar visor del documento";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            saMostrarAdjunto.Execute += saMostrarAdjunto_Execute;
        }

        protected override void OnDeactivated()
        {
            saMostrarAdjunto.Execute -= saMostrarAdjunto_Execute;
            base.OnDeactivated();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Acceso y personalizacion de target View control.  
        }

        //protected override void Dispose(bool disposing)
        //{
        //    saMostrarAdjunto.Dispose();
        //    base.Dispose(disposing);
        //}

        private void saMostrarAdjunto_Execute(object Sender, SimpleActionExecuteEventArgs e)
        {
            if (e.CurrentObject == null)
                return;
            ArchivoAdjunto archivo = (ArchivoAdjunto)ObjectSpace.GetObject(e.CurrentObject);
            string nombreArchivo = archivo.File.FileName;
            string extension = Path.GetExtension(nombreArchivo);
            WebDocumentViewer webDocViewer = new WebDocumentViewer
            {
                bytes = archivo.File.Content,
                fileName = nombreArchivo,
                contentType = string.Format("image/{0}", extension.Substring(1))
            };
            webDocViewer.ProcessRequest(HttpContext.Current);
        }

    }
}
