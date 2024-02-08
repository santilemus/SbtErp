using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    public enum ETipoCalculoOperacion
    {
        [XafDisplayName("Primera Quincena")]
        PrimeraQuincena = 0,
        [XafDisplayName("Segunda Quincena")]
        SegundaQuincena = 1,
        [XafDisplayName("Siempre")]
        Siempre = 2
    }
}
