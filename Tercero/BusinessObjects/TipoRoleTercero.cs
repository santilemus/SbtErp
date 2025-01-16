using DevExpress.ExpressApp.DC;


namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// Enumeración con los tipos de roles de terceros
    /// <br>Empleado = 1</br>
    /// <br>Banco = 2</br>
    /// <br>Cliente = 3</br>
    /// <br>Proveedor = 4</br>
    /// <br>Servicio = 5</br>
    /// <br>Gobierno = 6</br>
    /// <br>Sucursal = 7</br>
    /// <br>Fabricante = 8</br>
    /// <br>Aseguradora = 9</br>
    /// <br>AFP = 10</br>
    /// <br>Consultorio = 11</br>
    /// </summary>
    public enum TipoRoleTercero
    {
        [XafDisplayName("Empleado")]
        Empleado = 1,
        [XafDisplayName("Banco")]
        Banco = 2,
        [XafDisplayName("Cliente")]
        Cliente = 3,
        [XafDisplayName("Proveedor")]
        Proveedor = 4,
        [XafDisplayName("Proveedor de Servicio")]
        Servicio = 5,
        [XafDisplayName("Gobierno")]
        Gobierno = 6,
        [XafDisplayName("Sucursal")]
        Sucursal = 7,
        [XafDisplayName("Fabricante")]
        Fabricante = 8,
        [XafDisplayNameAttribute("Aseguradora")]
        Aseguradora = 9,
        [XafDisplayNameAttribute("AFP")]
        AFP = 10,
        [XafDisplayName("Consultorio")]
        Consultorio = 11

    }
}
