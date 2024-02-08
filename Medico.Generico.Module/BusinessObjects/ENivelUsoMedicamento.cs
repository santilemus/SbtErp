using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Nivel de uso de los medicamentos para orientar su prescripción
    /// </summary>
    public enum ENivelUsoMedicamento
    {
        [XafDisplayName("Prescrito Enfermera o Tecnólogo")]
        P = 0,
        [XafDisplayName("Prescrito Medico General")]
        M = 1,
        [XafDisplayName("Prescrito Medico Especialista")]
        E = 2,
        [XafDisplayName("Prescrito Medico General en Hospitales")]
        HM = 3,
        [XafDisplayName("Prescrito Medico Especialista en Hospitales")]
        HE = 4
    }
}
