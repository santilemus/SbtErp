using System;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con la clasificacion del tipo de contribuyente. Esta clasificacion se utiliza para los Terceros
    /// 
    /// </summary>
    public enum TipoContribuyente
    {
        Gravado = 0,
        Exento = 1,
        Excluido = 2
        //[XafDisplayName("Exportación")]
        //Exportacion = 3
    }
}
