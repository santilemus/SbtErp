using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Editors;
using DevExpress.Drawing;
using SBT.Apps.Tercero.Module.BusinessObjects;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Banco. BO para las transacciones de bancos 
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Transacción Bancaria"), NavigationItem("Banco"), Persistent(nameof(BancoTransaccion)),
        DefaultProperty(nameof(Numero))]  // revisar si es la propiedad por default adecuada
    [RuleCombinationOfPropertiesIsUnique("BancoTransaccion.Cuenta_Clasificacion_Numero", DefaultContexts.Save,
        "BancoCuenta,Clasificacion,Numero", CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = true)]
    [RuleIsReferenced("BancoTransaccion.Reference_Delete", DefaultContexts.Delete, typeof(BancoConciliacionDetalle), "Transaccion",
        InvertResult = true, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction,
        MessageTemplateMustBeReferenced = "El objeto {TargetObject} no debe tener referencias.")]
    [RuleIsReferenced("BancoTransaccion.Reference_Editar", DefaultContexts.Save, typeof(BancoConciliacionDetalle), "Transaccion",
        InvertResult = true, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction,
        MessageTemplateMustBeReferenced = "El objeto {TargetObject} no debe tener referencias.")]
    [Appearance("BancoTransaccion.Cheque y Cargo", Criteria = "[Clasificacion.Tipo] == 'Cheque' || [Clasificacion.Tipo] == 'Cargo'", AppearanceItemType = "ViewItem",
        Visibility = ViewItemVisibility.Hide, Context = "DetailView", TargetItems = "Abono;IdReferencia;Cobros")]
    [Appearance("BancoTransaccion.Remesa y Abono", Criteria = "[Clasificacion.Tipo] == 'Abono' || [Clasificacion.Tipo] == 'Remesa'", AppearanceItemType = "ViewItem",
        Visibility = ViewItemVisibility.Hide, Context = "DetailView", TargetItems = "ChequeNo;Proveedor;Beneficiario;Cargo;Pagos")]
    [Appearance("BancoCuenta.TarjetaCredito", Criteria = "[Cuenta.Tipo Cuenta] = 'Tarjeta'", AppearanceItemType = "ViewItem", TargetItems = "BancoCuenta",
        Context = "DetailView")]
    [Appearance("BancoCuenta_Cheque", Criteria = "[Clasificacion.Tipo] != 'Cheque'", AppearanceItemType = "ViewItem", Visibility = ViewItemVisibility.Hide,
        Context = "DetailView", TargetItems = "ChequeNo")]

    [ImageName(nameof(BancoTransaccion))]
    [DevExpress.Xpo.OptimisticLocking(Enabled = true, LockingKind = OptimisticLockingBehavior.ConsiderOptimisticLockingField)]
    [DevExpress.Xpo.OptimisticLockingReadBehavior(OptimisticLockingReadBehavior.ReloadObject, true)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoTransaccion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            numero = null;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            if ((Session is not NestedUnitOfWork) && (Session.DataLayer != null) && (Session.ObjectLayer is SecuredSessionObjectLayer) &&
                 Session.IsNewObject(this) && (Numero == null || Numero <= 0))
            {
                if (Clasificacion.Tipo == EBancoTipoTransaccion.Cheque && Session.IsNewObject(this) && chequera != null)
                {
                    if (ChequeNo >= chequera.NumeroInicio && ChequeNo < chequera.NumeroFin)
                        chequera.NumeroActual++;
                    chequera.Save();
                }
                numero = CorrelativoDoc();
            }
            base.OnSaving();
        }

        protected override void OnLoading()
        {
            base.OnLoading();
            chequera = null;
        }

        #region Propiedades
        [Size(250), DbType("varchar(250)"), Persistent(nameof(Comentario)), NonCloneable]
        string comentario;
        [DbType("varchar(25)"), Persistent(nameof(UsuarioAnulo)), NonCloneable]
        string usuarioAnulo;
        [DbType("datetime2"), Persistent(nameof(FechaAnula)), NonCloneable]
        DateTime? fechaAnula;
        decimal valorMoneda;
        Moneda moneda;
        DateTime fecha;
        [DbType("int"), Persistent(nameof(Numero)), NonCloneable]
        int? numero;
        string idReferencia;
        string concepto;
        BancoCuenta bancoCuenta;
        BancoTipoTransaccion clasificacion;
        [DbType("int"), Persistent("ChequeNo")]
        int chequeNo;
        [Size(8), DbType("varchar(8)"), Persistent("Serie")]
        string serie;
        string beneficiario;
        decimal monto;
        EBancoTransaccionEstado estado = EBancoTransaccionEstado.Digitado;
        BancoChequera chequera;
        private Tercero.Module.BusinessObjects.Tercero tercero;

        [Association("BancoCuenta-Transacciones"), Persistent(nameof(BancoCuenta)), XafDisplayName("Cuenta"), Index(0)]
        [DataSourceCriteria("[Empresa.Oid] == EmpresaActualOid()")]
        [ExplicitLoading]
        [ImmediatePostData(true)]
        [RuleRequiredField("BancoTransaccion.BancoCuenta_requerido", DefaultContexts.Save)]
        public BancoCuenta BancoCuenta
        {
            get => bancoCuenta;
            set
            {
                bool changed = SetPropertyValue(nameof(BancoCuenta), ref bancoCuenta, value);
                if (!IsLoading && !IsSaving && changed && Moneda == null)
                {
                    Moneda = BancoCuenta.Moneda;
                    ValorMoneda = Moneda.FactorCambio;
                }
            }
        }

        [Persistent("Clasificacion"), XafDisplayName("Clasificacion"), RuleRequiredField("BancoTransaccion.Tipo_Requerido", "Save")]
        [ToolTip("Clasificación de Transacciones de bancos", "Bancos", ToolTipIconType.Information)]
        [Index(1), VisibleInLookupListView(true)]
        [ExplicitLoading]
        public BancoTipoTransaccion Clasificacion
        {
            get => clasificacion;
            set
            {
                var changed = SetPropertyValue(nameof(Clasificacion), ref clasificacion, value);
                if (!IsLoading && !IsSaving && changed && value.Tipo == EBancoTipoTransaccion.Cheque)
                {
                    chequera = Session.FindObject<BancoChequera>(
                        DevExpress.Data.Filtering.CriteriaOperator.Parse("NumeroCuenta = ? And FechaFin Is Null", BancoCuenta));
                    if (chequera != null)
                        serie = chequera.Serie;
                    if (chequera != null && Session.IsNewObject(this))
                        chequeNo = chequera.NumeroActual;
                }
            }
        }

        [Index(2), XafDisplayName("Número")]
        [PersistentAlias(nameof(numero))]
        public int? Numero
        {
            get => numero;
        }

        [DbType("datetime2"), Persistent(nameof(Fecha)), VisibleInLookupListView(true)]
        [XafDisplayName("Fecha"), Index(3), RuleRequiredField("BancoTransaccion.Fecha_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}"), ModelDefault("EditMask", "dd/MM/yyyy")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [Persistent(nameof(Moneda))]
        [Index(4), XafDisplayName("Moneda"), RuleRequiredField("BancoTransaccion.Moneda_Requerida", DefaultContexts.Save)]
        [ExplicitLoading]
        public Moneda Moneda
        {
            get => moneda;
            set
            {
                bool changed = SetPropertyValue(nameof(Moneda), ref moneda, value);
                if (!IsLoading && !IsSaving && changed)
                    valorMoneda = value.FactorCambio;
            }
        }

        [DbType("numeric(12,2)"), Persistent(nameof(ValorMoneda)), VisibleInListView(false)]
        [XafDisplayName("Valor Moneda"), Index(5), ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [XafDisplayName("Cheque No")]
        [PersistentAlias(nameof(chequeNo)), Index(6)]
        public int ChequeNo
        {
            get => chequeNo;
        }

        [System.ComponentModel.DisplayName("Tercero"), Index(7), VisibleInListView(false), Persistent(nameof(Tercero))]
        [DataSourceCriteria(@"[Roles][Not [Id Role] In ('Consultorio', 'Banco', 'Fabricante')]")]
        [ToolTip(@"Tercero involucrado en la transacción. Cuando es un pago a la CxP es el proveedor; cuando es una transacción de CxC es el cliente")]
        public Tercero.Module.BusinessObjects.Tercero Tercero
        {
            get => tercero;
            set
            {
                bool changed = SetPropertyValue(nameof(Tercero), ref tercero, value);
                if (!IsLoading && !IsSaving && changed)
                    Beneficiario = Tercero?.Nombre ?? string.Empty;
            }
        }

        [Size(150), DbType("varchar(150)"), Persistent("Beneficiario"), XafDisplayName("Beneficiario"), Index(8)]
        [VisibleInLookupListView(true), VisibleInListView(false)]
        public string Beneficiario
        {
            get => beneficiario;
            set => SetPropertyValue(nameof(Beneficiario), ref beneficiario, value);
        }

        [DbType("money"), Persistent("Monto"), XafDisplayName("Monto"), Index(9), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("BancoTransaccion.Monto >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("Referencia Banco"), Index(10)]
        [ToolTip("Id de la transacción registrada en el banco, cuando se conoce")]
        public string IdReferencia
        {
            get => idReferencia;
            set => SetPropertyValue(nameof(IdReferencia), ref idReferencia, value);
        }
        [Size(150), DbType("varchar(150)"), XafDisplayName("Concepto"), Index(11)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [DbType("smallint"), Persistent("Estado"), XafDisplayName("Estado"), Index(12)]
        public EBancoTransaccionEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [PersistentAlias("Iif([Clasificacion.Tipo] = 1 Or [Clasificacion.Tipo] = 2, [Monto], 0)")]
        [XafDisplayName("Abono"), ModelDefault("DisplayFormat", "{0:N2}"), Index(14)]
        public decimal Abono
        {
            get { return Convert.ToDecimal(EvaluateAlias("Abono")); }
        }


        [PersistentAlias("Iif([Clasificacion.Tipo] = 3 Or [Clasificacion.Tipo] = 4, [Monto], 0)")]
        [XafDisplayName("Cargo"), ModelDefault("DisplayFormat", "{0:N2}"), Index(15)]
        public decimal Cargo
        {
            get { return Convert.ToDecimal(EvaluateAlias("Cargo")); }
        }

        [XafDisplayName("Fecha Anulación"), ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "g"), Index(16), ModelDefault("AllowEdit", "False")]
        [PersistentAlias(nameof(fechaAnula)), Browsable(false)]
        public DateTime? FechaAnula => fechaAnula;

        [Size(25), Index(17), XafDisplayName("Usuario Anuló"), ModelDefault("AllowEdit", "False"),
            PersistentAlias(nameof(usuarioAnulo)), Browsable(false)]
        public string UsuarioAnulo => usuarioAnulo;

        [PersistentAlias(nameof(comentario))]
        [Size(250), Index(18), XafDisplayName("Comentario")]
        public string Comentario => comentario;

        [XafDisplayName("Saldo Cuenta"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [Appearance("BancoTransaccion.CuentaSaldo", FontStyle = DXFontStyle.Bold, BackColor = "SlateBlue", FontColor = "Orange",
            Criteria = "!IsNull([Saldo])")]
        public decimal Saldo
        {
            get
            {
                if (!IsLoading && !IsSaving && BancoCuenta != null)
                    return BancoCuenta.CalcularSaldo(Fecha);
                else
                    return 0.0m;
            }
        }

        [Browsable(false)]
        [RuleFromBoolProperty("BancoTransaccion.ExisteSaldo", DefaultContexts.Save,
            TargetCriteria = "[Monto] != 0.0 && ([Clasificacion.Tipo] == 'Cheque' || [Clasificacion.Tipo] == 'Cargo')", SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Para la Emisión de Cheques y Notas de Cargo, la cuenta debe tener fondos suficientes para cubrir el monto de la transacción")]
        private bool ExisteSaldo
        {
            get
            {
                if (IsLoading || IsSaving)
                    return true;
                if (Clasificacion.Tipo == EBancoTipoTransaccion.Remesa || Clasificacion.Tipo == EBancoTipoTransaccion.Abono)
                    return true;
                else
                    return Monto <= BancoCuenta.CalcularSaldo(Fecha, Oid);
            }

        }

        #endregion

        #region Colecciones

        //[Association("BancoTransaccion-Pagos"), XafDisplayName("Pagos"), Index(0), DevExpress.Xpo.Aggregated]
        //public XPCollection<BancoTransaccionPago> Pagos => GetCollection<BancoTransaccionPago>(nameof(Pagos));

        [Association("BancoTransaccion-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle"), Index(1)]
        public XPCollection<BancoTransaccionDetalle> Detalles => GetCollection<BancoTransaccionDetalle>(nameof(Detalles));

        [Association("BancoTransaccion-Conciliaciones"), XafDisplayName("Conciliaciones"), Index(2)]
        public XPCollection<BancoConciliacionDetalle> Conciliaciones => GetCollection<BancoConciliacionDetalle>(nameof(Conciliaciones));

        [Association("BancoTransaccion-Partidas"), XafDisplayName("Partidas"), DevExpress.Xpo.Aggregated, Index(3)]
        public XPCollection<BancoTransaccionPartida> BancoTransaccionPartidas => GetCollection<BancoTransaccionPartida>(nameof(BancoTransaccionPartidas));

        //[Association("BancoTransaccion-CxPTransacciones",AssemblyName = "SBT.Apps.Compra.Module", ElementTypeName = "SBT.Apps.CxP.Module.BusinessObjects.CxPTransaccion")]
        //[XafDisplayName("Pagos")]
        //public XPCollection CxPTransacciones => GetCollection(nameof(CxPTransacciones));

        //[Association("BancoTransaccion-CxCTransacciones", AssemblyName = "SBT.Apps.Facturacion.Module", ElementTypeName = "SBT.Apps.CxC.Module.BusinessObjects.CxCTransaccion")]
        //[XafDisplayName("Cobros")]
        //[DataSourceCriteria("[Clasificacion.Tipo] In (1, 2) && [Estado] <= 2 && [FechaAnula] Is Not Null")]
        //public XPCollection Cobros => GetCollection(nameof(Cobros));

        #endregion

        [Action(Caption = "Entregar Cheque", ConfirmationMessage = "Esta Seguro?", TargetObjectsCriteria = "Clasificacion.Tipo = 3",
            SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject, ImageName = "Attention", AutoCommit = true,
            ToolTip = "Marcar el cheque seleccionado como entregado")]

        public void Entregado()
        {
            Estado = EBancoTransaccionEstado.Entregado;
            Save();

        }

        //[Action(Caption = "Test")]
        //public void TestNumberToLetter()
        //{
        //    Base.Module.NumeroALetras numLetter = new ();
        //    this.comentario = numLetter.Convertir(Monto, Moneda.Plural);
        //}

        /// <summary>
        /// Reescribimos el metodo para generar un correlativo por tipo de transaccion y año
        /// </summary>
        /// <returns></returns>
        protected int CorrelativoDoc()
        {
            object max;
            CriteriaOperator criteria = CriteriaOperator.FromLambda<BancoTransaccion>(x => x.BancoCuenta.Empresa.Oid == BancoCuenta.Empresa.Oid &&
            x.Clasificacion.Tipo == Clasificacion.Tipo && x.Fecha.Year == Fecha.Year);
            var oldValue = Session.LockingOption;
            Session.LockingOption = LockingOption.Optimistic;
            max = Session.Evaluate<BancoTransaccion>(CriteriaOperator.Parse("Max(Numero)"), criteria);
            Session.LockingOption = oldValue;
            return Convert.ToInt32(max ?? 0) + 1;
        }

        /// <summary>
        /// Invocar cuando se anula una transaccion de bancos, para agregar informacion de comentario, fecha, usuario que anulo y guardar los cambios
        /// </summary>
        /// <param name="AnularParams">Parametros para realizar la anulacion</param>
        [Action(Caption = "Anular", ConfirmationMessage = "Esta Segur@? de Anular la Transacción?", ImageName = "Attention", AutoCommit = true)]
        public virtual void Anular(AnularParametros AnularParams)
        {
            try
            {
                comentario += string.IsNullOrEmpty(Comentario.Trim()) ? AnularParams.Comentario : $"{Environment.NewLine}{AnularParams.Comentario}";
                fechaAnula = DateTime.Now;
                usuarioAnulo = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
                estado = EBancoTransaccionEstado.Anulado;
                Save();
            }
            catch
            {
                CancelEdit();
                throw;
            }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}


//Re: Generate unique number in C#

//string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

//Limited by the timer resolution which is platform dependent. But you will be able to generate many per second without risk of collisson.
//This will give you about 30 years before wrap around and a risk of collission if I did the math right...