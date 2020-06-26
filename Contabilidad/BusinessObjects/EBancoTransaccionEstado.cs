using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. Enumeracion con los estados de las transacciones de bancos. Se incluye el impreso para los cheques, para 
    /// evitar un campo separado
    /// </summary>
    public enum EBancoTransaccionEstado
    {
        Digitado = 0,
        Impreso = 1,
        Entregado = 2,
        Anulado = 3
    }
}
