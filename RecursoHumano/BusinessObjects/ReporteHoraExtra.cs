using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano.
    /// BO para los datos generales (encabezado) del reporte de horas extras por empleado
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Reporte Hora Extra"), NavigationItem("Recurso Humano"), DefaultProperty("Empleado")]
    [Persistent(nameof(ReporteHoraExtra))]
    [ImageName(nameof(ReporteHoraExtra))]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReporteHoraExtra : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReporteHoraExtra(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Estado = EEstadoReporteHoraExtra.Digitado;
            jefeDepartamento = null;
            unidad = null;
            planilla = null;
            fechaPago = null;
        }

        #region Propiedades

        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;
        [Persistent(nameof(Unidad))]
        EmpresaUnidad unidad;
        [Persistent(nameof(JefeDepartamento))]
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado jefeDepartamento;
        DateTime fechaInicio;
        DateTime fechaFin;
        [Persistent(nameof(FechaPago)), DbType("datetime")]
        DateTime? fechaPago;
        [Persistent(nameof(Planilla))]
        int? planilla;
        EEstadoReporteHoraExtra estado = EEstadoReporteHoraExtra.Digitado;

        [XafDisplayName("Empleado")]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set => SetPropertyValue(nameof(Empleado), ref empleado, value);
        }

        [XafDisplayName("Unidad"), PersistentAlias(nameof(unidad))]
        public EmpresaUnidad Unidad
        {
            get => unidad;
        }
        [XafDisplayName("Jefé Departamento"), PersistentAlias(nameof(jefeDepartamento))]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado JefeDepartamento
        {
            get => jefeDepartamento;
        }

        [DbType("datetime2"), XafDisplayName("Fecha Inicio"), Persistent(nameof(FechaInicio))]
        [RuleRequiredField("ReporteHoraExtra.FechaInicio_Requerido", DefaultContexts.Save, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Fin"), Persistent(nameof(FechaFin))]
        [RuleValueComparison("ReporteHoraExtra.FechaFin >= FechaInicio", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "[FechaInicio]",
            ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        [PersistentAlias(nameof(fechaPago)), XafDisplayName("Fecha Pago")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaPago
        {
            get { return fechaPago; }
        }

        /// <summary>
        /// LEEME ==> Cambiar despues el tipo al BO del encabezado de planillas
        /// </summary>
        [XafDisplayName("Planilla"), PersistentAlias(nameof(planilla))]
        public int? Planilla
        {
            get => planilla;
        }

        [DbType("smallint"), XafDisplayName("Estado")]
        public EEstadoReporteHoraExtra Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }
        #endregion

        #region Colecciones
        [Association("ReporteHoraExtra-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<ReporteHoraExtraDetalle> Detalles
        {
            get
            {
                return GetCollection<ReporteHoraExtraDetalle>(nameof(Detalles));
            }
        }

        [Association("ReporteHoraExtra-Resumenes"), DevExpress.Xpo.Aggregated, XafDisplayName("Resumen"), Index(1)]
        public XPCollection<ReporteHoraExtraResumen> Resumenes
        {
            get
            {
                return GetCollection<ReporteHoraExtraResumen>(nameof(Resumenes));
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