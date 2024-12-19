using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Tipo de Sistema de Unidad de Medida.
    /// <br>Internacional = 1</br>
    /// <br>AngloSajon = 2</br>
    /// <br>Otro = 3</br>
    /// </summary>
    public enum TipoSistemaMedida
    {
        [XafDisplayName("Internacional")]
        Internacional = 1,
        [XafDisplayName("AngloSajón")]
        AngloSajon = 2,
        Otro = 3
    }
}
