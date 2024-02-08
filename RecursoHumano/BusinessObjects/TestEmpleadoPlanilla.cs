using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    [DomainComponent]
    public class TestEmpleadoPlanilla
    {

        public Empleado.Module.BusinessObjects.Empleado Empleado { get; set; }
        public int DiasLicenciaSinSueldo { get; set; }
        public int DiasInasistencia { get; set; }
        public int DiasAmonestacion { get; set; }
        public int DiasIncapacidad { get; set; }
        public int DiasMaternidad { get; set; }
        public decimal TotalHorasExtra { get; set; }
        public decimal CotizaAcumuladaIsss { get; set; }
        public decimal CotizaAcumuladaAfp { get; set; }
        public decimal CotizaAcumuladaRenta { get; set; }
        public decimal IngresoBrutoQuincena { get; set; }
    }
}
