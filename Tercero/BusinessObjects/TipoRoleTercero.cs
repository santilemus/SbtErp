using DevExpress.ExpressApp.DC;
using System;


namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    public enum TipoRoleTercero
    {
        [XafDisplayName("Empleados")]
        Empleado = 1,
        [XafDisplayName("Bancos")]
        Banco = 2,
        [XafDisplayName("Clientes")]
        Cliente = 3,
        [XafDisplayName("Proveedores")]
        Proveedores = 4,
        [XafDisplayName("Proveedores de Servicios")]
        Servicio = 5,
        [XafDisplayName("Gobierno")]
        Gobierno = 6,
        [XafDisplayName("Sucursales")]
        Sucursal = 7,
        [XafDisplayName("Farmaceutica")]
        Fabricante = 8,
        [XafDisplayNameAttribute("Aseguradora")]
        Aseguradora = 9,
        [XafDisplayNameAttribute("Administradores de Pensiones")]
        AFP = 10,
        [XafDisplayName("Consultorio")]
        Consultorio = 11

    }
}
