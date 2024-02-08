using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para las cuentas bancarias 
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Cuenta Bancaria"), NavigationItem("Banco"), Persistent(nameof(BancoCuenta)),
        DefaultProperty(nameof(Numero))]
    [RuleCombinationOfPropertiesIsUnique("BancoCuenta.Banco_Numero", DefaultContexts.Save, "Banco,Numero",
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
    [ImageName(nameof(BancoCuenta))]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoCuenta : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoCuenta(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            CalcularSaldo(DateTime.Now);
        }

        #region Propiedades
        Empresa empresa;
        Tercero.Module.BusinessObjects.Banco banco;
        string numero;
        string nombre;
        ETipoCuentaBanco tipo = ETipoCuentaBanco.Corriente;
        Moneda moneda;
        DateTime fechaApertura;
        DateTime fechaCierre;
        Catalogo cuentaContable;
        DevExpress.Persistent.BaseImpl.ReportDataV2 reporteCheque;

        [Persistent("Empresa"), DbType("int"), XafDisplayName("Empresa"), Index(0), VisibleInListView(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("int"), Persistent("Banco"), XafDisplayName("Banco"), RuleRequiredField("BancoCuenta.Banco_Requerido", "Save")]
        //[DataSourceCriteria("[Roles][[IdRole] = 2]")]
        [DataSourceCriteria("[Activo] == true")]
        [Index(1), VisibleInLookupListView(true)]
        [ExplicitLoading]
        public Tercero.Module.BusinessObjects.Banco Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo Cuenta"), Index(2)]
        public ETipoCuentaBanco Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }
        [Size(25), DbType("varchar(25)"), XafDisplayName("Número Cuenta"), Index(3),
            RuleRequiredField("BancoCuenta.Numero_Requerido", DefaultContexts.Save)]
        [Persistent(nameof(Numero)), Indexed(nameof(Banco), Name = "idxBancoCuenta", Unique = true)]
        public string Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        [Size(60), DbType("varchar(60)"), Persistent("Nombre"), XafDisplayName("Nombre Cuenta"), VisibleInListView(false),
            Index(4)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("varchar(3)"), Persistent("Moneda"), XafDisplayName("Moneda"), RuleRequiredField("BancoCuenta.Moneda_Requerido", "Save")]
        [Index(5)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [DbType("datetime"), Persistent("FechaApertura"), XafDisplayName("Fecha Apertura"),
            RuleRequiredField("BancoCuenta.FechaApertura_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        [Index(6), VisibleInListView(false)]
        public DateTime FechaApertura
        {
            get => fechaApertura;
            set => SetPropertyValue(nameof(FechaApertura), ref fechaApertura, value);
        }

        [DbType("datetime"), Persistent("FechaCierre"), XafDisplayName("Fecha Cierre"), Index(7), VisibleInListView(false),
             RuleValueComparison("BancoCuenta.FechaCierre > FechaApertura", DefaultContexts.Save,
            ValueComparisonType.GreaterThan, "FechaApertura", ParametersMode.Expression, SkipNullOrEmptyValues = true)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaCierre
        {
            get => fechaCierre;
            set => SetPropertyValue(nameof(FechaCierre), ref fechaCierre, value);
        }


        [Persistent(nameof(CuentaContable)), XafDisplayName("Cuenta Contable"), Index(8), VisibleInListView(false)]
        [ExplicitLoading]
        public Catalogo CuentaContable
        {
            get => cuentaContable;
            set => SetPropertyValue(nameof(CuentaContable), ref cuentaContable, value);
        }


        [XafDisplayName("Saldo Cuenta"), Index(9)]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [Appearance("BancoCuenta.Saldo", FontStyle = System.Drawing.FontStyle.Bold, BackColor = "SlateBlue", FontColor = "Orange",
            Criteria = "!IsNull([Saldo])")]
        public decimal Saldo
        {
            get
            {
                if (!IsLoading && !IsSaving && !Session.IsNewObject(this))
                    return CalcularSaldo(DateTime.Now);
                else
                    return 0.0m;
            }
        }

        [Persistent("ReporteCheque"), XafDisplayName("Reporte Cheque"), VisibleInListView(false), Index(10)]
        public DevExpress.Persistent.BaseImpl.ReportDataV2 ReporteCheque
        {
            get => reporteCheque;
            set => SetPropertyValue(nameof(ReporteCheque), ref reporteCheque, value);
        }

        #endregion

        #region Colecciones
        [Association("BancoCuenta-Chequeras"), DevExpress.Xpo.Aggregated, XafDisplayName("Chequeras"), Index(0)]
        public XPCollection<BancoChequera> Chequeras
        {
            get
            {
                return GetCollection<BancoChequera>(nameof(Chequeras));
            }
        }

        [Association("BancoCuenta-Transacciones"), DevExpress.Xpo.Aggregated, XafDisplayName("Transacciones"), Index(1)]
        public XPCollection<BancoTransaccion> Transacciones
        {
            get
            {
                return GetCollection<BancoTransaccion>(nameof(Transacciones));
            }
        }

        [Association("BancoCuenta-Conciliaciones"), DevExpress.Xpo.Aggregated, XafDisplayName("Conciliaciones"), Index(2)]
        public XPCollection<BancoConciliacion> Conciliaciones
        {
            get
            {
                return GetCollection<BancoConciliacion>(nameof(Conciliaciones));
            }
        }
        #endregion

        #region Metodos
        public decimal CalcularSaldo(DateTime AFecha)
        {
            return Convert.ToDecimal(Session.Evaluate<BancoTransaccion>(CriteriaOperator.Parse("Sum(Iif([Clasificacion.Tipo] = 1 Or [Clasificacion.Tipo] = 2, [Monto],-[Monto]))"),
                            CriteriaOperator.Parse("[BancoCuenta.Empresa.Oid] = ? And [BancoCuenta.Oid] = ? And [Estado] != 3 And [Fecha] <= ?", Empresa.Oid, Oid, AFecha)));
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}