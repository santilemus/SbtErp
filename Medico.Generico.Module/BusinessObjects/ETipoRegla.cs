using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Para clasificacion de las reglas
    /// </summary>
    public enum ETipoRegla
    {
        [XafDisplayName("Recordatorio Paciente")]
        Recordatorio = 0,
        [XafDisplayName("Alerta Activa")]
        AlertaActiva = 1,
        [XafDisplayName("Alerta Pasiva")]
        AlertaPasiva = 2,
        [XafDisplayName("otra")]
        Otra = 3
    }
}
