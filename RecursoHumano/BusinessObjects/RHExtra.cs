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
using SBT.Apps.Empleado.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano.
    /// BO para los datos generales (encabezado) del reporte de horas extras por empleado
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Reporte Hora Extra"), NavigationItem("Recurso Humano"), DefaultProperty("Empleado")]
    [Persistent("PlaRHExtra")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class RHExtra : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RHExtra(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;
        [Persistent(nameof(Unidad))]
        EmpresaUnidad unidad = null;
        [Persistent(nameof(JefeDepartamento))]
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado jefeDepartamento = null;
        DateTime fechaInicio;
        DateTime fechaFin;
        [Persistent(nameof(FechaPago)), DbType("datetime")]
        DateTime ? fechaPago = null;
        [Persistent(nameof(Planilla))]
        int ? planilla = null;
        EEstadoRHExtra estado = EEstadoRHExtra.Digitado;

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
        [RuleRequiredField("ReporteHExtra.FechaInicio_Requerido", DefaultContexts.Save, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Fin"), Persistent(nameof(FechaFin))]
        [RuleValueComparison("ReporteHExtra.FechaFin >= FechaInicio", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "[FechaInicio]",
            ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        [PersistentAlias(nameof(fechaPago)), XafDisplayName("Fecha Pago")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime ? FechaPago
        {
            get { return fechaPago; }
        }
        
        /// <summary>
        /// LEEME ==> Cambiar despues el tipo al BO del encabezado de planillas
        /// </summary>
        [XafDisplayName("Planilla"), PersistentAlias(nameof(planilla))]
        public int ? Planilla
        {
            get => planilla;
        }

        [DbType("smallint"), XafDisplayName("Estado"), RuleRequiredField("ReporteHExtra.Estado_Requerido", "Save")]
        public EEstadoRHExtra Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }
        #endregion

        #region Colecciones
        [Association("RHExtra-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<RHExtraDetalle> Detalles
        {
            get
            {
                return GetCollection<RHExtraDetalle>(nameof(Detalles));
            }
        }

        [Association("RHExtra-Resumenes"), DevExpress.Xpo.Aggregated, XafDisplayName("Resumen"),Index(1)]
        public XPCollection<RHExtraResumen> Resumenes
        {
            get
            {
                return GetCollection<RHExtraResumen>(nameof(Resumenes));
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