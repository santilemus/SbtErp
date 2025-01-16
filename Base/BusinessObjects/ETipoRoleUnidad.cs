using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Tipos de unidades de la empresa, se utiliza en el BO <b>EmpresaUnidad</b> Valores posibles son:
    /// <br>Agencia = 1</br>
    /// <br>CasaMatriz = 2</br>
    /// <br>Departamento = 3</br>
    /// <br>Bodega = 4</br>
    /// <br>Predio = 7</br>
    /// <br>Otro = 20</br>
    /// <br>
    /// Para <b>Dte's, Departamento no es válido</b>
    /// </br>
    /// </summary>
    public enum ETipoRoleUnidad
    {
        [XafDisplayName("Agencia")]
        Agencia = 1,
        [XafDisplayName("Casa Matriz")]
        CasaMatriz = 2,
        [XafDisplayName("Departamento")]
        Departamento = 3,
        [XafDisplayName("Bodega")]
        Bodega = 4,
        [XafDisplayName("Predio y/o Patio")]
        Predio = 7,
        [XafDisplayName("Otro")]
        Otro = 20
    }
}
