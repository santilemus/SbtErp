using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.SystemModule;

namespace SBT.Apps.Erp.Module.Web.Controllers.SaveAndNew
{
    /// <summary>
    /// Controlador personalizado para la creacion de nuevos objetos en la plataforma web, cuando se trata del ingreso
    /// de datos en una pagina tipo popup y que corresponde al detalle de un BO master-detail (por eso if (Frame is NestedFrame)
    /// </summary>
    /// <remarks>
    /// mas informacion en https://supportcenter.devexpress.com/ticket/details/t939993/xaf-save-and-new-popup-implementation-issue
    /// </remarks>
    public class CustomWebNewObjectViewController : WebNewObjectViewController
    {
        protected override void New(DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventArgs args)
        {
            if (Frame is NestedFrame)
            {
                PopupWindowController frameRoudMap = Application.MainWindow.GetController<PopupWindowController>();
                frameRoudMap.AddFrame(Frame);
            }
            base.New(args);

        }
    }
}
