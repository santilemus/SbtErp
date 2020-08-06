using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    [NonPersistent]
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
