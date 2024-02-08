using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    public enum ECuentaEspecial
    {
        Null = 0,
        [XafDisplayName("Capital Social")]
        CapitalSocial = 1,
        [XafDisplayName("Liquidación")]
        Liquidacion = 2,
        [XafDisplayName("Reserva Legal del Ejercicio")]
        ReservaLegalEjercicio = 3,
        [XafDisplayName("Reserva Legal Ejercicios Anteriores")]
        ReservaLegalAnterior = 4,
        [XafDisplayName("Renta a Pagar")]
        RentaPagar = 5,
        [XafDisplayName("Utilidad del Ejercicio")]
        UtilidadEjercicio = 6,
        [XafDisplayName("Pérdida del Ejercicio")]
        PerdidaEjercicio = 7,
        [XafDisplayName("Pago a Cuenta")]
        RentaPagada = 8,
        [XafDisplayName("Otros Gastos No Deducibles")]
        GastoNoDeducible = 9
    }
}