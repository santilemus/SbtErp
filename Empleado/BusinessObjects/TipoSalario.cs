using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    public enum TipoSalario
    {
        [XafDisplayName("Mensual")]
        Mensual = 0,
        [XafDisplayName("Diario")]
        Diario = 1,
        [XafDisplayName("Otro")]
        Otro = 2,
        [XafDisplayName("No Aplica")]
        NoAplica = 3
    }
}
