using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano.
    /// BO para el detalle del reporte de horas extras por empleado
    /// </summary>
    [ModelDefault("Caption", "HExtra Detalle"), NavigationItem(false), DefaultProperty("FechaTrabajo"), CreatableItem(false)]
    [Persistent(nameof(ReporteHoraExtraDetalle))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReporteHoraExtraDetalle : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReporteHoraExtraDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        ReporteHoraExtra reporteHoraExtra;
        ParametroJornada tipoJornada;
        DateTime fechaTrabajo;
        string descripcion;
        EmpresaUnidad unidadSolicita;
        DateTime horaDesde;
        DateTime horaHasta;

        [Association("ReporteHoraExtra-Detalles"), XafDisplayName("Reporte Hora Extra")]
        public ReporteHoraExtra ReporteHoraExtra
        {
            get => reporteHoraExtra;
            set => SetPropertyValue(nameof(ReporteHoraExtra), ref reporteHoraExtra, value);
        }

        [Association("ParametroJornada-TipoJornadas"), XafDisplayName("Tipo Jornada")]
        [RuleRequiredField("ReporteHoraExtraDetalle.Jornada_Requerido", "Save")]
        public ParametroJornada TipoJornada
        {
            get => tipoJornada;
            set => SetPropertyValue(nameof(TipoJornada), ref tipoJornada, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Trabajo")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]

        public DateTime FechaTrabajo
        {
            get => fechaTrabajo;
            set => SetPropertyValue(nameof(FechaTrabajo), ref fechaTrabajo, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Descripción")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }
        [XafDisplayName("Unidad Solicita")]
        public EmpresaUnidad UnidadSolicita
        {
            get => unidadSolicita;
            set => SetPropertyValue(nameof(UnidadSolicita), ref unidadSolicita, value);
        }

        [DbType("datetime2"), XafDisplayName("Hora Desde"), Persistent(nameof(HoraDesde))]
        [RuleRequiredField("ReporteHoraExtraDetalle.HoraDesde_Requerido", DefaultContexts.Save, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:T}"), ModelDefault("EditMask", "t")]
        public DateTime HoraDesde
        {
            get => horaDesde;
            set => SetPropertyValue(nameof(HoraDesde), ref horaDesde, value);
        }

        [DbType("datetime2"), XafDisplayName("Hora Hasta"), Persistent(nameof(HoraHasta))]
        [RuleValueComparison("ReporteHoraExtraDetalle.HoraHasta > HoraDesde", DefaultContexts.Save, ValueComparisonType.GreaterThan,
            "[HoraDesde]", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:HH:mm:ss}"), ModelDefault("EditMask", "t")]
        public DateTime HoraHasta
        {
            get => horaHasta;
            set => SetPropertyValue(nameof(HoraHasta), ref horaHasta, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}