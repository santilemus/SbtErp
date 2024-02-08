using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ModelDefault("Caption", "Categoría Activo"), NavigationItem("Activo Fijo"), CreatableItem(false)]
    [DefaultProperty(nameof(Nombre)), Persistent(nameof(ActivoCategoria))]
    [ImageName("CategoriaProducto")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoCategoria : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoCategoria(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            VidaUtil = 0;
            ValorResidual = 0.0m;
            activo = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string codigo;
        bool activo;
        decimal valorResidual;
        int vidaUtil;
        Catalogo cuentaGasto;
        Catalogo cuentaDepreciacion;
        Catalogo cuentaActivo;
        ActivoCategoria padre;
        string nombre;

        [Size(100), DbType("varchar(100)"), RuleRequiredField("ActivoCategoria.Descripcion_Requerido", DefaultContexts.Save)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Association("ActivoCategoria-Categorias"), XafDisplayName("Padre")]
        [ToolTip("Categoría padre")]
        public ActivoCategoria Padre
        {
            get => padre;
            set
            {
                bool changed = SetPropertyValue(nameof(Padre), ref padre, value);
                if (!IsLoading && !IsSaving && changed && Session.IsNewObject(this))
                {
                    Codigo = value.Codigo;
                }
            }
        }

        [XafDisplayName("Cuenta Activo"), RuleRequiredField("ActivoCategoria.Cuenta_Requerido", "Save")]
        public Catalogo CuentaActivo
        {
            get => cuentaActivo;
            set => SetPropertyValue(nameof(CuentaActivo), ref cuentaActivo, value);
        }

        [XafDisplayName("Cuenta Depreciación")]
        public Catalogo CuentaDepreciacion
        {
            get => cuentaDepreciacion;
            set => SetPropertyValue(nameof(CuentaDepreciacion), ref cuentaDepreciacion, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("Código")]
        [Indexed(Name = "idxActivoCategoria")]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [XafDisplayName("Cuenta Gastos")]
        public Catalogo CuentaGasto
        {
            get => cuentaGasto;
            set => SetPropertyValue(nameof(CuentaGasto), ref cuentaGasto, value);
        }

        [XafDisplayName("Vida Útil (meses)")]
        [DbType("smallint")]
        [RuleValueComparison("ActivoCategoría.VidaUtil_Valido", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, "La vida útil debe ser mayor a cero")]
        public int VidaUtil
        {
            get => vidaUtil;
            set => SetPropertyValue(nameof(VidaUtil), ref vidaUtil, value);
        }

        [XafDisplayName("Valor Residual")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [RuleValueComparison("ActivoCatalogo.ValorResidual_Valido", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, "El valor residual debe ser mayor a cero")]

        public decimal ValorResidual
        {
            get => valorResidual;
            set => SetPropertyValue(nameof(ValorResidual), ref valorResidual, value);
        }

        [DbType("bit"), XafDisplayName("Activo")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        #region Colecciones
        [Association("ActivoCategoria-Categorias"), XafDisplayName("Categorías Hijas"), Index(0)]
        public XPCollection<ActivoCategoria> Categorias => GetCollection<ActivoCategoria>(nameof(Categorias));
        [Association("Categoria-Activos"), DevExpress.Xpo.Aggregated, XafDisplayName("Activos"), Index(1)]
        public XPCollection<ActivoCatalogo> Activos => GetCollection<ActivoCatalogo>(nameof(Activos));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}