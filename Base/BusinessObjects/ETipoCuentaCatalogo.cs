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
        [XafDisplayName("Capital")]
        Capital = 3,
        [XafDisplayName("Ingreso")]
        Ingreso = 4,
        [XafDisplayName("Gasto")]
        Gasto = 5,
        [XafDisplayName("Costo")]
        Costo = 6,
        [XafDisplayName("Orden")]
        Orden = 7
    }
}
