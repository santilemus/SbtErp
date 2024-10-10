using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    /// <summary>
    /// Tipos de cuenta contable. Valores válidos son:
    /// <br>Activo = 1</br>
    /// <br>Pasivo = 2</br>
    /// <br>Patrimonio = 3</br>
    /// <br>Costo = 4</br>
    /// <br>Gasto = 5</br>
    /// <br>Ingreso = 6</br>
    /// <br>Resultado = 7</br>
    /// <br>Orden = 8</br>
    /// </summary>
    public enum ETipoCuentaCatalogo
    {
        [XafDisplayName("Activo")]
        Activo = 1,
        [XafDisplayName("Pasivo")]
        Pasivo = 2,
        [XafDisplayName("Patrimonio")]
        Patrimonio = 3,
        [XafDisplayName("Costo")]
        Costo = 4,
        [XafDisplayName("Gasto")]
        Gasto = 5,
        [XafDisplayName("Ingreso")]
        Ingreso = 6,
        [XafDisplayName("Resultado")]
        Resultado = 7,
        [XafDisplayName("Orden")]
        Orden = 8
    }
}
