using System;
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
using SBT.Apps.Contabilidad.BusinessObjects;
using SBT.Apps.Contabilidad.Module.BusinessObjects;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. BO con los saldos mensuales contables. Es calculado por un SP, aqui no se ingresan datos desde la interfaz de usuario
    /// </summary>
    /// 
    [ModelDefault("Caption", "Saldo Mensual"), NavigationItem(false), Persistent("ConSaldoMes")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ConSaldoMes : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ConSaldoMes(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(Oid)), Key(true), DbType("bigint")]
        Int64 oid = -1;
        [Persistent(nameof(Empresa)), FetchOnly]
        Empresa empresa;
        [DbType("int"), Persistent(nameof(Periodo)), FetchOnly]
        Periodo periodo;
        [Persistent(nameof(MesAnio)), DbType("int"), FetchOnly, Indexed(nameof(cuenta), Name = "idx_MesAnio_Cuenta")]
        int mesAnio;
        [Persistent(nameof(Cuenta))]
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


        [PersistentAlias(nameof(empresa)), XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get { return empresa; }
        }


        [PersistentAlias(nameof(periodo)), XafDisplayName("Período")]
        public Periodo Periodo
        {
            get => periodo;
        }

        [PersistentAlias(nameof(mesAnio)), XafDisplayName("Mes y Año")]
        public int MesAnio
        {
            get { return mesAnio; }
        }

        [PersistentAlias(nameof(cuenta)), XafDisplayName("Cuenta")]
        public Catalogo Cuenta
        {
            get => cuenta;
        }

        
        [PersistentAlias(nameof(mes)), XafDisplayName("Mes")]
        public int Mes
        {
            get { return mes; }
        }
        
        [PersistentAlias("[Cuenta.Nombre]")]
        public string Nombre
        {
            get { return Convert.ToString(EvaluateAlias(nameof(Nombre))); }
        }

        [PersistentAlias(nameof(saldoInicio)), XafDisplayName("Saldo Inicio Mes")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SaldoInicio
        {
            get { return saldoInicio; }
        }
        
        [PersistentAlias(nameof(debe)), XafDisplayName("Debe")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Debe
        {
            get { return debe; }
        }

        [PersistentAlias(nameof(haber)), XafDisplayName("Haber")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Haber
        {
            get { return haber; }
        }
       
        [PersistentAlias(nameof(saldoFin)), XafDisplayName("Saldo Fin Mes")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SaldoFin
        {
            get { return saldoFin; }
        }
        

        [PersistentAlias("[SaldoFin] - [SaldoInicio]"), XafDisplayName("Movimiento Mes")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal MovimientoMes
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(MovimientoMes))); }
        }
        
        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}