using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Forma de pago de los diferentes tipos de planilla. Pueden ser: Quincenal, Semanal, Anual, Catorcena
    /// </summary>
    public enum EFormaPago
    {
        Semanal = 1,
        Catorcena = 2,
        Quincenal = 3,
        Mensual = 4,
        Anual = 5
    }
}
