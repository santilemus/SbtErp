using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Medico.Blazor.Server.Controllers
{
    /// <summary>
    /// Object ViewController para ocultar el botón de edición del BO CustomLogonParameters
    /// </summary>
    /// <remarks>
    /// mas info en: https://docs.devexpress.com/eXpressAppFramework/112982/data-security-and-safety/security-system/authentication/customize-standard-authentication-behavior-and-supply-additional-logon-parameters
    /// </remarks>
    public class CustomLogonEditActionVisibilityController : ObjectViewController<DetailView, CustomLogonParameters>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            View.CustomizeViewItemControl<LookupPropertyEditor>(this, e => {
                e.HideEditButton();
            });
        }
    }
}
