using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    /// <summary>
    /// Enumeracion con los tipos de cuenta contable
    /// </summary>
    public enum ETipoCuentaCatalogo
    {
        [XafDisplayName("Activo")]
        Activo = 1,
        [XafDisplayName("Pasivo")]
        Pasivo = 2,
        [XafDisplayName("Patrimonio")]
        Capital = 3,
        [XafDisplayName("Costos")]
        Ingreso = 4,
        [XafDisplayName("Gastos")]
        Gasto = 5,
        [XafDisplayName("Producto")]
        Costo = 6,
        [XafDisplayName("Resultados")]
        Resultado = 7,
        [XafDisplayName("Orden")]
        Orden = 8
    }
}
