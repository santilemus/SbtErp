namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    public class BancoTransaccionListViewControllerWeb : BaseListViewControllerWeb
    {
        public BancoTransaccionListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.BancoTransaccion);
        }
    }

    public class BancoChequeraListViewControllerWeb: BaseListViewControllerWeb
    {
        public BancoChequeraListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Banco.Module.BusinessObjects.BancoChequera);
        }
    }


}
