using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Nivel de uso de los medicamentos para orientar su prescripción
    /// </summary>
    public enum ENivelUsoMedicamento
    {
        [XafDisplayName("Prescrito por Enfermera o Tecnólogo")]
        P = 0,
        [XafDisplayName("Prescrito por Médico General")]
        M = 1,
        [XafDisplayName("Prescrito por Médico Especialista")]
        E = 2,
        [XafDisplayName("Prescrito por Médico General en Hospitales")]
        HM = 3,
        [XafDisplayName("Prescrito por Médico Especialista en Hospitales")]
        HE = 4
    }
}
