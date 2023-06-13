using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Clase de documento (de compra o venta). Requerido para los libros del IVA
    /// </summary>
    public enum EClaseDocumento
    {
        [XafDisplayName("Nulo")]
        Null = 0,
        [XafDisplayName("Impreso por Imprenta")]
        Imprenta = 1,
        [XafDisplayName("Formulario Único")]
        FormularioUnico = 2,
        Otro = 3,
        [XafDisplayName("Dte")]
        Dte = 4
    }
}
