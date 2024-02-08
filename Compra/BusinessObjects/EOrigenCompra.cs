using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    public enum EOrigenCompra
    {
        Local = 0,
        [XafDisplayName("Importación")]
        Importacion = 1,
        [XafDisplayName("Internación")]
        Internacion = 2,
        Excluido = 3
    }
}