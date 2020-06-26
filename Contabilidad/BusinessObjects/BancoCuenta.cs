﻿using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;



namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para las cuentas bancarias
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Cuenta Bancaria"), NavigationItem("Banco"), Persistent("BanCuenta"), 
        DefaultProperty("Numero")]
    [RuleCombinationOfPropertiesIsUnique("BancoCuenta.Banco_NumeroCta", DefaultContexts.Save, "Banco,Oid",
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
    [ImageName(nameof(BancoCuenta))]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
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
            Empresa = EmpresaDeSesion();
            Moneda = ObtenerMonedaBase();
     
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        Empresa empresa;
        SBT.Apps.Tercero.Module.BusinessObjects.Tercero banco;
        string numero;
        string nombre;
        ETipoCuentaBanco tipo = ETipoCuentaBanco.Corriente;
        Moneda moneda;
        DateTime fechaApertura;
        DateTime fechaCierre;
        Catalogo cuentaConta;
        DevExpress.Persistent.BaseImpl.ReportDataV2 reporteCheque;
        decimal saldo;

        [Persistent("Empresa"), DbType("int"), XafDisplayName("Empresa"), Browsable(false), Index(0)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("int"), Persistent("Banco"), XafDisplayName("Banco"), RuleRequiredField("BancoCuenta.Banco_Requerido", "Save")]
        [DataSourceCriteria("[Roles][[IdRole] = 2]")]
        [Index(1), VisibleInLookupListView(true)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Banco
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
        [Size(25), DbType("varchar(25)"), Persistent("Oid"), XafDisplayName("Número Cuenta"), Index(3), 
            RuleRequiredField("BancoCuenta.Numero_Requerido", DefaultContexts.Save)]
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


        [Persistent("CuentaConta"), XafDisplayName("Cuenta Contable"), Index(8), VisibleInListView(false)]
        public Catalogo CuentaConta
        {
            get => cuentaConta;
            set => SetPropertyValue(nameof(CuentaConta), ref cuentaConta, value);
        }

        
        [PersistentAlias(nameof(saldo)), Index(9)]
        public decimal Saldo
        {
            get { return saldo; }
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
        public void CalcularSaldo(DateTime AFecha)
        {         
            var valor = Session.Evaluate<BancoTransaccion>(CriteriaOperator.Parse("Sum(Iif([Clasificacion.Tipo] = 1 Or [Clasificacion.Tipo] = 2, [Monto],-[Monto])"),
                            CriteriaOperator.Parse("[Empresa] = ? And [NumeroCuenta] = ? And [Estado] != 3 And [Fecha] <= ?", Empresa.Oid , Oid, AFecha));
            saldo = Convert.ToDecimal(valor);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}