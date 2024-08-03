using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Microsoft.CodeAnalysis.Operations;
using SBT.Apps.Banco.Module.BusinessObjects;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using System;
using System.ComponentModel;


namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// Cuenta por Cobrar.
    /// BO que corresponde a las transacciones de cuentas por cobrar. Es el encabezado y es generico para cualquier tipo
    /// de documento. Pueden ser notas de credito, debito, abonos, cheques rechazados. En resumen cualquiera para el cual
    /// existe una tipifiacion o concepto
    /// </summary>
    /// <remarks>
    /// 1. Faltan los siguientes properties: fecha del cheque, moneda.  (evaluar que otros datos harian falta)
    ///                                  
    /// </remarks>

    [DefaultClassOptions, ModelDefault("Caption", "Cuenta por Cobrar"), NavigationItem(false)]
    [CreatableItem(false), Persistent(nameof(CxCTransaccion)), DefaultProperty("Numero")]
    [ImageName(nameof(CxCTransaccion))]
    [RuleCriteria("CxCTransaccion Nota Credito o Debito valida solo cuando es Credito Fiscal", DefaultContexts.Save, 
        "[Tipo.Padre.Oid] in (1, 16)", TargetCriteria = "[Venta.TipoFactura.Codigo] = 'COVE01' && [Venta.Saldo] > 0.0 && [Venta.Estado] == 'Debe'")]
    //[RuleObjectExists(@"CxCTransaccion.AutorizacionDocumento debe existir", DefaultContexts.Save, "[Oid] == '@This.AutorizacionDocumento.Oid'",
    //        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false,
    //        LooksFor = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.AutorizacionDocumento), 
    //    TargetCriteria = @"[Tipo.Padre.Oid] in (1, 16)")]

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
            Estado = ECxCTransaccionEstado.Digitado;
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
        private string numeroDocumento;
        private XPCollection<CxCTipoTransaccion> tipoTransaccionesCxCValidas;

        /// <summary>
        /// Tipo de concepto o de transaccion de cuenta por cobrar
        /// </summary>
        //[Association("TipoTransaccion-Transacciones"), XafDisplayName("Tipo Concepto")]
        [RuleRequiredField("CxCTransaccion.Tipo_Requerido", DefaultContexts.Save)]
        [Index(0), VisibleInLookupListView(true)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        //[DataSourceCriteria("!IsNull([Padre]) && [Activo] == True")]
        [DataSourceProperty(nameof(TipoTransaccionesCxCValidas))]
        [ImmediatePostData(true)]
        public CxCTipoTransaccion Tipo
        {
            get => tipo;
            set
            {
                bool changed = SetPropertyValue(nameof(Tipo), ref tipo, value);
                if (!IsLoading && !IsSaving && Tipo != null && changed)
                {
                    if (Tipo.Padre.Oid == 1 || Tipo.Padre.Oid == 16)
                    {
                        CriteriaOperator criteria = CriteriaOperator.FromLambda<SBT.Apps.Facturacion.Module.BusinessObjects.AutorizacionDocumento>(
                            x => x.Tipo.Codigo == "DACV02" && x.Activo == true && x.Agencia.Oid == Venta.Agencia.Oid);
                        var resolucion = Session.FindObject<SBT.Apps.Facturacion.Module.BusinessObjects.AutorizacionDocumento>(criteria);
                        AutorizacionDocumento = resolucion;
                        //OnChanged(nameof(AutorizacionDocumento));
                    }
                    //DoTipoChanged(true, tipo);
                }
            }
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
        [XafDisplayName("Banco Transacción"), Index(1), VisibleInListView(false),
            ToolTip("La transacción de bancos, cuando es transferencia, remesa, o pago electronico")]
        //[Association("BancoTransaccion-CxCTransacciones")]
        [DataSourceCriteria("[BancoCuenta.Empresa] == '@This.Venta.Empresa' && [Clasificacion.Tipo] In (1, 2)")]   // clasificacion.Tipo in (Abono, Remesa)
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public BancoTransaccion BancoTransaccion
        {
            get => bancoTransaccion;
            set => SetPropertyValue(nameof(BancoTransaccion), ref bancoTransaccion, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha"), Index(2)]
        [RuleValueComparison("CxCTransaccion.Fecha > Fecha Factura", DefaultContexts.Save,
            ValueComparisonType.GreaterThanOrEqual, "[Venta.Fecha]", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        /// <summary>
        /// Algunos tipos de concepto van a requerir una autorizacion de correlativos, habra que identificar en el BO
        /// CxCTransaccion el tipo de documento o vincularlo al concepto, para que el sistema pueda obener la autorizacion y validar
        /// que sea la correcta para el tipo de documento, ademas este vigente y el correlativo generado este dentro del 
        /// rango
        /// </summary>
        [XafDisplayName("Autorización Documento"), PersistentAlias(nameof(autorizacionDocumento)), Index(3)]
        [VisibleInListView(false)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ModelDefault("AllowEdit", "False")]
        [RuleRequiredField]
        public AutorizacionDocumento AutorizacionDocumento
        {
            get => autorizacionDocumento;
            set => SetPropertyValue<AutorizacionDocumento>(nameof(AutorizacionDocumento), ref autorizacionDocumento, value);
        }


        [DbType("int"), XafDisplayName("Número"), Index(3)]
        [ModelDefault("AllowEdit", "False")]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ToolTip("Numero Correlativo por tipo de documento, año y empresa")]
        public int? Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        /// <summary>
        /// Es string porque cuando se trata de un Dte es el Guid generado y cuando es formulario es el correlativo autorizado
        /// </summary>
        [DbType("varchar(36)"), System.ComponentModel.DisplayName("Número Documento"), Index(4)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ToolTip("El número de documento según tipo de documento. Puede ser el correlativo autorizado en formulario o el Guid del Dte")]
        [RuleRequiredField("CxCTransaccion.NumeroDocumento_requerido", DefaultContexts.Save, SkipNullOrEmptyValues = true, 
            TargetCriteria = "[Tipo.Padre.Oid] in (1, 16)")]
        public string NumeroDocumento
        {
            get => numeroDocumento;
            set => SetPropertyValue<string>(nameof(NumeroDocumento), ref numeroDocumento, value);
        }

        [XafDisplayName("Moneda"), Index(4)]
        [DataSourceCriteria("[Activa] == True")]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [DbType("numeric(12,2)"), XafDisplayName("Valor Moneda"), Index(5)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [ModelDefault("AllowEdit", "False")]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [Association("Venta-CxCTransacciones")]
        [XafDisplayName("Venta"), Index(6)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        // NO ESTA FUNCIONANDO EL SIGUIENTE ATRIBUTO, SE DEBE REVISAR 27/07/2024 Y BORRARLO SI ES EL CASO
        //[DataSourceCriteria("[Empresa.Oid] == EmpresaActualOid() && [TipoFactura.Codigo] == 'COVE01' && [Estado] == 'Debe' && [CondicionCredito] == 'Credito' && [Saldo] > 0.0")]
        public Venta Venta
        {
            get => venta;
            set
            {
                bool changed = SetPropertyValue(nameof(Venta), ref venta, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    Moneda = venta.Moneda;
                }
            }
        }

        [DbType("numeric(14,2)"), XafDisplayName("Monto"), Index(7)]
        [RuleValueComparison("CxCTransaccion.Monto > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        /// <summary>
        /// Cartera de Cuenta por Cobrar o Venta. El vendedor o el cobrador esta relacionado con la cartera y el cliente
        /// a CarteraCliente
        /// </summary>
        [Association("Cartera-CxCTransacciones"), XafDisplayName("Cartera"), Index(8)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        public Cartera Cartera
        {
            get => cartera;
            set => SetPropertyValue(nameof(Cartera), ref cartera, value);
        }

        /// <summary>
        /// Cuando el cliente realiza el pago con tarjeta de çrédito, es necesario indicar el tipo de tarjeta
        /// </summary>
        [DbType("varchar(12)"), XafDisplayName("Tipo Tarjeta"), Index(9), VisibleInListView(false)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        [DataSourceCriteria("[Categoria] == 6 And [Activo] == True")]   // categoria 6 son tarjetas de credito
        [RuleRequiredField("CxCTransaccion.TipoTarjeta_Requerido", "Save", TargetCriteria = "[Tipo.Oid] In (8, 9)",
             ResultType = ValidationResultType.Warning)]
        public Listas TipoTarjeta
        {
            get => tipoTarjeta;
            set => SetPropertyValue(nameof(TipoTarjeta), ref tipoTarjeta, value);
        }

        /// <summary>
        /// Banco emisor de la tarjeta de crédito, solo cuando el cliente realiza el pago con tarjeta de crédito
        /// </summary>
        [XafDisplayName("Banco"), Index(10)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Banco Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }

        /// <summary>
        /// No de tarjeta de credito o debito, solo cuando el cliente realiza el pago con tarjeta de crédito
        /// </summary>
        [Size(20), DbType("varchar(20)"), XafDisplayName("No Tarjeta"), ToolTip("No de Tarjeta de debito o credito, cuando es el medio de pago")]
        [Index(11), VisibleInListView(false)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string NoTarjeta
        {
            get => noTarjeta;
            set => SetPropertyValue(nameof(NoTarjeta), ref noTarjeta, value);
        }

        /// <summary>
        ///  No de cheque, No de pago electronico, Id de la remesa, transferencia, no vaucher, número nota de crédito, número dte etc.
        /// </summary>
        [Size(100), DbType("varchar(100)"), XafDisplayName("No Referencia"), Index(12), VisibleInListView(false)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        [ToolTip(@"Número de referencia o de documento. Ej: número dte, número nota de crédito, transferencia, remesa, etc")]
        [RuleRange("CxCTransaccion.Referencia_Rango", DefaultContexts.Save, "[AutorizacionDocumento.NoDesde]", "[AutorizacionDocumento.NoHasta]", 
            ParametersMode.Expression, TargetCriteria = "([Tipo.Padre.Oid] == 1 || [Tipo.Padre.Oid] == 16) && !IsNull([Venta]) && [AutorizacionDocumento.Clase] != 'Dte'", 
            SkipNullOrEmptyValues = true, CustomMessageTemplate = @"El valor ingresado en referencia debe estar en el rango autorizado de documentos")]
        public string Referencia
        {
            get => referencia;
            set => SetPropertyValue(nameof(Referencia), ref referencia, value);
        }

        [DbType("smallint"), XafDisplayName("Estado"), Index(13)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        public ECxCTransaccionEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario"), Index(14), VisibleInListView(false)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [XafDisplayName("Fecha Anulación")]
        [ModelDefault("AllowEdit", "False"), Index(98)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        public DateTime FechaAnula
        {
            get => fechaAnula;
            set => SetPropertyValue(nameof(FechaAnula), ref fechaAnula, value);
        }
        [Size(25), XafDisplayName("Usuario Anulo")]
        [ModelDefault("AllowEdit", "False"), Index(99)]
        [DetailViewLayout("Datos Pago", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string UsuarioAnulo
        {
            get => usuarioAnulo;
            set => SetPropertyValue(nameof(UsuarioAnulo), ref usuarioAnulo, value);
        }

        #endregion

        #region Collecciones

        [Browsable(false)]
        public XPCollection<CxCTipoTransaccion> TipoTransaccionesCxCValidas
        {
            get
            {
                if (tipoTransaccionesCxCValidas == null)
                {
                    tipoTransaccionesCxCValidas = new XPCollection<CxCTipoTransaccion>(Session);
                }
                if (Venta.TipoFactura.Codigo == "COVE02")  // cuando es consumidor final se excluyen notas de credito y debito
                    tipoTransaccionesCxCValidas.Filter = CriteriaOperator.FromLambda<CxCTipoTransaccion>(x => x.Padre != null  && x.Padre.Oid != 1 && x.Padre.Oid != 16 && x.Activo);
                else
                    tipoTransaccionesCxCValidas.Filter = CriteriaOperator.FromLambda<CxCTipoTransaccion>(x => x.Padre != null && x.Activo);
                return tipoTransaccionesCxCValidas;
            }
        }
            
        #endregion


        #region Metodos

        /// <summary>
        /// Metodo que se debe ejecutar cuando Cambia el Tipo de Transacción. Reescribir en las clases heredades 
        /// (documentos) cuando corresponda. En el caso de las notas de crédito y débito en este evento se obtiene
        /// la resolución de autorización para emitir los documentos, próximo correlativo del documento a emitir.
        /// </summary>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        /// <param name="oldValue">El valor de la propiedad Tipo antes del setter</param>
        protected virtual void DoTipoChanged(bool forceChangeEvents, CxCTipoTransaccion oldValue)
        {

        }

        #endregion
        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}