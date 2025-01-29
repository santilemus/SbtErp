using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. Clasificacion Basica de las transacciones bancarias 
    /// <br>Abono  = 1</br>
    /// <br>Remesa = 2</br>
    /// <br>Cheque = 3</br>
    /// <br>Cargo  = 4</br>
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