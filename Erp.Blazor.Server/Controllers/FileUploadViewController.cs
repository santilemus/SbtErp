using DevExpress.ExpressApp;
using DevExpress.ExpressApp.FileAttachments.Blazor.Editors;
using DevExpress.ExpressApp.FileAttachment.Blazor.Editors.Adapters;
using System.Reflection;
using SBT.Apps.Base.Module.BusinessObjects;


namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    public class FileUploadViewController : ObjectViewController<DetailView, FileUploadParameter>
    {
        private System.ComponentModel.IContainer components = null;
        public FileUploadViewController()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.CustomizeViewItemControl<FileDataPropertyEditor>(this, customizeAction);
        }

        private void customizeAction(FileDataPropertyEditor editor)
        {
            if (editor.Control is FileDataEditorComponentAdapter fileDataEditorComponentAdapter)
            {
                var type = fileDataEditorComponentAdapter.GetType();
                var createComponent = type.GetMethod("CreateComponent", BindingFlags.NonPublic | BindingFlags.Instance);
                var ttt = createComponent.GetType();
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        //[HttpPost]
        //[Route("UploadFile")]
        //public IActionResult UploadFile(IFormFile myFile)
        //{

        //    // Check whether the file can be uploaded.
        //    // ...
        //    BadRequestObjectResult result = new BadRequestObjectResult(@"Ocurrió un error durante la carga del archivo");
        //    return result;
        //}
    }
}
