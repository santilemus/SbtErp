using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum ETipoOperacion
    {
        Nulo = 0,
        [XafDisplayName("Cargo")]
        Cargo = 1,
        [XafDisplayName("Abono")]
        Abono = 2,
    }
}