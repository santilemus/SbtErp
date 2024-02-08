using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    /// <summary>
    /// BO Correspondiente al Catalogo de cuentas contable
    /// </summary>
    /// <remarks>
    /// PENDIENTE. Si agregamos la columna periodo. La idea de este catalogo es que no sea necesario estar replicando cada año.
    /// Por eso es de evaluar si agregamos el período, para saber en que año se creo la cuenta.
    /// </remarks>
    [DefaultClassOptions, CreatableItem(false)]
    [ImageName("CatalogoContable")]
    [ModelDefault("Caption", "Catálogo Contable"), NavigationItem("Contabilidad"), DefaultProperty(nameof(CodigoCuenta)), Persistent("ConCatalogo"),
        DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None),
        ListViewFilter("Catálogo de la Empresa de la Sesion", "Empresa.Oid = EmpresaActualOid()")]
    [RuleCriteria("Catalogo [Padre] != [Cuenta]", DefaultContexts.Save, "[CodigoCuenta] != [Padre.CodigoCuenta]",
        "Codigo Cuenta debe ser diferente de Padre", TargetCriteria = "!IsNull([Padre])")]
    [RuleCriteria("Catalogo - Cuenta de Mayor", DefaultContexts.Save, "Len([CodigoCuenta]) == 4 && CtaMayor = True && [CtaResumen] == True",
        "Las cuentas de 4 digitdos deben ser De Resumen y de Mayor", ResultType = ValidationResultType.Warning, TargetCriteria = "Len([CodigoCuenta]) == 4")]
    [RuleCombinationOfPropertiesIsUnique("Catalogo. EmpresaCuentaEspecial Unica", DefaultContexts.Save, "Empresa,CodigoCuenta,CuentaEspecial",
        TargetCriteria = "[CuentaEspecial] > 0")]
    [Serializable]

    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Catalogo : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Catalogo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CtaResumen = true;
            CtaMayor = false;
            activa = true;
            CuentaEspecial = 0;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
        }

        #region Propiedades

        string concepto;
        ECuentaEspecial cuentaEspecial;
        Empresa empresa;
        string codigoCuenta;
        string nombre;
        Catalogo padre;
        ETipoCuentaCatalogo tipoCuenta = ETipoCuentaCatalogo.Activo;
        bool ctaResumen;
        bool ctaMayor;
        ETipoSaldoCuenta tipoSaldoCta = ETipoSaldoCuenta.Deudor;
        bool activa;

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("COD_EMP")]
#else
        [DbType("smallint"), Persistent("Empresa")]
