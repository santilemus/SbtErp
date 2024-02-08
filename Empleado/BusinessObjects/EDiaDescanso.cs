using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano. Enumeracion con los dias de la semana para indicar el dia de descanso
    /// </summary>
    public enum EDiaDescanso
    {
        Domingo = 0,
        Lunes = 1,
        Martes = 2,
        [XafDisplayName("Miércoles")]
        Miercoles = 3,
        Jueves = 4,
        Viernes = 5,
        [XafDisplayName("Sábado")]
        Sabado = 7
    }
}
