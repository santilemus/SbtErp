using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con los estados posibles de los estilos de vida. Se utiliza en la propiedad Estado del BO EstiloVida
    /// </summary>
    public enum EEstadoEstiloVida
    {
        [XafDisplayName("Si")]
        Si = 0,
        [XafDisplayName("Abandonar")]
        Abandonar = 1,
        [XafDisplayName("No")]
        No = 2,
        [XafDisplayName("No Aplica")]
        NoAplica = 3
    }
}
