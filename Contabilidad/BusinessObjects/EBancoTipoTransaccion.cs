using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. Clasificacion Basica de las transacciones bancarias 
    /// </summary>
    public enum EBancoTipoTransaccion
    {
        [XafDisplayName("Nota de Abono")]
        Abono = 1,
        Remesa = 2,
        Cheque = 3,
        [XafDisplayName("Nota de Cargo")]
        Cargo = 4
    }
}