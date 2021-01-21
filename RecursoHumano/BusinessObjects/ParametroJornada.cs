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

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// BO para la parametrización de las jornadas de trabajo y el porcentaje de las horas extras. La parametrización es por empresa
    /// </summary>
    /// <remarks>
    /// Al igual que los otros BO, donde existe la propiedad empresa, se debe filtrar por defecto la informacion a la empresa de la sesion
    /// </remarks>
    [DefaultClassOptions, ModelDefault("Caption", "Parámetros Jornadas"), NavigationItem("Recurso Humano"), Persistent(nameof(ParametroJornada)), 
        DefaultProperty("Tipo")]
    [ImageName("ParametroJornada")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ParametroJornada : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ParametroJornada(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Empresa empresa;
        Listas tipo;
        decimal horasJornada;
        decimal minHorasJornada;
        decimal horasSemana;
        decimal porcentajeHExtra;
        DateTime horaFin;
        DateTime horaInicio;
        bool activo = true;

        [Persistent("Empresa"), DbType("int"), XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }


        [Size(12), DbType("varchar(12)"), Persistent("Tipo"), XafDisplayName("Tipo TipoJornada"),
            RuleRequiredField("ParametroJornada.Tipo_Requerido", DefaultContexts.Save)]
        [DataSourceCriteria("Categoria = 21")]
        public Listas Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("numeric(5,2)"), Persistent("HorasJornada"), XafDisplayName("Horas TipoJornada")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal HorasJornada
        {
            get => horasJornada;
            set => SetPropertyValue(nameof(HorasJornada), ref horasJornada, value);
        }

        [DbType("numeric(5,2)"), Persistent("MinHorasJornada"), XafDisplayName("Mínimo de Horas")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleRange("ParametroJornada.MinHorasJornada > 0", DefaultContexts.Save, "1", "HorasJornada", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        public decimal MinHorasJornada
        {
            get => minHorasJornada;
            set => SetPropertyValue(nameof(MinHorasJornada), ref minHorasJornada, value);
        }

        [DbType("numeric(5,2)"), Persistent("HorasSemana"), XafDisplayName("Horas Semana")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("ParametroJornada.HoraSemana > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        public decimal HorasSemana
        {
            get => horasSemana;
            set => SetPropertyValue(nameof(HorasSemana), ref horasSemana, value);
        }

        [DbType("numeric(5,2)"), Persistent("PorcentajeHExtra"), XafDisplayName("Porcentaje Hora Extra")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("ParametroJornada.Porcentaje >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public decimal PorcentajeHExtra
        {
            get => porcentajeHExtra;
            set => SetPropertyValue(nameof(PorcentajeHExtra), ref porcentajeHExtra, value);
        }

        [DbType("datetime"), Persistent("HoraInicio"), XafDisplayName("Hora Inicio"),
            RuleRequiredField("ParametroJornada.HoraInicio_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:HH:mm:ss}"), ModelDefault("EditMask", "{0:HH:mm:ss}")]
        public DateTime HoraInicio
        {
            get => horaInicio;
            set => SetPropertyValue(nameof(HoraInicio), ref horaInicio, value);
        }

        [DbType("datetime"), Persistent("HoraFin"), XafDisplayName("Hora Fin"),
            RuleRequiredField("ParametroJornada.HoraFin_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:HH:mm:ss}"), ModelDefault("EditMask", "{0:HH:mm:ss}")]
        public DateTime HoraFin
        {
            get => horaFin;
            set => SetPropertyValue(nameof(HoraFin), ref horaFin, value);
        }

        [DbType("bit"), Persistent("Activo"), XafDisplayName("Activo"), RuleRequiredField("ParametroJornada.Activo_Requerido", DefaultContexts.Save)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }
        #endregion

        #region Colecciones
        [Association("ParametroJornada-TipoJornadas"), XafDisplayName("Reporte HExtra Detalle"), Index(0)]
        public XPCollection<ReporteHoraExtraDetalle> TipoJornadas
        {
            get
            {
                return GetCollection<ReporteHoraExtraDetalle>(nameof(TipoJornadas));
            }
        }

        [Association("ParamJornada-Resumenes"), XafDisplayName("RHExtra Resumenes"), Index(1)]
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