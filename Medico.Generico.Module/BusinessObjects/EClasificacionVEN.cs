using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Clasificar la prioridad de los medicamentes: Vitales, Esenciales y No Esenciales (VEN), 
    /// </summary>
    public enum EClasificacionVEN
    {
        [XafDisplayName("No Esenciales")]
        NoEsenciales = 0,
        [XafDisplayName("Esenciales")]
        Esenciales = 1,
        [XafDisplayName("Vitales")]
        Vitales = 2
    }
}
