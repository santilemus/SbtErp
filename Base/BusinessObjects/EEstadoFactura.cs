using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con los Estados de los documentos de compra o Venta (Comprobantes de Credito Fiscal, Facturas, etc)
    /// <br>Debe = 0</br>
    /// <br>Pagado = 1</br>
    /// <br>Anulado = 2</br>
    /// <br>Devolucion = 3</br>
    /// </summary>
    public enum EEstadoFactura
    {
        [XafDisplayName("Debe")]
        Debe = 0,
        [XafDisplayName("Pagado")]
        Pagado = 1,
        [XafDisplayName("Anulado")]
        Anulado = 2,
        [XafDisplayName("Devolución")]
        Devolucion = 3
    }
}