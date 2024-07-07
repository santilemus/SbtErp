namespace SBT.Apps.Erp.Blazor.Server.Controllers.Venta
{
    public class CxCTransaccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public CxCTransaccionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(CxC.Module.BusinessObjects.CxCTransaccion);
        }
    }
}
