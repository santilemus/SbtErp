using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. BO con los saldos mensuales contables. Es calculado por un SP, aqui no se ingresan datos desde la interfaz de usuario
    /// </summary>
    /// 
    [ModelDefault("Caption", "Saldo Mensual"), NavigationItem("Contabilidad"), Persistent("ConSaldoMes"), CreatableItem(false), VisibleInReports(true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SaldoMes : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SaldoMes(Session session)
            : base(session)
        {
        }

        public SaldoMes(Session session, Empresa emp, Periodo per, Catalogo cta, DateTime dia, decimal valdebe, decimal valhaber)
            : base(session)
        {
            empresa = emp;
            periodo = per;
            cuenta = cta;
            mes = dia.Month;
            mesAnio = Convert.ToInt32(string.Format("{0:D}{1:MMyyyy}", emp.Oid, dia));
            debe = valdebe;
            haber = valhaber;
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            empresa = null;
            periodo = null;
            mesAnio = 0;
            cuenta = null;
            saldoInicio = 0.0m;
            debe = 0.0m;
            haber = 0.0m;
            saldoFin = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(Oid)), Key(true), DbType("bigint")]
        Int64 oid = -1;
        [Persistent(nameof(Empresa)), FetchOnly]
        Empresa empresa;
        [DbType("int"), Persistent(nameof(Periodo)), FetchOnly]
        Periodo periodo;
        [Persistent(nameof(MesAnio)), DbType("int"), FetchOnly, Indexed(nameof(Cuenta), Name = "idxMesAnioCuenta_SaldoMes", Unique = true)]
        int mesAnio;
        Catalogo cuenta;
        [Persistent(nameof(Mes)), DbType("smallint"), FetchOnly]
        int mes = 1;
        [Persistent(nameof(SaldoInicio)), DbType("money"), FetchOnly]
        decimal saldoInicio;
        [Persistent(nameof(Debe)), DbType("money"), FetchOnly]
        decimal debe;
        [Persistent(nameof(Haber)), DbType("money"), FetchOnly]
        decimal haber;
        [Persistent(nameof(SaldoFin)), DbType("money"), FetchOnly]
        decimal saldoFin;


        [PersistentAlias(nameof(oid))]
        public Int64 Oid
        {
            get { return oid; }
        }


        [PersistentAlias(nameof(empresa)), XafDisplayName("Empresa"), Browsable(false), Index(0)]
        public Empresa Empresa
        {
            get { return empresa; }
        }


        [PersistentAlias(nameof(periodo)), XafDisplayName("Período"), Index(1)]
        public Periodo Periodo
        {
            get => periodo;
        }

        [PersistentAlias(nameof(mesAnio)), XafDisplayName("Mes y Año"), ModelDefault("DisplayFormat", "{0:0#####}")]
        [VisibleInListView(false), Index(2)]
        public int MesAnio
        {
            get { return mesAnio; }
        }

        [Persistent(nameof(Cuenta)), XafDisplayName("Cuenta"), VisibleInListView(true), Index(3)]
        public Catalogo Cuenta
        {
            get => cuenta;
            set => SetPropertyValue(nameof(Cuenta), ref cuenta, value);
        }

        [PersistentAlias(nameof(mes)), XafDisplayName("Mes"), Index(4)]
        public int Mes
        {
            get { return mes; }
        }

        [Size(20), Index(5)]
        public string NombreMes => string.Format("{0:MMMM}", new DateTime(Periodo.Oid, Mes, 01));

        [PersistentAlias(nameof(saldoInicio)), XafDisplayName("Saldo Inicio Mes")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [Index(6)]
        public decimal SaldoInicio
        {
            get { return saldoInicio; }
        }

        [PersistentAlias(nameof(debe)), XafDisplayName("Debe")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), Index(7)]
        public decimal Debe
        {
            get { return debe; }
        }

        [PersistentAlias(nameof(haber)), XafDisplayName("Haber")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [Index(8)]
        public decimal Haber
        {
            get { return haber; }
        }

        [PersistentAlias(nameof(saldoFin)), XafDisplayName("Saldo Fin Mes")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [Index(9)]
        public decimal SaldoFin
        {
            get { return saldoFin; }
        }


        [PersistentAlias("[SaldoFin] - [SaldoInicio]"), XafDisplayName("Movimiento Mes")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [VisibleInListView(false), Index(10)]
        public decimal MovimientoMes
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(MovimientoMes))); }
        }


        #endregion

        #region Metodos
        public void Update(decimal totDebe, decimal totHaber)
        {
            debe = totDebe;
            haber = totHaber;
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}