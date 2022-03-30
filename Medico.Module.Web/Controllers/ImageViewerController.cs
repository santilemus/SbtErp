using System;
using System.IO;
using System.Web;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using SBT.Apps.Medico.Expediente.Module.BusinessObjects;
using SBT.Apps.Medico.Module.Web.Classes;
using DevExpress.ExpressApp.Web;


namespace SBT.Apps.Medico.Module.Web.Controllers
{
    /// <summary>
    /// View Controller para mostrar en una ventana separada un objeto del tipo PacienteFileData
    /// </summary>
    /// <remarks>
    /// Mas informacion: https://supportcenter.devexpress.com/ticket/details/t751790/how-to-open-a-picture-in-a-separate-window-when-a-user-clicks-an-action
    ///                  https://docs.devexpress.com/eXpressAppFramework/DevExpress.Persistent.Base.IFileData.SaveToStream(System.IO.Stream)
    /// </remarks>
    public class ImageViewerController : ViewController
    {
        private SimpleAction saShowPicture;
        public ImageViewerController() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.PacienteFileData);
            saShowPicture = new SimpleAction(this, "saShowPicture", PredefinedCategory.RecordEdit.ToString());
            saShowPicture.TargetObjectType = typeof(SBT.Apps.Medico.Expediente.Module.BusinessObjects.PacienteFileData);
            saShowPicture.Caption = "Mostrar Archivo";
            saShowPicture.ImageName = "Preview";
            saShowPicture.TargetViewType = ViewType.ListView;
            saShowPicture.ToolTip = "Abrir y Mostrar el documento";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            saShowPicture.Execute += saShowPicture_Execute;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            saShowPicture.Execute -= saShowPicture_Execute;
        }

        private void saShowPicture_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (e.CurrentObject == null)
                return;
            PacienteFileData fd = (ObjectSpace.GetObject(e.CurrentObject) as PacienteFileData);
            if (fd.File != null && !fd.File.IsEmpty)
            {
                try
                {
                    string fileName = fd.File.FileName;
                    string extension = Path.GetExtension(fileName);

                    using (var stream = new MemoryStream())
                    {
                        fd.File.SaveToStream(stream);
                        ImageViewer pictViewer = new ImageViewer();
                        stream.Position = 0;
                        pictViewer.bytes = stream.ToArray();
                        pictViewer.fileName = fileName;

                        pictViewer.contentType = GetContentType(extension.ToLower().Substring(1));
                        pictViewer.ProcessRequest(HttpContext.Current);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private string GetContentType(string extension)
        {
            if (extension == "png" || extension == "jpg" || extension == "jpeg")
                return string.Format("image/{0}", extension);
            else if (extension == "pdf")
                return string.Format("application/{0}", extension);
            else if (extension == "mpeg" || extension == "webm" || extension == "ogg" || extension == "wav" || extension == "midi")
                return string.Format("audio/{0}", extension);
            else
                return string.Format("application/octet-stream");
        }

    }
}
