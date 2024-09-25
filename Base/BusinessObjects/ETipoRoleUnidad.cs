using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Tipos de unidades de la empresa, se utiliza en el BO <b>EmpresaUnidad</b> Valores posibles son:
    /// <br>Departamento = 1</br>
    /// <br>Agencia = 2</br>
    /// <br>Bodega = 3</br>
    /// <br>UnidadExterna = 4</br>
    /// </summary>
    public enum ETipoRoleUnidad
    {
        [XafDisplayName("Departamento o Sección")]
        Departamento = 1,
        [XafDisplayName("Casa Matriz o Agencia")]
        Agencia = 2,
        [XafDisplayName("Bodega")]
        Bodega = 3,
        [XafDisplayName("Unidad Externa")]
        UnidadExterna = 4
    }
}
