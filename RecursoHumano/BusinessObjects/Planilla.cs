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
    [ImageName(nameof(Planilla))]
    [CreatableItem(false)]
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
            if (Empresa != null)
            {
                Parametro pa = Session.FindObject<Parametro>(CriteriaOperator.Parse("[Empresa.Oid] == ?", Empresa.Oid));
                if (pa != null)
                    parametro = pa;
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Parametro parametro;
        Empresa empresa;
        [Persistent(nameof(Tipo))]
        TipoPlanilla tipo;
        Moneda moneda;
        decimal valorMoneda = 1.0m;
        [Persistent(nameof(FechaInicio)), DbType("datetime2")]
        DateTime? fechaInicio;
        [Persistent(nameof(FechaFin)), DbType("datetime2")]
        DateTime? fechaFin;
        [Persistent(nameof(Estado)), DbType("smallint")]
        EEstadoPlanilla estado = EEstadoPlanilla.PreCalculo;
        [Persistent(nameof(FechaPago)), DbType("datetime2")]
        DateTime? fechaPago;


        [XafDisplayName("Empresa"), Persistent(nameof(Empresa))]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }
        [Association("TipoPlanilla-Planillas"), XafDisplayName("Tipo"), PersistentAlias(nameof(tipo))]
        public TipoPlanilla Tipo
        {
            get => tipo;
        }

        [XafDisplayName("Moneda"), Persistent(nameof(Moneda))]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [Persistent(nameof(ValorMoneda)), XafDisplayName("Valor Moneda")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

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