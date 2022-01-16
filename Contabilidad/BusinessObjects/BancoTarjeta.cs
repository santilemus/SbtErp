using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para las tarjetas de crédito y debito
    /// </summary>
    /// <remarks>
    /// 1. Agregar metodo cuando es una tarjeta de debito, automatic
    /// </remarks>
    [DefaultClassOptions, ModelDefault("Caption", "Tarjeta"), NavigationItem("Banco"), Persistent(nameof(BancoTarjeta)),
        DefaultProperty(nameof(Numero)), CreatableItem(false)]
    [ImageName("credit-card")]
    [Appearance("BancoTarjeta.LimiteCredito", Criteria = "[Tipo] != 'Credito'", Enabled = false,
            Context = "DetailView", AppearanceItemType = "ViewItem", TargetItems = "[BancoCuenta];[LimiteCredito]")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoTarjeta : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTarjeta(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            tipo = ETipoTarjeta.Credito;
            limiteCredito = 0.0m;
            
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        Empresa empresa;
        decimal limiteCredito;
        BancoCuenta bancoCuenta;
        SBT.Apps.Contabilidad.BusinessObjects.Catalogo cuentaContable;
        DateTime fechaCierre;
        DateTime fechaVence;
        DateTime fechaEmision;
        Moneda moneda;
        string nombre;
        string numero;
        ETipoTarjeta tipo;
        Tercero.Module.BusinessObjects.Tercero banco;

        [XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("int"), Persistent(nameof(Banco)), XafDisplayName("Banco"), RuleRequiredField("BancoTarjeta.Banco_Requerido", "Save")]
        //[DataSourceCriteria("[Roles][[IdRole] = 2]")]
        [DataSourceCriteria("[Activo] == true")]
        [Index(1), VisibleInLookupListView(true)]
        [ExplicitLoading]
        public Tercero.Module.BusinessObjects.Tercero Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }

        [DbType("smallint"), Persistent("Tipo"), XafDisplayName("Tipo"), Index(2)]
        public ETipoTarjeta Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [Size(25), DbType("varchar(20)"), XafDisplayName("Número"), Index(3),
            RuleRequiredField("BancoTarjeta.Numero_Requerido", DefaultContexts.Save)]
        [Persistent(nameof(Numero)), Indexed(nameof(Banco), Name = "idxBancoTarjetaNumero", Unique = true)]
        public string Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        [Size(60), DbType("varchar(60)"), Persistent("Nombre"), XafDisplayName("Nombre"), VisibleInListView(false),
            Index(4)]
        [ToolTip("Nombre según la tarjeta de crédito")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("varchar(3)"), Persistent("Moneda"), XafDisplayName("Moneda"), RuleRequiredField("BancoTarjeta.Moneda_Requerido", "Save")]
        [Index(5)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [DbType("datetime"), Persistent(nameof(FechaEmision)), XafDisplayName("Fecha Emisión"),
         RuleRequiredField("BancoTarjeta.FechaEmision_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        [Index(6), VisibleInListView(false)]
        public DateTime FechaEmision
        {
            get => fechaEmision;
            set => SetPropertyValue(nameof(FechaEmision), ref fechaEmision, value);
        }

        [DbType("datetime"), Persistent(nameof(FechaVence)), XafDisplayName("Fecha Vence"), Index(7), VisibleInListView(false),
         RuleValueComparison("BancoTarjeta.FechaVence > FechaEmision", DefaultContexts.Save,
         ValueComparisonType.GreaterThan, "FechaEmision", ParametersMode.Expression, SkipNullOrEmptyValues = true)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaVence
        {
            get => fechaVence;
            set => SetPropertyValue(nameof(FechaVence), ref fechaVence, value);
        }

        [DbType("datetime"), Persistent(nameof(FechaCierre)), XafDisplayName("Fecha Cierre"), Index(8), VisibleInListView(false),
         RuleValueComparison("BancoTarjeta.FechaCierre > FechaEmision", DefaultContexts.Save,
         ValueComparisonType.GreaterThan, "FechaEmision", ParametersMode.Expression, SkipNullOrEmptyValues = true)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaCierre
        {
            get => fechaCierre;
            set => SetPropertyValue(nameof(FechaCierre), ref fechaCierre, value);
        }

        [Index(9), XafDisplayName("Cuenta Banco"), VisibleInLookupListView(true)]
        [ToolTip("Número de cuenta bancaria, cuando la tarjeta es de débito")]
        public BancoCuenta BancoCuenta
        {
            get => bancoCuenta;
            set => SetPropertyValue(nameof(BancoCuenta), ref bancoCuenta, value);
        }

        [Persistent(nameof(CuentaContable)), XafDisplayName("Cuenta Contable"), Index(10), VisibleInListView(false)]
        [ExplicitLoading]
        public SBT.Apps.Contabilidad.BusinessObjects.Catalogo CuentaContable
        {
            get => cuentaContable;
            set => SetPropertyValue(nameof(CuentaContable), ref cuentaContable, value);
        }

        [XafDisplayName("Límite Crédito"), Index(11)]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal LimiteCredito
        {
            get => limiteCredito;
            set => SetPropertyValue(nameof(LimiteCredito), ref limiteCredito, value);
        }

        #endregion

        #region Colecciones
        [Association("BancoTarjeta-Transacciones"), DevExpress.Xpo.Aggregated, ModelDefault("Caption", "Transacciones"), Index(0)]
        public XPCollection<BancoTarjetaTransaccion> Transacciones => GetCollection<BancoTarjetaTransaccion>(nameof(Transacciones));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

}