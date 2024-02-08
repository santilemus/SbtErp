using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum CategoriaLista
    {
        // <summary>
        // Activo Fijo. Estado de Uso de los Activos. Forma de Codificacion: UA001, UA002...UA0nn.
        // </summary>
        [XafDisplayName("Estado Uso Activo")]
        EstadoUsoActivo = 1,

        /// <summary>
        /// Tipos de sangre. Manera de codificarlos:  GSAXXXXXXXX. Ejemplos: GSA01
        /// </summary>
        [XafDisplayName("Grupo Sanguineo")]
        GrupoSanguineo = 2,

        ///<summary>
        /// Tipos de Garantía válida que puede presentar un tercero que es cliente al recibir credito
        ///</summary>
        [XafDisplayName("Tipo Garantía")]
        [ToolTip("Tipos de Garantía válidos, cuando se vincula a un tercero que es cliente")]
        TipoGarantia = 3,

        /// <summary>
        /// Tipos de seguros. Manera de codificarlos. SEGXXXXXXXX. Ejemplo: SEG001 --> Medico Hospitalario, SEG002 --> Seguro Medico colectivo, etc
        /// Para dejar solo la parte del correlativo por si necesitamos trabajar con el -->  Regex.Replace("SEG001", @"[^0-9]+", "");
        /// </summary>
        [XafDisplayName("Tipos de Seguro")]
        TipoSeguro = 4,

        ///// <summary>
        ///// Formas de Pago. Manera de codificarlas Ejemplo: FPA01 --> Efectivo, FPA02 --> Cheque
        ///// </summary>
        ///// <remarks>
        ///// FUERA DE USO, porque las sentencias Select de las transacciones se vuelven muy complejas cuando se usa
        ///// frecuentemente Listas por ejemplo para el BO Ventas. Se reemplazo por enum ECondicionPago
        ///// </remarks>
        //[XafDisplayName("Forma Pago")]
        //FormaPago = 5,

        /// <summary>
        /// Tipos de Tarjetas. Forma de codificarlas: TTAXXXXXXXX. Ejemplo: TTA01, TTA02
        /// </summary>
        [XafDisplayName("Tipo de Tarjeta")]
        TipoTarjeta = 6,

        /// <summary>
        /// Líneas de los productos. Forma de codificarlas: LPRXXXXX. Ejemplos: LPR00001 --> Combustibles, LPR00002 --> Papelería, LPRNNNNN
        /// </summary>
        [XafDisplayName("Linea de Producto")]
        LineaProducto = 7,

        /// <summary>
        /// Catálogo de las marcas. 
        /// La manera de codificarlas: siglas de la marca. HP, LENOVO, SAMSUNG, etc
        /// </summary>
        [XafDisplayName("Marcas")]
        MarcasProducto = 8,

        /// <summary>
        /// Tipificación de los estados de los empleados.
        /// La manera de codificarlos: EMPLXXXXXXXX. Ejemplo: EMPL01 --> Contrato Activo, EMPL02 --> Permiso S/Sueldo, EMPL03 --> Retirado
        /// Para dejar solo la parte del correlativo por si necesitamos trabajar con el -->  Regex.Replace("SEG001", @"[^0-9]+", "");
        /// </summary>
        [XafDisplayName("Estado Empleado")]
        EstadoEmpleado = 9,

        /// <summary>
        /// Tipos de documentos de identidad.
        /// Forma de codificarlos.  DID-XXXXXXXX. Ejemplo: DUI, NIT, PAS = Pasaporte, RES = Carne de Residente, etc.
        /// </summary>
        [XafDisplayName("Documentos de Identidad")]
        DocumentoIdentidad = 10,

        /// <summary>
        /// Catálogos de las capacitaciones. 
        /// La manera de cOdificarlos: CAP-XXXXXXXX. Ejemplo: CAP-01, CAP-0101, CAP-0102, CAP-0103, CAP010101, CAP-02, CAP-0201, etc.
        /// </summary>
        [XafDisplayName("Capacitaciones")]
        Capacitacion = 11,

        /// <summary>
        /// La tipificación de los parientes. 
        /// La manera de codificarlos: PAR-XXXXXXXX. Ejemplo: PAR01, PAR02, PAR03
        /// </summary>
        [XafDisplayName("Parentesco")]
        TipoPariente = 12,

        /// <summary>
        /// Definición de los tipos de atributos de un producto
        /// La manera de codificarlos: ATRXXXXXXXX. Ejemplo: ATR001 = Color, ATR0002 = Modelo, ATR0003 = Ancho, ATR0004 = Largo, ATR0005 = Peso, etc.
        /// </summary>
        [XafDisplayName("Atributo Producto")]
        AtributoProducto = 13,

        /// <summary>
        /// Clasificación de los precios de acuerdo con las políticas de la empresa
        /// Ejemplo: Precio Mayoreo, Precio Detalle, Precio de Temporada, etc.
        /// La manera de codificarlos: TPRXXXXXXXX. Ejemplo: TPR001 = Detalle, TPR002 = Mayoreo, TPR003 = Promocion
        /// </summary>
        [XafDisplayName("Tipos de Precio")]
        TipoPrecio = 14,
        /// <summary>
        ///  Los tipos de factura para transacciones de compra y venta que se pueden manejar en el sistema de acuerdo a las normativas legales vigentes
        ///  Ejemplo: En El Salvador, Crédito Fiscal, Consumidor Final, Exportación
        ///  La manera de codificarlos es: COVEXXXXXXXX. Ejemplo: COVE01 => Credito Fiscal, COVE02 => Consumidor Final, COVE03 => Ticket de Venta, COVE04, etc.
        /// </summary>
        [XafDisplayName("Documentos de Compra Venta")]
        CompraVenta = 15,

        /// <summary>
        ///  Los tipos de documentos aplicados a las compras y ventas que se pueden manejar en el sistema de acuerdo a las normativas legales vigentes
        ///  Ejemplo: En El Salvador, Nota de Debito, Nota de Credito, Ticket de Devolucion
        ///  La manera de codificarlos es: DACVXXXXXXXX. Ejemplo: DACV01 => Nota Debito, DACV02 => Nota Credito, DACV03 => Ticket de Devolucion.
        /// </summary>
        [XafDisplayName("Aplicados a Compra Venta")]
        AplicadosCompraVenta = 16,

        ///// <summary>
        /////  La condición de pago de una compra o venta
        /////  Ejemplo: Crédito, Contado, etc
        /////  La manera de codificarlos es: CPAXXXXXXXX. Ejemplo: CPA01 => Contado, CPA02 => Credito, etc.
        ///// </summary>
        ///// <remarks>
        ///// FUERA DE USO, porque las sentencias Select de las transacciones se vuelven muy complejas cuando se usa
        ///// frecuentemente Listas por ejemplo para el BO Ventas. Se reemplazo por enum ECondicionPago
        ///// </remarks>
        //[XafDisplayName("Condición Pago")]
        //CondicionPago = 17,

        /// <summary>
        /// Codificacion de las transacciones de ingreso que pueden aplicarse al empleado y que son gravadas o sujetas de calculo de aportes sociales
        /// y renta. Ejemplo: RHTI01 = Comisiones, RHTI02 = Horas Extras, RHTIXX = nombre_transaccion
        /// </summary>
        [XafDisplayName("RRHH - Transacciones Ingreso Gravadas")]
        PlaTransacIngresoGravada = 18,
        /// <summary>
        /// Codificacion de las transacciones de ingreso que pueden aplicarse al empleado que NO SON GRAVADAS, es decir; no afectan el calculo de aportes
        /// sociales o renta; pero afectan el devengado neto a pagar al empleado. Ejemplo: RHTE01 = Viaticos, RHTE02 = Combustible,
        /// RHTE03 = Depreciación Vehiculo, RHTE04 = Subsidio Alimentacion, RHTEXX = nombre_transaccion
        /// </summary>
        [XafDisplayName("RRHH - Transacciones Ingreso No Gravadas")]
        PlaTransacIngresoNoGravada = 19,
        /// <summary>
        /// Codificacion de las transacciones de descuento que pueden aplicarse al empleado en el calculo de planillas y que son
        /// descuentos distintos a los de ley. Ejemplos: RHTD01 = Ordenes de Descuento, RHTD02 = Embargos, RHTD03 = Donacion,
        /// RHTDXX = nombre_transaccion
        /// </summary>
        [XafDisplayName("RRHH - Transacciones de Descuentos")]
        PlaTransacDescuento = 20,
        /// <summary>
        /// Codificacion de los tipos de jornada laboral que pueden usarse en el modulo de RRHH. 
        /// Ejemplos: RHEX01 = Diurna, RHEX02 = Nocturna, RHEX03 = Diurna Dia Descanso, etc.
        /// </summary>
        [XafDisplayName("RRHH - Tipo Jornada")]
        PlaTipoJornada = 21
    }
}
