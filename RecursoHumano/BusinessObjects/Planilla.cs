using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano.
    /// BO para consulta y procesos relacionados con las planillas generadas en el sistema
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Planillas"), NavigationItem("Recurso Humano"), DefaultProperty("Tipo")]
    [Persistent(nameof(Planilla))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Planilla : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Planilla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (Empresa == null)
                empresa = EmpresaDeSesion();
            if (Empresa != null)
            {
                Parametro pa = Session.FindObject<Parametro>(CriteriaOperator.Parse("[Empresa.Oid] == ?", Empresa.Oid));
                if (pa != null)
                    parametro = pa;
            }
            moneda = ObtenerMonedaBase();
            if (moneda != null)
                valorMoneda = moneda.FactorCambio;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Parametro parametro;
        [Persistent(nameof(Empresa))]
        Empresa empresa = null;
        [Persistent(nameof(Tipo))]
        TipoPlanilla tipo = null;
        [Persistent(nameof(Moneda))]
        Moneda moneda = null;
        [Persistent(nameof(ValorMoneda))]
        decimal valorMoneda = 1.0m;
        [Persistent(nameof(FechaInicio)), DbType("datetime2")]
        DateTime? fechaInicio = null;
        [Persistent(nameof(FechaFin)), DbType("datetime2")]
        DateTime? fechaFin = null;
        [Persistent(nameof(Estado)), DbType("smallint")]
        EEstadoPlanilla estado = EEstadoPlanilla.PreCalculo;
        [Persistent(nameof(FechaPago)), DbType("datetime2")]
        DateTime? fechaPago = null;


        [XafDisplayName("Empresa"), PersistentAlias(nameof(empresa))]
        public Empresa Empresa
        {
            get => empresa;
        }
        [Association("TipoPlanilla-Planillas"), XafDisplayName("Tipo"), PersistentAlias(nameof(tipo))]
        public TipoPlanilla Tipo
        {
            get => tipo;
        }

        [XafDisplayName("Moneda"), PersistentAlias(nameof(moneda))]
        public Moneda Moneda
        {
            get => moneda;
        }

        [PersistentAlias(nameof(valorMoneda)), XafDisplayName("Valor Moneda")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorMoneda => valorMoneda;

        [PersistentAlias(nameof(fechaInicio)), XafDisplayName("Fecha Inicio")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaInicio
        {
            get { return fechaInicio; }
        }

        [PersistentAlias(nameof(fechaFin)), XafDisplayName("Fecha Fin")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaFin
        {
            get => fechaFin;
        }

        [PersistentAlias(nameof(estado)), XafDisplayName("Estado")]
        public EEstadoPlanilla Estado
        {
            get { return estado; }
        }

        [PersistentAlias(nameof(fechaPago)), XafDisplayName("Fecha Pago")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaPago
        {
            get { return fechaPago; }
        }


        [XafDisplayName("Parámetros RRHH")]
        public Parametro Parametro
        {
            get => parametro;
            set => SetPropertyValue(nameof(Parametro), ref parametro, value);
        }
    

        #endregion

        #region Colecciones
        [Association("Planilla-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<PlanillaDetalle> Detalles
        {
            get
            {
                return GetCollection<PlanillaDetalle>(nameof(Detalles));
            }
        }
        #endregion

        #region Metodos
        public void SetEncabezadoDePlanilla(TipoPlanilla ATipo, DateTime AFechaInicio, DateTime AFechaFin, DateTime AFechaPago)
        {
            tipo = ATipo;
            fechaInicio = AFechaInicio;
            fechaFin = AFechaFin;
            fechaPago = AFechaPago;
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

   
}