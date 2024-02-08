using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.Web;

namespace SBT.Apps.Erp.Module.Web.Controllers.SaveAndNew
{
    /// <summary>
    /// View Controller para la plataforma web y que tiene como proposito implementar el button de SaveAndNew (Guardar y Nuevo)
    /// en las paginas tipo popup que corresponden a la edicion del detalle en BO master - detail.
    /// </summary>
    /// <remarks>
    /// Por defecto se activa para todos los detail cuya IU es un popup.
    /// Las vistas que se necesiten excluir, es decir que no contengan la accion Save And New, se deben agregar en el 
    /// if del evento OnActivated.
    /// Mas informacion sobre la implementacion en
    /// https://supportcenter.devexpress.com/ticket/details/t939993/xaf-save-and-new-popup-implementation-issue
    /// https://supportcenter.devexpress.com/ticket/details/t558114/how-to-auto-refresh-a-listview-in-a-web-application-when-saving-changes-in-a-pop-up-window#
    /// Una implementacion alternativa que no se probo esta en el siguiente enlace en la respuesta de hace 5 anios (desde el 18/nov/2021)
    /// ir a otras respuestas por Dennis. Este metodo es mas simple pero hay algunas cosas que deben modificarse para que sea generico. Sin embargo,
    /// es interesante revisar cada linea de la implementacion porque ilustra el uso de ObjectSpace, por ejemplo como acceder al parent objectspace
    /// desde el nested objectspace (del detalle)
    /// https://supportcenter.devexpress.com/ticket/details/t219521/how-to-add-the-save-and-new-action-in-a-detaiview-shown-for-the-aggregated-detail
    /// </remarks>
    public class SaveAndNewViewControllerWeb : ViewController<DetailView>
    {
        internal DevExpress.ExpressApp.Actions.SimpleAction SaveAndNew_Button;
        public SaveAndNewViewControllerWeb()
        {
            SaveAndNew_Button = new DevExpress.ExpressApp.Actions.SimpleAction(this, "SaveAndNew_Button",
                DevExpress.Persistent.Base.PredefinedCategory.PopupActions.ToString()); // DevExpress.Persistent.Base.PredefinedCategory.Save);
            SaveAndNew_Button.Caption = "Save And New";
            SaveAndNew_Button.ConfirmationMessage = null;
            SaveAndNew_Button.ToolTip = null;
            SaveAndNew_Button.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(SaveAndNew_Button_Execute);
            Actions.Add(SaveAndNew_Button);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // las vistas para las cuales deshabilitamos la accion SaveAndNew. Agregar las vistas en el if
            string Id = View.Id;
            if ((Id == "AuditObjectInfo_DetailView") || (Id == "BO2_DetailView") ||
                (Id == "BO3_DetailView") || (Id == "Bo4_DetailView") || (Id == "RangoFechaParams_DetailView") ||
                (Id == "LibroIvaParams_DetailView"))
            {
                SaveAndNew_Button.Active.SetItemValue("SaveAndNew_Button", false);
            }
            else
            {
                UpdateButtonStatus();
            }

            if (View is DetailView detailView)
            {
                ((WebLayoutManager)detailView.LayoutManager).PageControlCreated += SaveAndNewViewControllerWeb_PageControlCreated;
            }

            // para refrescar la lista de detalle en la vista principal o master, cuando se terminan de agregar items en el popup
            // ver: https://supportcenter.devexpress.com/ticket/details/t558114/how-to-auto-refresh-a-listview-in-a-web-application-when-saving-changes-in-a-pop-up-window#
            DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
            if (popupWindow != null)
            {
                popupWindow.RefreshParentWindowMode = RefreshParentWindowMode.OnAccept | RefreshParentWindowMode.OnCancel;
            }

        }

        protected override void OnDeactivated()
        {
            if (View is DetailView detailView)
            {
                ((WebLayoutManager)detailView.LayoutManager).PageControlCreated -= SaveAndNewViewControllerWeb_PageControlCreated;
            }

            base.OnDeactivated();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void SaveAndNew_Button_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.ObjectSpace.IsModified)
            {
                View.ObjectSpace.CommitChanges();
            }
            View.Close(false);
            DetailView parentDetailView = (DetailView)Application.MainWindow.View;
            PopupWindowController frameRoudMap = Application.MainWindow.GetController<PopupWindowController>();
            if (frameRoudMap != null)
            {
                Frame parentFrame = frameRoudMap.ParentFrame;
                if (parentFrame != null)
                {
                    NewObjectViewController targetNewController = parentFrame.GetController<NewObjectViewController>();
                    ChoiceActionItem targetNewItem = targetNewController.NewObjectAction.Items[0];
                    targetNewController.NewObjectAction.DoExecute(targetNewItem);
                }

            }
        }

        public void UpdateButtonStatus()
        {
            if ((View.ViewEditMode == ViewEditMode.Edit))
            {
                SaveAndNew_Button.Active.SetItemValue("SaveAndNew_Button", true);
            }
            else
            {
                SaveAndNew_Button.Active.SetItemValue("SaveAndNew_Button", false);
            }
        }

        private void MySaveAndNew_Button_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.ObjectSpace.IsModified)
            {
                View.ObjectSpace.CommitChanges();
            }
            View.Close(false);
            DetailView parentDetailView = (DetailView)Application.MainWindow.View;
            PopupWindowController frameRoudMap = Application.MainWindow.GetController<PopupWindowController>();
            if (frameRoudMap != null)
            {
                Frame parentFrame = frameRoudMap.ParentFrame;
                if (parentFrame != null)
                {
                    NewObjectViewController targetNewController = parentFrame.GetController<NewObjectViewController>();
                    ChoiceActionItem targetNewItem = targetNewController.NewObjectAction.Items[0];
                    targetNewController.NewObjectAction.DoExecute(targetNewItem);
                }

            }
        }

        private void SaveAndNewViewControllerWeb_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            e.PageControl.ActiveTabChanged += PageControl_ActiveTabChanged;
        }

        private void PageControl_ActiveTabChanged(object source, TabControlEventArgs e)
        {
            var listPropertyEditor = View.FindItem(e.Tab.Name) as ListPropertyEditor;
            if (listPropertyEditor != null)
            {
                if (listPropertyEditor.Frame is NestedFrame)
                {
                    PopupWindowController frameRoudMap = Application.MainWindow.GetController<PopupWindowController>();
                    frameRoudMap.Pop();
                    frameRoudMap.AddFrame(Frame);
                }
            }
        }
    }
}
