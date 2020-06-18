using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion para identificar las maneras que se pueden utilizar para calcular el aguinaldo
    /// </summary>
    public enum EMetodoAguinaldo
    {
        [XafDisplayName("Días Salario")]
        DiasSalario = 0,
        [XafDisplayName("Porcentaje Salario")]
        Porcentaje = 1,
        [XafDisplayName("Monto Fijo")]
        MontoFijo = 2
    }
}
