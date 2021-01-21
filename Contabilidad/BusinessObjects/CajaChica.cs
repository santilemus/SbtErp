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

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para definir la parametrizacion de la caja chica
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Parámetros Caja Chica"), NavigationItem("Banco"), Persistent("BanCajaChica")]
    [RuleCombinationOfPropertiesIsUnique("CajaChica_EmpresaCodigo_Unico", DefaultContexts.Save, "Empresa,Codigo",
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
    [CreatableItem(false), DefaultProperty(nameof(Codigo))]
    [ImageName(nameof(CajaChica))]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CajaChica : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CajaChica(Session session)
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
        string codigo;
        Moneda moneda;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado responsable;
        decimal montoFondo;
        decimal maximoGasto;
        decimal minimoDisponible;
        bool activa = true;

        [Persistent("Empresa"), DbType("int"), XafDisplayName("Empresa"), Browsable(false), Index(0)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }


        [Size(6), DbType("varchar(6)"), Persistent("Codigo"), XafDisplayName("Código Caja"),
            RuleRequiredField("CajaChica.Codigo_Requerido", DefaultContexts.Save), Index(1)]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }


        [DbType("varchar(3)"), Persistent("Moneda"), XafDisplayName("Moneda"), RuleRequiredField("CajaChica.Moneda_Requerido", "Save")]
        [Index(2)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }


        [Persistent("Responsable"), XafDisplayName("Responsable"), Index(3), VisibleInLookupListView(true),
            RuleRequiredField("CajaChica.Responsable_Requerido", "Save")]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Responsable
        {
            get => responsable;
            set => SetPropertyValue(nameof(Responsable), ref responsable, value);
        }

        [DbType("money"), Persistent("MontoFondo"), XafDisplayName("Monto del Fondo"), Index(4),
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"),
            RuleValueComparison("CajaChica.MontoFondo > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        public decimal MontoFondo
        {
            get => montoFondo;
            set => SetPropertyValue(nameof(MontoFondo), ref montoFondo, value);
        }

        [DbType("money"), Persistent("MaximoGasto"), XafDisplayName("Gasto Máximo"), Index(5),
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"),
            RuleRange("CajaChica.MaximoGasto > 0 y <= MontoFondo", DefaultContexts.Save, "0", "MontoFondo", 
            ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        public decimal MaximoGasto
        {
            get => maximoGasto;
            set => SetPropertyValue(nameof(MaximoGasto), ref maximoGasto, value);
        }

        [DbType("money"), Persistent("MinimoDisponible"), XafDisplayName("MinimoDisponible"), Index(6), 
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"),
            RuleRange("CajaChica.MinimoDisponible >= MaximoGasto y <= MontoFondo", DefaultContexts.Save, "MaximoGasto", "MontoFondo", 
            ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        public decimal MinimoDisponible
        {
            get => minimoDisponible;
            set => SetPropertyValue(nameof(MinimoDisponible), ref minimoDisponible, value);
        }

        [DbType("bit"), Persistent("Activa"), XafDisplayName("Activa"), Index(7)]
        public bool Activa
        {
            get => activa;
            set => SetPropertyValue(nameof(Activa), ref activa, value);
        }

        #endregion

        #region Colecciones
        [Association("CajaChica-Transacciones"), DevExpress.Xpo.Aggregated, XafDisplayName("Transacciones")]
        public XPCollection<CajaChicaTransaccion> Transacciones
        {
            get
            {
                return GetCollection<CajaChicaTransaccion>(nameof(Transacciones));
            }
        }
        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}