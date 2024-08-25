using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con las formas de pago de una transaccion de compra o venta
    /// <br>Efectivo = 0</br>
    /// <br>Cheque = 1</br>
    /// <br>Tarjeta = 2</br>
    /// <br>Transferencia = 3</br>
    /// <br>Otro = 4</br> 
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