#endif
        [Association("Empresa-Catalogos")]
        [XafDisplayName("Empresa"), Index(1), RuleRequiredField("Catalogo.Empresa_Requerida", "Save"), VisibleInListView(false)]
        [Browsable(true), VisibleInReports(true)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

#if Firebird
        [DbType("DM_CODIGO20"), Persistent("COD_CTA_PADRE")]
#else
        [DbType("varchar(20)"), Persistent("CtaPadre")]
#endif
        [Size(20), XafDisplayName("Cuenta Padre"), RuleRequiredField("Catalogo.CuentaPadre_Requerido", "Save", SkipNullOrEmptyValues = true), Index(2)]
        [Association("Padre-Cuentas"), DataSourceCriteria("[CtaResumen] == True && [Activa] == True")]
        [ExplicitLoading(Depth = 1)]
        public Catalogo Padre
        {
            get => padre;
            set
            {
                bool changed = SetPropertyValue(nameof(Padre), ref padre, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    var oldTipoCta = TipoCuenta;
                    var oldTipoSaldoCta = TipoSaldoCta;
                    CodigoCuenta = padre.CodigoCuenta;
                    TipoCuenta = padre.TipoCuenta;
                    TipoSaldoCta = padre.TipoSaldoCta;
                    OnChanged(nameof(TipoCuenta), oldTipoCta, CodigoCuenta);
                    OnChanged(nameof(TipoSaldoCta), oldTipoSaldoCta, TipoSaldoCta);
                }
            }
        }


#if Firebird
        [DbType("DM_CODIGO20"), Persistent("COD_CUENTA")]
#else
        [DbType("varchar(20)"), Persistent(nameof(CodigoCuenta))]
#endif
        [Size(20), XafDisplayName("Código Cuenta"), RuleRequiredField("Catalogo.CodigoCuenta_Requerido", "Save"), Index(3),
            NonCloneable]
        [Indexed(nameof(Empresa), Name = "idxCodigoCuenta_Catalogo", Unique = true)]
        public string CodigoCuenta
        {
            get => codigoCuenta;
            set => SetPropertyValue(nameof(CodigoCuenta), ref codigoCuenta, value);
        }

#if Firebird
        [DbType("DM_DESCRIPCION150"), Persistent("NOMBRE")]
#else
        [DbType("varchar(150)"), Persistent("Nombre")]
#endif
        [Size(150), XafDisplayName("Nombre"), RuleRequiredField("Catalogo.Nombre_Requerido", DefaultContexts.Save), Index(4),
            VisibleInLookupListView(true)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [XafDisplayName("Nivel"), VisibleInLookupListView(false)]
        [PersistentAlias("Iif(!IsNull([Padre]), [Padre].Nivel + 1, 1)")]
        public int Nivel
        {
            get { return Convert.ToInt16(EvaluateAlias("Nivel")); }
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("TIPO_CUENTA")]
#else
        [DbType("smallint"), Persistent("TipoCuenta")]
#endif
        [XafDisplayName("Tipo Cuenta"), Index(6), VisibleInLookupListView(false), RuleRequiredField("Catalogo.TipoCuenta_Requerido", "Save")]
        public ETipoCuentaCatalogo TipoCuenta
        {
            get => tipoCuenta;
            set => SetPropertyValue(nameof(TipoCuenta), ref tipoCuenta, value);
        }

#if Firebird
        [DbType("DM_BOOLEAN"), Persistent("CTA_RESUMEN")]
        [ValueConverter(typeof(ToBooleanDataType))]
#else
        [DbType("bit"), Persistent("CtaResumen")]
#endif
        [XafDisplayName("Cuenta Resumen"), Index(7), VisibleInLookupListView(false), RuleRequiredField("Catalogo.CtaResumen_Requerido", DefaultContexts.Save)]
        public bool CtaResumen
        {
            get => ctaResumen;
            set => SetPropertyValue(nameof(CtaResumen), ref ctaResumen, value);
        }

#if Firebird
        [DbType("DM_BOOLEAN"), Persistent("CTA_MAYOR")]
        [ValueConverter(typeof(ToBooleanDataType))]
#else
        [DbType("bit"), Persistent("CtaMayor")]
#endif
        [XafDisplayName("Cuenta Mayor"), Index(8), VisibleInLookupListView(false), RuleRequiredField("Catalogo.CtaMayor_Requerido", DefaultContexts.Save)]
        //[RuleRequiredField("Catalogo.CtaMayor_Requerido", DefaultContexts.Save, TargetCriteria = "[CtaResumen] == true")]
        [RuleValueComparison("Catalogo.CtaMayor es falso", DefaultContexts.Save, ValueComparisonType.Equals, false,
            TargetCriteria = "[CtaResumen] == false", CustomMessageTemplate = "'{TargetObject}' debe ser False cuando CtaResumen es False")]
        public bool CtaMayor
        {
            get => ctaMayor;
            set => SetPropertyValue(nameof(CtaMayor), ref ctaMayor, value);
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("TIPO_SALDO")]
#else
        [DbType("smallint"), Persistent(nameof(TipoSaldoCta))]
#endif
        [XafDisplayName("Tipo Saldo"), Index(9), RuleRequiredField("Catalogo.TipoSaldo_Requerido", "Save")]
        [VisibleInLookupListView(true)]
        public ETipoSaldoCuenta TipoSaldoCta
        {
            get => tipoSaldoCta;
            set => SetPropertyValue(nameof(TipoSaldoCta), ref tipoSaldoCta, value);
        }

#if Firebird
        [DbType("DM_ENTERO_CORTO"), Persistent("CUENTA_ESPECIAL")]
#else
        [DbType("smallint"), Persistent(nameof(CuentaEspecial))]
#endif
        [XafDisplayName("Cuenta Especial"), Index(10), VisibleInListView(false)]
        [ToolTip("Es cuenta especial, cuando se trata de una cuenta clave para la liquidacion o cierre del ejercicio, Solo son de detalle")]
        [Appearance("Catalogo.CuentaEspecial Visible", AppearanceItemType = "ViewItem", Criteria = "[CtaResumen] == False",
            Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
        public ECuentaEspecial CuentaEspecial
        {
            get => cuentaEspecial;
            set => SetPropertyValue(nameof(CuentaEspecial), ref cuentaEspecial, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Concepto"), VisibleInListView(false), Index(11)]
        [ToolTip("Concepto predefinido cuando se trata de una cuenta especial")]
        [Appearance("Catalogo.CuentaEspecial Concepto Visible", AppearanceItemType = "ViewItem", Criteria = "[CuentaEspecial] > 0",
            Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

#if Firebird
        [DbType("DM_BOOLEAN"), Persistent("ACTIVA")]
        [ValueConverter(typeof(ToBooleanDataType))]
#else
        [DbType("bit"), Persistent("Activa")]
#endif
        [XafDisplayName("Cuenta Activa"), Index(12), VisibleInLookupListView(false), RuleRequiredField("Catalogo.Activa", "Save")]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        #region Colecciones
        [Association("Padre-Cuentas"), XafDisplayName("Cuentas Hijas")]
        public XPCollection<Catalogo> Cuentas
        {
            get
            {
                return GetCollection<Catalogo>(nameof(Cuentas));
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