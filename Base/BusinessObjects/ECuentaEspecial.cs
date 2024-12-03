using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    /// <summary>
    /// Cuentas identificadas como especiales y que son útiles para las partidas de liquidación y cierre
    /// <br>Null = 0</br>
    /// <br>CapitalSocial = 1</br>
    /// <br>Liquidacion = 2 -Cuenta de Perdidas y Ganancias- </br>
    /// <br>ReservaLegalEjercicio = 3</br>
    /// <br>ReservaLegalAnterior = 4 -Reserva Legal de Ejercicios anteriores-</br>
    /// <br>RentaPagar = 5</br>
    /// <br>UtilidadEjercicio = 6</br>
    /// <br>PerdidaEjercicio = 7</br>
    /// <br>RentaPagada = 8 -Pago a Cuenta- </br>
    /// <br>GastoNoDeducible = 9 -Otros GAstos No Deducibles-</br>
    /// </summary>
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