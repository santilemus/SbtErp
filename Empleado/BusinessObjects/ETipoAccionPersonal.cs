using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    /// <summary>
    /// Tipificacion de las acciones de personal
    /// </summary>
    public enum ETipoAccionPersonal
    {
        [XafDisplayName("Licencia sin Goce de Sueldo")]
        LicenciaSSueldo = 1,
        [XafDisplayName("Incapacidad")]
        Incapacidad = 2,
        [XafDisplayName("Suspensión de Contrato")]
        SuspensionContrato = 3,
        [XafDisplayName("Traslado")]
        Traslado = 4,
        [XafDisplayName("Promoción")]
        Promocion = 5,
        [XafDisplayName("Aumento de Salario")]
        AumentoSalario = 6,
        [XafDisplayName("Despido")]
        Despido = 7,
        [XafDisplayName("Renuncia")]
        Renuncia = 8,
        [XafDisplayName("Vacación")]
        Vacacion = 9,
        [XafDisplayName("Maternidad")]
        Maternidad = 10,
        [XafDisplayName("Inasistencia")]
        Inasistencia = 11,
        [XafDisplayName("Ayuda Muerte Trabajador")]
        AyudaMuerte = 12,
        [XafDisplayName("Ayuda Muerte Pariente")]
        AyudaMuertePariente = 13,
        [XafDisplayName("Licencia C/Goce Sueldo")]
        LicenciaCSueldo = 14,
        [XafDisplayName("Amonestación")]
        Amonestacion = 15,
        [XafDisplayName("Otro")]
        Otro = 16
    }
}
