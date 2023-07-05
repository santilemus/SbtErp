using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con los tipos de operacion en un detalle de partida, el cual indica si se debe considerar la 
    /// el objeto (registro) en la generacion de la consolidacion. Si se va a incluir, entonces como debe operararse,
    /// independiente de la naturaleza del asiento contable (normalmente va a ser la contrario)
    /// </summary> 
    public enum ETipoOperacionConsolidacion
    {
        [XafDisplayName("Ninguno")]
        Ninguno = 0,
        [XafDisplayName("Cargo")]
        Cargo = 1,
        [XafDisplayName("Abono")]
        Abono = 2
    }
}
