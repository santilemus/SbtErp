using SBT.Apps.Facturacion.Module.BusinessObjects;

namespace SBT.Apps.Erp.Module.Controllers.Contabilidad
{
    /// <summary>
    /// Controller que permite habilitar la generación de asientos contables de los documentos de venta emitidos
    /// </summary>
    public class VentaPartidaContableController: IntegracionAsientoContableController
    {
        public VentaPartidaContableController()
        {
            DoSourceBO<Venta>("Venta - Generar Partida", DevExpress.ExpressApp.ViewType.DetailView);
        }
    }
}
