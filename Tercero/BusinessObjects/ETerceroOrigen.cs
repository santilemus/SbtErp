using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    public enum ETerceroOrigen
    {
        [XafDisplayName("Nacional")]
        Nacional = 0,
        [XafDisplayName("Domiciliado")]
        [ToolTip("Extranjero con domicilio local")]
        Domiciliado = 1,
        [ToolTip("Extranjero sin domicilio local")]
        [XafDisplayName("No Domiciliado")]
        NoDomiciliado = 2,
        [ToolTip("Domicilio en paraiso fiscal")]
        [XafDisplayName("Paraiso Fiscal")]
        ParaisoFiscal = 3
    }
}
