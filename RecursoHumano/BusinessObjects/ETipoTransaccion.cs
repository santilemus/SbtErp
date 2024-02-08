using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    public enum ETipoTransaccion
    {
        [XafDisplayName("Ingreso Gravado")]
        IngresoGravado = 1,
        [XafDisplayName("Ingreso No Gravado")]
        IngresoExento = 2,
        [XafDisplayName("Descuento")]
        Descuento = 3
    }
}
