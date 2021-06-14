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
