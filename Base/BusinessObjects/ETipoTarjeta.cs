using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracin con los tipos de tarjeta
    /// </summary>
    public enum ETipoTarjeta
    {
        Debito = 0,
        Credito = 1,
        [XafDisplayName("ECard (Virtual)")]
        ECard = 2
    }
}