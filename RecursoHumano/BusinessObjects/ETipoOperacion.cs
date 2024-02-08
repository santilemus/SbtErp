using DevExpress.ExpressApp.DC;


namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano
    /// Enumeracion con los tipos de operacion para planillas
    /// </summary>
    /// <remarks>
    /// El 22/08/2020 se incorpora IngresoSubTotal y DescuentoSubTotal porque es necesario ordenar la operaciones
    /// por tipo cuando se calculan y casos como ingreso bruto no puede ser de tipo Null (0), porque simplemente se
    /// debe calcular cuando ya se tienen los datos de Ingresos calculados
    /// </remarks>
    public enum ETipoOperacion
    {
        Null = 0,
        Ingreso = 1,
        [XafDisplayName("Ingreso SubTotal")]
        IngresoSubTotal = 2,
        Descuento = 3,
        [XafDisplayName("Descuento SubTotal")]
        DescuentoSubTotal = 4,
        Resultado = 5
    }
}
