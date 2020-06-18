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
    /// Recurso Humano. BO para los parametros generales del modulo de recursos humanos
    /// *** LEEME ==> Ver la ultima implementacion Sanrey, faltan columnas
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Parámetros RRHH"), DefaultProperty("Empresa"), VisibleInDashboards(false),
        DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None), Persistent("PlaParametroEmpresa"), NavigationItem("Recurso Humano")]
    [ImageName("ParametroRRHH")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Parametro : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Parametro(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Empresa = EmpresaDeSesion();
            DiasLicMatrimonio = 0;
            DiasLicMuerte = 0;
            DiasLicNatilidad = 0;
            DiasMaxLicencia = 0;
            DiasVacacion = 15;
            MinDiasAguinaldoProp = 1;
            MinDiasAguinaldoComp = 365;
            MinDiasTrabVacacion = 365;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        Empresa empresa;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("COD_EMP")]
#else
        [DbType("smallint"), Persistent("Empresa")]
#endif
        //[Association("Empresa-Parametros")]
        [XafDisplayName("Empresa"), RuleRequiredField("Parametro.Empresa_Requerido", DefaultContexts.Save), Index(0)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public Empresa Empresa
        {
            get
            {
                return empresa;
            }
            set
            {
                SetPropertyValue("Empresa", ref empresa, value);
            }
        }

        decimal sueldoMinimo;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("SUELDO_MINIMO")]
#else
        [DbType("money"), Persistent("SueldoMinMensual")]
#endif
        [XafDisplayName("Sueldo Mínimo"), ToolTip("Sueldo Mínimo mensual, de acuerdo a la legislación laboral vigente"), 
            RuleValueComparison("Parametro.SueldoMinimo > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false),
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), Index(1)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public decimal SueldoMinimo
        {
            get
            {
                return sueldoMinimo;
            }
            set
            {
                SetPropertyValue("SueldoMinimo", ref sueldoMinimo, value);
            }
        }

        decimal minimoDiario;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("SUELDO_MIN_DIARIO")]
#else
        [DbType("money"), Persistent("SueldoMinDiario")]
#endif
        [XafDisplayName("Sueldo Mínimo Diario"), ToolTip("Sueldo Mínimo diario, de acuerdo a la legislación laboral vigente"),
            RuleValueComparison("Parametro.MinimoDiario > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false),
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), Index(2)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public decimal MinimoDiario
        {
            get
            {
                return minimoDiario;
            }
            set
            {
                SetPropertyValue("MinimoDiario", ref minimoDiario, value);
            }
        }

        decimal salarioMaxSeguro;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("MAX_SAL_SSOCIAL")]
#else
        [DbType("money"), Persistent("MaxSalSSocial")]
#endif
        [XafDisplayName("Máximo Cotizable Seguro"), ToolTip("Salario Máximo Cotizable vigente, para el seguro social"),
            RuleValueComparison("Parametro.SalarioMaxSeguro >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false),
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), Index(8)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public decimal SalarioMaxSeguro
        {
            get
            {
                return salarioMaxSeguro;
            }
            set
            {
                SetPropertyValue("SalarioMaxSeguro", ref salarioMaxSeguro, value);
            }
        }

        decimal salarioMaxPrevision;
#if (Firebird)
        [DbType("DM_DINERO122"), Persistent("MAX_SAL_PENSION")]
#else
        [DbType("money"), Persistent("MaxSalPension")]
#endif
        [XafDisplayName("Máximo Cotizable Previsional"), ToolTip("Salario Máximo Cotizable para el fondo previsional (pensiones)"),
            RuleValueComparison("Parametro.SalarioMaxPrevision >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false),
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), Index(9)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public decimal SalarioMaxPrevision
        {
            get
            {
                return salarioMaxPrevision;
            }
            set
            {
                SetPropertyValue("SalarioMaxPrevision", ref salarioMaxPrevision, value);
            }
        }

        decimal porcSeguroEmpleado;
#if (Firebird)
        [DbType("numeric(6,3)"), Persistent("PORC_SSOCIAL_EMPLE")]
#else
        [DbType("numeric(6,3)"), Persistent("PorcSSocialEmple")]
#endif
        [XafDisplayName("Porcentaje Seguro Empleado"), ToolTip("Porcentaje de aporte del empleado al Seguro Social"),
            RuleValueComparison("Parametro.PorcSeguroEmpleado >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false),
            ModelDefault("DisplayFormat", "{0:N3}"), ModelDefault("EditMask", "n3"), Index(10)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public decimal PorcSeguroEmpleado
        {
            get
            {
                return porcSeguroEmpleado;
            }
            set
            {
                SetPropertyValue("PorcSeguroEmpleado", ref porcSeguroEmpleado, value);
            }
        }

        decimal porcSeguroPatrono;
#if (Firebird)
        [DbType("numeric(6,3)"), Persistent("PORC_SSOCIAL_PATRONO")]
#else
        [DbType("numeric(6,3)"), Persistent("PorcSSocialPatrono")]
#endif
        [XafDisplayName("Porcentaje Seguro Patrono"), ToolTip("Porcentaje vigente del aporte patronal al seguro social"),
            RuleValueComparison("Parametro.PorcSeguroPatrono >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false),
            ModelDefault("DisplayFormat", "{0:N3}"), ModelDefault("EditMask", "n3"), Index(11)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public decimal PorcSeguroPatrono
        {
            get
            {
                return porcSeguroPatrono;
            }
            set
            {
                SetPropertyValue("PorcSeguroPatrono", ref porcSeguroPatrono, value);
            }
        }

        int minDiasTrabVacacion;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("MIN_DIAS_TRABV")]
#else
        [DbType("smallint"), Persistent("MinDiasTrabVac")]
#endif
        [XafDisplayName("Mín. Días Trabajados Vacación Completa"), ToolTip("Mínimo de días trabajados para tener derecho a vacación completa"),
            RuleValueComparison("Parametro.MinDiasTrabVacacion >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false), 
            Index(12), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public int MinDiasTrabVacacion
        {
            get
            {
                return minDiasTrabVacacion;
            }
            set
            {
                SetPropertyValue("MinDiasTrabVacacion", ref minDiasTrabVacacion, value);
            }
        }

        int diasVacacion;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("DIAS_VACA")]
#else
        [DbType("smallint"), Persistent("DiasVacacion")]
#endif
        [XafDisplayName("Días Vacación"), ToolTip("Días de vacación a los cuales tiene derecho el empleado"),
            RuleValueComparison("Parametro.DiasVacacion >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false), 
            Index(13), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Left, LayoutGroupType.SimpleEditorsGroup)]
        public int DiasVacacion
        {
            get
            {
                return diasVacacion;
            }
            set
            {
                SetPropertyValue("DiasVacacion", ref diasVacacion, value);
            }
        }

        DateTime ? fechaCalcAguinaldo;
#if (Firebird)
        [DbType("DM_FECHA"), Persistent("FECHA_CALC_AGUI")]
#else
        [DbType("datetime"), Persistent("FechaCalcAguinaldo")]
#endif
        [XafDisplayName("Fecha Cálculo Aguinaldo"), ToolTip("Fecha límite para calcular el aguinaldo"), Index(15),
            VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public DateTime ? FechaCalcAguinaldo
        {
            get
            {
                return fechaCalcAguinaldo;
            }
            set
            {
                SetPropertyValue("FechaCalcAguinaldo", ref fechaCalcAguinaldo, value);
            }
        }

       // NOTA: Para el aguinaldo hacer una tabla (porcentajes) y ver si se incluye alli la fecha de calculo o se borra
       //       BORRAR, porcentaje de vacacion, dias de vacacion y otros parametros relacionados y usar una tabla como
       //       lo ultimo que se hizo para el IGD

        int minDiasAguinaldoProp;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("MIN_DIAS_AGUIP")]
#else
        [DbType("smallint"), Persistent("MinDiasAguiProp")]
#endif
        [XafDisplayName("Mínimo Días Aguinaldo Proporcional"), ToolTip("Mínimo de días que debe laborar el empleado para tener derecho al aguinaldo proporcional"),
            RuleValueComparison("Parametro.MinDiasAguinaldoProp >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false), 
            Index(17), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int MinDiasAguinaldoProp
        {
            get
            {
                return minDiasAguinaldoProp;
            }
            set
            {
                SetPropertyValue("MinDiasAguinaldoProp", ref minDiasAguinaldoProp, value);
            }
        }

        int minDiasAguinaldoComp;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("MIN_DIAS_AGUIC")]
#else
        [DbType("smallint"), Persistent("MinDiasAguiComp")]
#endif
        [XafDisplayName("Mínimo Días Aguinaldo Completo"), ToolTip("Mínimo de días que debe laborar el empleado para tener derecho al aguinaldo completo"),
            RuleValueComparison("Parametro.MinDiasAguinaldoComp >= MinDiasAguinaldoProp", DefaultContexts.Save, 
                ValueComparisonType.GreaterThanOrEqual, ParametersMode.Expression, "MinDiasAguinaldoProp", SkipNullOrEmptyValues = false), Index(18),
            VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int MinDiasAguinaldoComp
        {
            get
            {
                return minDiasAguinaldoComp;
            }
            set
            {
                SetPropertyValue("MinDiasAguinaldoComp", ref minDiasAguinaldoComp, value);
            }
        }

        decimal porcentajeBono;
#if (Firebird)
        [DbType("numeric(6,3)"), Persistent("PORC_BONIF")]
#else
        [DbType("numeric(6,3)"), Persistent("PorcBono")]
#endif
        [XafDisplayName("Porcentaje Bono"), ToolTip("Porcentaje del salario de un mes al cual corresponde el monto del bono"), 
            RuleValueComparison("Parametro.PorcentajeBono >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true),
            ModelDefault("DisplayFormat", "{0:N3}"), ModelDefault("EditMask", "n3"), Index(19), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public decimal PorcentajeBono
        {
            get
            {
                return porcentajeBono;
            }
            set
            {
                SetPropertyValue("PorcentajeBono", ref porcentajeBono, value);
            }
        }

        DateTime fechaInicioBono;
#if (Firebird)
        [DbType("DM_FECHA"), Persistent("FECHA_INI_BONIF")]
#else
        [DbType("datetime"), Persistent("FechaInicioBono")]
#endif
        [XafDisplayName("Fecha Inicio Bono"), ToolTip("Fecha de inicio del período de calculo del bono"), Index(20),
            VisibleInListView(false), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public DateTime FechaInicioBono
        {
            get
            {
                return fechaInicioBono;
            }
            set
            {
                SetPropertyValue("FechaInicioBono", ref fechaInicioBono, value);
            }
        }

        DateTime fechaCalcBono;
#if (Firebird)
        [DbType("DM_FECHA"), Persistent("FECHA_CALC_BONIF")]
#else
        [DbType("datetime"), Persistent("FechaCalBono")]
#endif
        [XafDisplayName("Fecha Calculo Bono"), ToolTip("Fecha de calculo del bono"),
            RuleValueComparison("FechaCalcBono >= FechaInicioBono", DefaultContexts.Save, ValueComparisonType.GreaterThan, ParametersMode.Expression, 
            "FechaInicioBono", SkipNullOrEmptyValues = true), Index(21), VisibleInListView(false), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public DateTime FechaCalcBono
        {
            get
            {
                return fechaCalcBono;
            }
            set
            {
                SetPropertyValue("FechaCalcBono", ref fechaCalcBono, value);
            }
        }

        int minDiasBonoProp;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("MIN_DIAS_BONIFP")]
#else
        [DbType("smallint"), Persistent("MinDiasBonoProp")]
#endif
        [XafDisplayName("Mínimo Días Bono Proporcional"), ToolTip("Mínimo de días que debe laborar el empleado para tener derecho al bono proporcional"),
            RuleValueComparison("Parametro.MinDiasBonoProp >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false), 
            Index(22), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int MinDiasBonoProp
        {
            get
            {
                return minDiasBonoProp;
            }
            set
            {
                SetPropertyValue("MinDiasBonoProp", ref minDiasBonoProp, value);
            }
        }

        int minDiasBonoComp;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("MIN_DIAS_BONIFC")]
#else
        [DbType("smallint"), Persistent("MinDiasBonoCompleto")]
#endif
        [XafDisplayName("Mínimo Días Bono Completo"), ToolTip("Mínimo de días que debe laborar el empleado para tener derecho al bono completo"),
            RuleValueComparison("Parametro.MinDiasBonoComp >= MinDiasBonoProp", DefaultContexts.Save,
                ValueComparisonType.GreaterThanOrEqual, ParametersMode.Expression, "MinDiasBonoProp", SkipNullOrEmptyValues = false), Index(23),
            VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int MinDiasBonoComp
        {
            get
            {
                return minDiasBonoComp;
            }
            set
            {
                SetPropertyValue("MinDiasBonoComp", ref minDiasBonoComp, value);
            }
        }

        int noAusencias;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("NUM_AUSENCIAS")]
#else
        [DbType("smallint"), Persistent("NoAusencias")]
#endif
        [XafDisplayName("No Ausencias"), ToolTip("Número de inasistencias no justificadas por mes"), 
            RuleValueComparison("Parametro.NoAusencias >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true), 
            Index(24), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int NoAusencias
        {
            get
            {
                return noAusencias;
            }
            set
            {
                SetPropertyValue("NoAusencias", ref noAusencias, value);
            }
        }

        int diasMaxLicencia;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("DIAS_MAX_LICGS")]
#else
        [DbType("smallint"), Persistent("DiasMaxLicenciaGS")]
#endif
        [XafDisplayName("Días Máximo Lic. Con Sueldo"), ToolTip("Número máximo de días de licencia con goce de sueldo"),
            RuleValueComparison("Parametro.DiasMaxLicencia >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true), 
            Index(25), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int DiasMaxLicencia
        {
            get
            {
                return diasMaxLicencia;
            }
            set
            {
                SetPropertyValue("DiasMaxLicencia", ref diasMaxLicencia, value);
            }
        }

        int diasLicMatrimonio;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("DIAS_LIC_MATRIM")]
#else
        [DbType("smallint"), Persistent("DiasLicMatrimonio")]
#endif
        [XafDisplayName("Días Máximo Lic. Matrimonio"), ToolTip("Número máximo de Días de licencia por matrimonio"),
            RuleValueComparison("Parametro.DiasLicMatrimonio >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true), 
            Index(26), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int DiasLicMatrimonio
        {
            get
            {
                return diasLicMatrimonio;
            }
            set
            {
                SetPropertyValue("DiasLicMatrimonio", ref diasLicMatrimonio, value);
            }
        }

        int diasLicMuerte;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("DIAS_LIC_MUERTE")]
#else
        [DbType("smallint"), Persistent("DiasLicMuerte")]
#endif
        [XafDisplayName("Días Lic. Muerte"), ToolTip("Días de licencia por muerte de un pariente"),
            RuleValueComparison("Parametro.DiasLicMuerte >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true), 
            Index(27), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int DiasLicMuerte
        {
            get
            {
                return diasLicMuerte;
            }
            set
            {
                SetPropertyValue("DiasLicMuerte", ref diasLicMuerte, value);
            }
        }

        int diasLicNatilidad;
#if (Firebird)
        [DbType("DM_ENTERO_CORTO"), Persistent("DIAS_LIC_NATAL")]
#else
        [DbType("smallint"), Persistent("DiasLicNatalidad")]
#endif
        [XafDisplayName("Días Lic. Muerte"), ToolTip("Días de licencia por natalidad"),
            RuleValueComparison("Parametro.DiasLicNatalidad >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true), 
            Index(28), VisibleInListView(false), VisibleInLookupListView(false)]
        [DetailViewLayout(LayoutColumnPosition.Right, LayoutGroupType.SimpleEditorsGroup)]
        public int DiasLicNatilidad
        {
            get
            {
                return diasLicNatilidad;
            }
            set
            {
                SetPropertyValue("DiasLicNatilidad", ref diasLicNatilidad, value);
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