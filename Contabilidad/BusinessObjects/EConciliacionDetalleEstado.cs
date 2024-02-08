using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    public enum EConciliacionDetalleEstado
    {
        [XafDisplayName("Aplicado y Contabilizado")]
        AplicadoYContabilizado = 0,
        [XafDisplayName("Cargo Contabilizado No Aplicado por Banco")]
        CargoContabilizadoNoABanco = 1,
        [XafDisplayName("Cargo Aplicado por el Banco No Contabilizado")]
        CargoABancoNoContabilizado = 2,
        [XafDisplayName("Abono Contabilizado No Aplicado por Banco")]
        AbonoContabilizadoNoABanco = 3,
        [XafDisplayName("Abono Aplicado por el Banco No Contabilizado")]
        AbonoABancoNoContabilizado = 4
    }
}