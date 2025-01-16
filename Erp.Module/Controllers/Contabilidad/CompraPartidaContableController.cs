using SBT.Apps.Compra.Module.BusinessObjects;

namespace SBT.Apps.Erp.Module.Controllers.Contabilidad
{
    public class CompraPartidaContableController: IntegracionAsientoContableController
    {
        public CompraPartidaContableController()
        {
            DoSourceBO<CompraFactura>("Generar Partida", DevExpress.ExpressApp.ViewType.DetailView);
        }
    }
}
