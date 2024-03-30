using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Tipo de Operación: Null = 0, Cargo = 1, Abono = 2
    /// </summary>
    public enum ETipoOperacion
    {
        Nulo = 0,
        [XafDisplayName("Cargo")]
        Cargo = 1,
        [XafDisplayName("Abono")]
        Abono = 2,
    }
}