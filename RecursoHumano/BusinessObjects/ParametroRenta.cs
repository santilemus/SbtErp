using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recursos Humanos. BO para la parametrizacion de las tablas de calculo de la retencion de renta
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Tablas ISR"), DefaultProperty("ID"), VisibleInDashboards(false),
        DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None), Persistent("PlaParametroRenta"), NavigationItem("Recurso Humano")]
    [ImageName("ParametroRenta")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ParametroRenta : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ParametroRenta(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TipoTabla = ETablaISR.Mensual;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        ZonaGeografica pais;

#if (Firebird)
        [DbType("DM_CODIGO07"), Persistent("COD_PAIS")]
#else
        [DbType("varchar(7)"), Persistent(nameof(Pais))]
#endif
        //[Association("Pais-ParametroRentas", "Sanrey.Erp.Base", "Sanrey.Erp.Base.BusinessObjects.ZonaGeografica")]
        [Size(7), XafDisplayName("País"), Index(1), RuleRequiredField("ParametroRenta.Pais_Requerido", DefaultContexts.Save)] 
        [DataSourceCriteria("[ZonaPadre] Is Null And [Activa] = true")]
        public ZonaGeografica Pais
        {
            get => pais;
            set => SetPropertyValue(nameof(Pais), ref pais, value);
        }


        ETablaISR tipoTabla;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("TIPO_TABLA")]
#else
        [DbType("smallint"), Persistent(nameof(TipoTabla))]
#endif
        [XafDisplayName("Tipo Tabla"), Index(2), RuleRequiredField("ParametroRenta.TipoTabla_Requerido", DefaultContexts.Save)]
        public ETablaISR TipoTabla
        {
            get => tipoTabla;
            set => SetPropertyValue(nameof(TipoTabla), ref tipoTabla, value);
        }

        decimal sueldoDesde;
#if (Firebird)
        [DbType("DM_DINERO124"), Persistent("SUELDO_DESDE")]
#else
        [DbType("money"), Persistent(nameof(SueldoDesde))]
#endif
        [XafDisplayName("Sueldo Desde"), RuleValueComparison("ParametroRenta.SueldoDesde >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, 
            SkipNullOrEmptyValues = false), Index(4), ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal SueldoDesde
        {
            get => sueldoDesde;
            set => SetPropertyValue(nameof(SueldoDesde), ref sueldoDesde, value);
        }

        decimal sueldoHasta;
#if (Firebird)
        [DbType("DM_DINERO124"), Persistent("SUELDO_HASTA")]
#else
        [DbType("money"), Persistent(nameof(SueldoHasta))]
#endif
        [XafDisplayName("Sueldo Hasta"), RuleValueComparison("ParametroRenta.SueldoHasta > SueldoDesde", DefaultContexts.Save, ValueComparisonType.GreaterThan, 
            ParametersMode.Expression, "SueldoDesde", SkipNullOrEmptyValues = false), Index(5), ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal SueldoHasta
        {
            get => sueldoHasta;
            set => SetPropertyValue(nameof(SueldoHasta), ref sueldoHasta, value);
        }

        decimal rentaFija;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("MONTO_RETENC")]
#else
        [DbType("money"), Persistent(nameof(RentaFija))]
#endif
        [XafDisplayName("Renta Fija"), RuleValueComparison("ParametroRenta.RentaFija >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,0, 
            SkipNullOrEmptyValues = false), Index(6), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal RentaFija
        {
            get => rentaFija;
            set => SetPropertyValue(nameof(RentaFija), ref rentaFija, value);
        }

        decimal porcentaje;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("PORCENT_RENTA")]
#else
        [DbType("money"), Persistent(nameof(Porcentaje))]
#endif
        [XafDisplayName("Porcentaje"), RuleValueComparison("ParametroRenta.Porcentaje >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = false), Index(7), ModelDefault("DisplayFormat", "{0:P2}"), ModelDefault("EditMask", "{0:P2}")]
        public decimal Porcentaje
        {
            get => porcentaje;
            set => SetPropertyValue(nameof(Porcentaje), ref porcentaje, value);
        }

        decimal limite;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("LIM_APLICAR_PORC")]
#else
        [DbType("money"), Persistent(nameof(Limite))]
#endif
        [XafDisplayName("Sobre Límite De"), RuleValueComparison("ParametroRenta.Limite >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0,
            SkipNullOrEmptyValues = false), Index(7), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Limite
        {
            get => limite;
            set => SetPropertyValue(nameof(Limite), ref limite, value);
        }

        #endregion

        #region Colecciones


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}