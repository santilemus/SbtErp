using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para el registro de las transacciones de caja chica" 
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Caja Chica - Transacciones"), NavigationItem("Banco"), Persistent(nameof(CajaChicaTransaccion)),
        DefaultProperty(nameof(FechaInicio))]
    [ImageName(nameof(CajaChicaTransaccion))]
    [CreatableItem(false)]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CajaChicaTransaccion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CajaChicaTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            fechaLiquidacion = null;
            bancoTransaccion = null;
            fechaLiquidacion = null;
            observaciones = null;
            valesPendientes = 0.0m;
            fechaFin = null;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        CajaChica cajaChica;
        Moneda moneda;
        [DbType("money"), Persistent(nameof(ValorMoneda))]
        decimal valorMoneda = 1.0m;
        DateTime fechaInicio;
        DateTime? fechaFin;
        decimal valesPendientes;
        string observaciones;
        [Persistent(nameof(Responsable))]
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado responsable;
        [Persistent(nameof(FechaLiquidacion)), DbType("datetime")]
        DateTime? fechaLiquidacion;
        [Persistent(nameof(BancoTransaccion))]
        BancoTransaccion bancoTransaccion;
        [Persistent(nameof(MinimoDisponible)), DbType("money")]
        decimal minimoDisponible;
        [Persistent(nameof(MaximoGasto)), DbType("money")]
        decimal maximoGasto;
        [Persistent(nameof(MontoFondo)), DbType("money")]
        decimal montoFondo;

        [Association("CajaChica-Transacciones"), ImmediatePostData(true), Index(0)]
        public CajaChica CajaChica
        {
            get => cajaChica;
            set
            {
                var modificado = SetPropertyValue(nameof(CajaChica), ref cajaChica, value);

                if (!IsLoading && !IsSaving && modificado)
                {
                    montoFondo = value.MontoFondo;
                    maximoGasto = value.MaximoGasto;
                    minimoDisponible = value.MinimoDisponible;
                    responsable = value.Responsable;
                }
            }
        }

        [DbType("varchar(3)"), Persistent("Moneda"), XafDisplayName("Moneda"), RuleRequiredField("CajaChicaTransaccion.Moneda_Requerido", "Save")]
        [ImmediatePostData(true), Index(1)]
        public Moneda Moneda
        {
            get => moneda;
            set
            {
                var modificado = SetPropertyValue(nameof(Moneda), ref moneda, value);
                if (!IsLoading && !IsSaving && modificado)
                    valorMoneda = value.FactorCambio;
            }
        }

        [PersistentAlias("valorMoneda"), XafDisplayName("Valor Moneda"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [Index(2), VisibleInListView(false)]
        public decimal ValorMoneda
        {
            get => valorMoneda;
        }

        [DbType("datetime"), Persistent("FechaInicio"), XafDisplayName("Fecha Inicio"), Index(3)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        [DbType("datetime"), Persistent("FechaFin"), XafDisplayName("Fecha Fin")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        [RuleValueComparison("CajaChicaTransaccion.FechaFin >= FechaInicio", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "FechaInicio", ParametersMode.Expression, SkipNullOrEmptyValues = true), Index(4)]
        public DateTime? FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        [DbType("money"), Persistent("ValesPendientes"), XafDisplayName("Vales Pendientes"), Index(5)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("CajaChicaTransaccion.ValesPendientes >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true)]
        public decimal ValesPendientes
        {
            get => valesPendientes;
            set => SetPropertyValue(nameof(ValesPendientes), ref valesPendientes, value);
        }

        [Size(150), DbType("varchar(150)"), Persistent("Observaciones"), XafDisplayName("Observaciones"), Index(6),
            VisibleInListView(false)]
        public string Observaciones
        {
            get => observaciones;
            set => SetPropertyValue(nameof(Observaciones), ref observaciones, value);
        }

        [PersistentAlias(nameof(fechaLiquidacion)), XafDisplayName("Fecha LIquidación"), Index(7), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaLiquidacion
        {
            get { return fechaLiquidacion; }
        }

        [PersistentAlias("responsable"), XafDisplayName("Responsable"), VisibleInLookupListView(true), Index(8)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Responsable
        {
            get => responsable;
        }

        [XafDisplayName("Monto Fondo"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [PersistentAlias(nameof(montoFondo)), Index(9), VisibleInListView(false)]
        public decimal MontoFondo
        {
            get { return montoFondo; }
        }

        [XafDisplayName("Gasto Máximo"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [PersistentAlias(nameof(maximoGasto)), Index(10), VisibleInListView(false)]
        public decimal MaximoGasto
        {
            get { return maximoGasto; }
        }

        [XafDisplayName("Mínimo Disponible"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [PersistentAlias(nameof(minimoDisponible)), Index(11), VisibleInListView(false)]
        public decimal MinimoDisponible
        {
            get { return minimoDisponible; }
        }

        [PersistentAlias(nameof(bancoTransaccion)), XafDisplayName("Banco Transacción"), Index(12), VisibleInListView(false)]
        public BancoTransaccion BancoTransaccion
        {
            get => bancoTransaccion;
        }

        [PersistentAlias("[BancoTransaccion.ChequeNo]"), XafDisplayName("Cheque No"), Index(12), VisibleInListView(false)]
        public int ChequeNo
        {
            get { return Convert.ToInt32(EvaluateAlias(nameof(ChequeNo))); }
        }


        #endregion

        #region Colecciones
        [Association("CajaChicaTransaccion-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles")]
        public XPCollection<CajaChicaTransaccionDetalle> Detalles
        {
            get
            {
                return GetCollection<CajaChicaTransaccionDetalle>(nameof(Detalles));
            }
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}