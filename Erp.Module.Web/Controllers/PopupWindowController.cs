using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using System;
using System.Collections.Generic;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// Configurar el tamaño y estilo por defecto de los popup window
    /// </summary>
    /// <remarks>
    /// mas info en: https://docs.devexpress.com/eXpressAppFramework/113456/task-based-help/miscellaneous-ui-customizations/how-to-adjust-the-size-and-style-of-pop-up-dialogs-asp-net
    /// </remarks>
    public class PopupWindowController : WindowController
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

        #region Para implementar SaveAndNew button en los popup de ingreso de datos de detalle
        /// <summary>
        /// Agregado el 18/nov/2021. Si da problemas con la funcionalidad ya disponible, separar en otro WindowController
        /// Es parte de la implementacion del action Save And New (Guardar y Nuevo) en los BO que se editan en un
        /// popup porque corresponden al detail de una relacion Master-Detail. Mas info en:
        /// https://supportcenter.devexpress.com/ticket/details/t939993/xaf-save-and-new-popup-implementation-issue
        /// </summary>

        private Stack<Frame> framesMap = new Stack<Frame>();
        public void AddFrame(Frame frame)
        {
            if (!framesMap.Contains(frame))
            {
                framesMap.Push(frame);
                frame.Disposed += frame_Disposed;
            }
        }
        public Frame ParentFrame
        {
            //get { return framesMap.Peek(); }
            get { return framesMap.Count > 0 ? framesMap.Peek() : null; }
        }

        private void frame_Disposed(object sender, EventArgs e)
        {
            Frame frame = framesMap.Pop();
            frame.Disposed -= frame_Disposed;
        }

        public void Pop()
        {
            framesMap.Pop();
        }

        #endregion
    }
}
