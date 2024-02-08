using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum EstadoCivil
    {
        //[XafDisplayName("NInguno")]
        Ninguno = 0,
        [XafDisplayName("Solter@")]
        Soltero = 1,
        [XafDisplayName("Casad@")]
        Casado = 2,
        [XafDisplayName("Acompañad@")]
        Acompanado = 3,
        [XafDisplayName("Viud@")]
        Viudo = 4,
        [XafDisplayName("Divorciad@")]
        Divorciado = 5,
        [XafDisplayName("No Aplica")]
        Noaplica = 6
    }
}
