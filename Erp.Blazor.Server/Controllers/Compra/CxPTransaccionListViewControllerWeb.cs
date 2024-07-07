namespace SBT.Apps.Erp.Blazor.Server.Controllers.Compra
{
    public class CxPTransaccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public CxPTransaccionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(CxP.Module.BusinessObjects.CxPTransaccion);
        }
    }
}
