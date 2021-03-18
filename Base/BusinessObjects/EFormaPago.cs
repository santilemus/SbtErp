using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con las formas de pago de una transaccion de compra o venta
    /// </summary>
    public enum EFormaPago
    {
        Efectivo = 0,
        Cheque = 1,
        [XafDisplayName("Tarjeta Crédito o Débito")]
        Tarjeta = 2,
        Transferencia = 3,
        Otro = 4
    }
}