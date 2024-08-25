using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;


namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con las clases de tributos. Los valores válidos son:
    /// <br>ResumenDte = 0</br>
    /// <br>CuerpoDte = 1</br>
    /// <br>AdValorem = 2</br>
    /// </summary>
    public enum EClaseTributo
    {
        [XafDisplayName("Resumen Dte")]
        [ToolTip("1 - Tributos aplicados por ítems reflejados en el resumen Dte")]
        ResumenDte = 0,
        [XafDisplayName("Cuerpo Dte")]
        [ToolTip("2 - Tributos aplicados por ítems reflejados en el cuerpo del dte")]
        CuerpoDte = 1,
        [XafDisplayName("AdValorem")]
        [ToolTip("3 - Impuestos ad-valorem aplicados por ítem de uso informativo. Reflejados en resumen del Documento")]
        AdValorem = 2
    }
}