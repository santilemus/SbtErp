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
using SBT.Apps.Contabilidad.BusinessObjects;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false), ModelDefault("Caption", "Tarjeta Transacción"), NavigationItem("Banco"),
        DefaultProperty(nameof(Fecha)), Persistent(nameof(BancoTarjetaTransaccion))]
    [ImageName("credit_card-to_bank")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCriteria("BancoTarjetaTransaccion Tarjeta No Cerrada", DefaultContexts.Save, 
        "[Fecha] >= [Tarjeta.FechaEmision] && IsNull([Tarjeta.FechaVence]) && IsNull([Tarjeta.FechaCierre])")]
    public class BancoTarjetaTransaccion : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTarjetaTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            numero = -1;
            Tipo = ETipoOperacion.Cargo;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedadades

        decimal monto;
        ETipoOperacion tipo;
        string concepto;
        DateTime fecha;
        [Persistent(nameof(Numero))]
        int numero;
        BancoTarjeta tarjeta;

        [Association("BancoTarjeta-Transacciones"), Index(0)]
        public BancoTarjeta Tarjeta
        {
            get => tarjeta;
            set => SetPropertyValue(nameof(Tarjeta), ref tarjeta, value);
        }

        [PersistentAlias(nameof(numero)), XafDisplayName("Número"), Index(1)]
        public int Numero => Numero;

        [DbType("datetime2"), XafDisplayName("Fecha"), Index(3)]
        [RuleRequiredField("BancoTarjetaTransaccion.Fecha_Requerido", DefaultContexts.Save)]
        [RuleValueComparison("BancoTarjetaTransaccion.Fecha >= FechaEmision", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "[Tarjeta.FechaEmision]", ParametersMode.Expression)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "g")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [RuleRange("BancoTarjetaTransaccion.Tipo es Cargo o Abono", DefaultContexts.Save, 1, 2, SkipNullOrEmptyValues = false)]
        [Index(4)]
        public ETipoOperacion Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [Size(100), DbType("varchar(100)"), RuleRequiredField("BancoTarjetaTransaccion.Concepto_requerido", "Save")]
        [Index(5), VisibleInLookupListView(true)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [DbType("numeric(14,2)"), Persistent(nameof(Monto)), XafDisplayName("Monto"), Index(6), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("BancoTarjetaTransaccion.Monto >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        [PersistentAlias("Iif([Tipo] = 1, [Monto], 0)")]
        [XafDisplayName("Cargo"), ModelDefault("DisplayFormat", "{0:N2}"), Index(7)]
        public decimal Cargo
        {
            get { return Convert.ToDecimal(EvaluateAlias("Cargo")); }
        }

        [PersistentAlias("Iif([Tipo] = 2, [Monto], 0)")]
        [XafDisplayName("Abono"), ModelDefault("DisplayFormat", "{0:N2}"), Index(8)]
        public decimal Abono
        {
            get { return Convert.ToDecimal(EvaluateAlias("Abono")); }
        }

        #endregion

        protected override void OnSaving()
        {
            if (Session is NestedUnitOfWork)
                return;
            //base.OnSaving();
            if (Session.IsNewObject(this) && numero == -1)
            {
                object max;
                string sCriteria = "BancoTarjeta.Empresa.Oid == ? && GetYear(Fecha) == ?";
                var oldValue = Session.LockingOption;
                Session.LockingOption = LockingOption.Optimistic;
                max = Session.Evaluate<BancoTarjetaTransaccion>(CriteriaOperator.Parse("Max(Numero)"), CriteriaOperator.Parse(sCriteria, Tarjeta.Empresa.Oid, Fecha.Year));
                Session.LockingOption = oldValue;
                numero = Convert.ToInt32(max ?? 0) + 1;
            }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}