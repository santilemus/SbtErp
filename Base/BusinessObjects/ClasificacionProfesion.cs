using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum ClasificacionProfesion
    {
        [XafDisplayName("Profesión")]
        Profesion = 1,
        [XafDisplayName("PostGrado")]
        PostGrado = 2,
        [XafDisplayName("Especialización")]
        Especializacion = 3,
        [XafDisplayName("Oficio o Arte")]
        OficionArte = 4,
        [XafDisplayName("Otra")]
        Otra = 5
    }
}
