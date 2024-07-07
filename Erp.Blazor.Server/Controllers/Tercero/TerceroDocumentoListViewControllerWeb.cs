namespace SBT.Apps.Erp.Blazor.Server.Controllers.Tercero
{
    public class TerceroDocumentoListViewControllerWeb : BaseListViewControllerWeb
    {
        public TerceroDocumentoListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Tercero.Module.BusinessObjects.TerceroDocumento);
        }
    }
}
