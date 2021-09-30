using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Banco.Module.BusinessObjects;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;


namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// Cuenta por Cobrar.
    /// BO que corresponde a las transacciones de cuentas por cobrar. Es el encabezado y es generico para cualquier tipo
    /// de documento. Pueden ser notas de credito, debito, abonos, cheques rechazados. En resumen cualquiera para el cual
    /// existe una tipifiacion o concepto
    /// </summary>
    /// <remarks>
    /// 1. Faltan los siguientes properties: fecha del cheque,
    ///                                      moneda.  (evaluar que otros datos harian falta)
    ///                                  
    /// </remarks>

    [DefaultClassOptions, ModelDefault("Caption", "Transacción CxC"), NavigationItem("Cuenta por Cobrar")]
    [CreatableItem(false), Persistent(nameof(CxCTransaccion)), DefaultProperty("Numero")]
    [ImageName(nameof(CxCTransaccion))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxCTransaccion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxCTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            autorizacionDocumento = null;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        protected decimal monto;
        Venta venta;
        decimal valorMoneda;
        Moneda moneda;
        int? numero;
        Cartera cartera;
        Listas tipoTarjeta;
        BancoTransaccion bancoTransaccion;
        string noTarjeta;
        string referencia;
        ECxCTransaccionEstado estado;
        SBT.Apps.Tercero.Module.BusinessObjects.Banco banco;  // revisar si se borra
        string comentario;
        DateTime fecha;
        CxCTipoTransaccion tipo;
        [Persistent(nameof(AutorizacionDocumento))]
        AutorizacionDocumento autorizacionDocumento;
        string usuarioAnulo;
        DateTime fechaAnula;


        [DbType("datetime2"), XafDisplayName("Fecha"), Index(1)]
        [RuleValueComparison("CxCTransaccion.Fecha > Fecha Factura", DefaultContexts.Save,
            ValueComparisonType.GreaterThanOrEqual, "[Venta.Fecha]", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        /// <summary>
        /// Tipo de concepto o de transaccion de cuenta por cobrar
        /// </summary>
        //[Association("TipoTransaccion-Transacciones"), XafDisplayName("Tipo Concepto")]
        [RuleRequiredField("CxcTransaccion.Tipo_Requerido", DefaultContexts.Save)]
        [Index(2), VisibleInLookupListView(true)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [DataSourceCriteria("!IsNull([Padre]) && [Activo] == True")]
        public CxCTipoTransaccion Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        /// <summary>
        /// Algunos tipos de concepto van a requerir una autorizacion de correlativos, habra que identificar en el BO
        /// CxCTransaccion el tipo de documento o vincularlo al concepto, para que el sistema pueda obener la autorizacion y validar
        /// que sea la correcta para el tipo de documento, ademas este vigente y el correlativo generado este dentro del 
        /// rango
        /// </summary>
        [XafDisplayName("Autorización Documento"), PersistentAlias(nameof(autorizacionDocumento)), Index(2)]
        [VisibleInListView(false)]
        public AutorizacionDocumento AutorizacionDocumento => autorizacionDocumento;


        [DbType("int"), XafDisplayName("Número"), Index(3)]
        [ModelDefault("AllowEdit", "False")]
        [ToolTip("Numero Correlativo por tipo de documento y empresa")]
        public int? Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        [XafDisplayName("Moneda"), Index(4)]
        [DataSourceCriteria("[Activa] == True")]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [DbType("numeric(12,2)"), XafDisplayName("Valor Moneda"), Index(5)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [ModelDefault("AllowEdit", "False")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [Association("Venta-CxCTransacciones")]
        [XafDisplayName("Venta"), Index(6)]
        public Venta Venta
        {
            get => venta;
            set => SetPropertyValue(nameof(Venta), ref venta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Monto"), Index(7)]
        [RuleValueComparison("CxCTransaccion.Monto > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        /// <summary>
        /// Cartera de Cuenta por Cobrar o Venta. El vendedor o el cobrador esta relacionado con la cartera y el cliente
        /// a CarteraCliente
        /// </summary>
        [Association("Cartera-CxCTransacciones"), XafDisplayName("Cartera"), Index(3)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public Cartera Cartera
        {
            get => cartera;
            set => SetPropertyValue(nameof(Cartera), ref cartera, value);
        }


        [DbType("varchar(12)"), XafDisplayName("Tipo Tarjeta"), Index(6), VisibleInListView(false)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        [DataSourceCriteria("[Categoria] == 6 And [Activo] == True")]   // categoria 6 son tarjetas de credito
        [RuleRequiredField("CxCTransaccion.TipoTarjeta_Requerido", "Save", TargetCriteria = "[FormaPago.Codigo] In ('FPA03', 'FPA04')",
             ResultType = ValidationResultType.Warning)]

        public Listas TipoTarjeta
        {
            get => tipoTarjeta;
            set => SetPropertyValue(nameof(TipoTarjeta), ref tipoTarjeta, value);
        }

        [XafDisplayName("Banco"), Index(6)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Banco Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }

        /// <summary>
        /// No de tarjeta de credito o debito
        /// </summary>
        [Size(20), DbType("varchar(20)"), XafDisplayName("No Tarjeta"), ToolTip("No de Tarjeta de debito o credito, cuando es el medio de pago")]
        [Index(7), VisibleInListView(false)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string NoTarjeta
        {
            get => noTarjeta;
            set => SetPropertyValue(nameof(NoTarjeta), ref noTarjeta, value);
        }

        /// <summary>
        ///  No de cheque, No de pago electronico, Id de la remesa, transferencia, no vaucher etc.
        /// </summary>
        [Size(40), DbType("varchar(40)"), XafDisplayName("No Referencia"), Index(8), VisibleInListView(false)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string Referencia
        {
            get => referencia;
            set => SetPropertyValue(nameof(Referencia), ref referencia, value);
        }

        /// <summary>
        /// La trasaccion de bancos cuando es: transferencia bancaria, remesa a cuenta, pago electronico.
        /// En los tres casos anteriores el movimiento de bancos es previo al registro del pago de la cuenta por cobrar
        /// Cuando son pagos en efectivo o cheques, evaluar si puede ser la remesa del deposito a cuenta de la empresa
        /// </summary>
        /// <remarks>
        /// Agregar validacion para que este dato sea requerido unicamente en los casos de transferencia, remesa o pago electronico
        /// Agregar regla de apariencia, para que en los casos donde no es necesario este dato, aparezca deshabilitado u ocultarlo
        /// </remarks>
        [XafDisplayName("Banco Transacción"), Index(9), VisibleInListView(false),
            ToolTip("La transacción de bancos, cuando es transferencia, remesa, o pago electronico")]
        //[Association("BancoTransaccion-CxCTransacciones")]
        [DataSourceCriteria("[BancoCuenta.Empresa] == '@This.Venta.Empresa' && [Clasificacion.Tipo] In (1, 2)")]   // clasificacion.Tipo in (Abono, Remesa)
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public BancoTransaccion BancoTransaccion
        {
            get => bancoTransaccion;
            set => SetPropertyValue(nameof(BancoTransaccion), ref bancoTransaccion, value);
        }

        [DbType("smallint"), XafDisplayName("Estado"), Index(15), RuleRequiredField("CxCTransaccion.Estado_Requerido", "Save")]
        [DetailViewLayout("Otros Datos", LayoutGroupType.SimpleEditorsGroup, 2)]
        public ECxCTransaccionEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario"), Index(16), VisibleInListView(false)]
        [DetailViewLayout("Otros Datos", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [XafDisplayName("Fecha Anulación")]
        [ModelDefault("AllowEdit", "False"), Index(98)]
        public DateTime FechaAnula
        {
            get => fechaAnula;
            set => SetPropertyValue(nameof(FechaAnula), ref fechaAnula, value);
        }
        [Size(25), XafDisplayName("Usuario Anulo")]
        [ModelDefault("AllowEdit", "False"), Index(99)]
        public string UsuarioAnulo
        {
            get => usuarioAnulo;
            set => SetPropertyValue(nameof(UsuarioAnulo), ref usuarioAnulo, value);
        }

        #endregion

        #region Collecciones
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}