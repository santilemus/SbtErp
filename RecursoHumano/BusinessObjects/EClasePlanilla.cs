using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Enumeración con las clases de planilla. Evaluar si se agregan: hora extras, comisión, servicios profesionales, premios, dietas
    /// </summary>
    public enum EClasePlanilla
    {
        [XafDisplayName("Salarios")]
        Salarios = 1,
        [XafDisplayName("Bonificación")]
        Bonificacion = 2,
        [XafDisplayName("Aguinaldo")]
        Aguinaldo = 3,
        [XafDisplayName("Vacación")]
        Vacacion = 4,
        [XafDisplayName("Liquidación")]
        Liquidacion = 5,
        // estas ultimas dos estan en veremos, si las dejamos o las quitamos. Si se dejan hay que agregar dietas, servicios profesionales
        [XafDisplayName("Comisión")]
        Comision = 6,
        [XafDisplayName("Horas Extras")]
        HorasExtras = 7,
        [XafDisplayName("Dietas")]
        Dietas = 8


    }
}
