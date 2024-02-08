using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum TipoConstante
    {
        [XafDisplayName("Número Entero")]
        Entero = 1,
        [XafDisplayName("Cadena de Texto")]
        Texto = 2,
        [XafDisplayName("Doble Precisión (Decimales)")]
        Doble = 3,
        [XafDisplayName("Fecha y Hora")]
        FechaHora = 4,
        [XafDisplayName("Lógico")]
        Logico = 5
    }
}
