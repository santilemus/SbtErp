using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con las clases de documentos de compra y venta (de acuerdo a la codificacion del MH)
    /// </summary>
    public enum EClaseDocumentoCompraVenta
    {
        [XafDisplayName("N/A")]
        NA = 0,
        [XafDisplayName("Impreso por Imprenta o Ticket")]
        Imprenta = 1,
        [XafDisplayName("Formulario Unico")]
        FormularioUnico = 2,
        /// <summary>
        /// solo para libro de compras
        /// </summary>
        [XafDisplayName("Otro")]
        Otro = 3
    }
}