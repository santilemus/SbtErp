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

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos
    /// BO que corresponde al encabezado de la conciliaciones bancarias
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Conciliación"), NavigationItem("Banco"), DefaultProperty("Fecha"), 
        Persistent("BanConciliacion")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoConciliacion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoConciliacion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        BancoCuenta numeroCuenta;
        DateTime fecha;
        DateTime fechaInicio;
        DateTime fechaFin;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado elaboro;
        decimal saldoEstadoCuenta;
        [Persistent(nameof(SaldoLibro)), DbType("numeric(14,2)")]
        decimal saldoLibro;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado autorizo;
        [Persistent(nameof(FechaAutorizo)), DbType("datetime2")]
        DateTime ? fechaAutorizo;


        [Association("BancoCuenta-Conciliaciones"), Index(0), XafDisplayName("Número Cuenta"), VisibleInLookupListView(true)]
        public BancoCuenta NumeroCuenta
        {
            get => numeroCuenta;
            set => SetPropertyValue(nameof(NumeroCuenta), ref numeroCuenta, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha"), Index(1)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Inicio"), Index(2)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Fin"), Index(3)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        [XafDisplayName("Elaboró"), RuleRequiredField("BancoConciliacion.Elaboro_Requerido", DefaultContexts.Save), Index(4)]
        [VisibleInListView(false)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Elaboro
        {
            get => elaboro;
            set => SetPropertyValue(nameof(Elaboro), ref elaboro, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Saldo Estado Cuenta"), Index(5),
            RuleValueComparison("BancoConciliacion.SaldoEstadoCuenta >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SaldoEstadoCuenta
        {
            get => saldoEstadoCuenta;
            set => SetPropertyValue(nameof(SaldoEstadoCuenta), ref saldoEstadoCuenta, value);
        }

        [PersistentAlias(nameof(saldoLibro)), XafDisplayName("Saldo Libros"), Index(6)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SaldoLibro
        {
            get { return saldoLibro; }
        }

        [XafDisplayName("Autorizó"), Index(7), VisibleInListView(false)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Autorizo
        {
            get => autorizo;
            set => SetPropertyValue(nameof(Autorizo), ref autorizo, value);
        }
     
        [PersistentAlias(nameof(fechaAutorizo)), XafDisplayName("Fecha Autorizó"), Index(8), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime ? FechaAutorizo
        {
            get { return fechaAutorizo; }
        }

        #endregion

        #region Colecciones
        [Association("BancoConciliacion-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle")]
        public XPCollection<BancoConciliacionDetalle> Detalles
        {
            get
            {
                return GetCollection<BancoConciliacionDetalle>(nameof(Detalles));
            }
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