using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. Clasificacion Basica de las transacciones bancarias
    /// </summary>
    public enum EBancoTipoTransaccion
    {
        Abono = 1,
        Remesa = 2,
        Cheque = 3,
        Cargo = 4
    }
}