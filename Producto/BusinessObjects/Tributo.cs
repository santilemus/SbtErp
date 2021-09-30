using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;


namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Definicion de los impuestos y contribuciones especiales a calcular. No aplica el IVA, sino otros, como Fovial, Contrans, etc
    /// </summary>
    /// <remarks>
    /// Debe moverse a Productos. Y dinamicamente tendra que manejarse la relacion con la venta, hay que ver el impacto
    /// Revisar el BO ProductoImpuesto que se excluyo del proyecto SBT.Apps.Producto para validar que todos los datos se han incorporado
    /// </remarks>
    [DefaultClassOptions, CreatableItem(false), ModelDefault("Caption", "Definición de Tributos"), NavigationItem("Facturación")]
    [Persistent(nameof(Tributo)), DefaultProperty(nameof(Nombre))]
    [ImageName(nameof(Tributo))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Tributo : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Tributo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string formula;
        bool activo;
        Type tipoBO;
        EClaseTributo clase;
        string nombre;

        [Size(100), DbType("varchar(50)"), Index(0), XafDisplayName("Nombre"), RuleRequiredField("Impuesto.Nombre_Requerido", "Save")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("smallint"), XafDisplayName("Modalidad"), Index(2), VisibleInLookupListView(true)]
        public EClaseTributo Clase
        {
            get => clase;
            set => SetPropertyValue(nameof(Clase), ref clase, value);
        }

        /// <summary>
        /// El BO para el cual se implementa la formula
        /// </summary>
        /// <remarks>
        /// Mas info
        /// 1. EditorAliases.TypePropertyEditor implementa la lista de seleccion de los BO
        ///    Ver: https://docs.devexpress.com/eXpressAppFramework/113579/concepts/business-model-design/data-types-supported-by-built-in-editors/type-properties
        /// 2. ValueConverter, para hacer la propiedad persistente se tiliza la conversion a string y guardar el nombre del BO en la bd, incluyendo el namespace
        /// </remarks>
        [XafDisplayName("Tipo BO"), Persistent(nameof(TipoBO))]
        [EditorAlias(EditorAliases.TypePropertyEditor)]
        [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter)), ImmediatePostData]
        //[DataSourceCriteria("")]
        [Size(100), DbType("varchar(100)")]
        public Type TipoBO
        {
            get => tipoBO;
            set => SetPropertyValue(nameof(TipoBO), ref tipoBO, value);
        }

        
        [Size(400), DbType("varchar(400)"), XafDisplayName("Fórmula"), Persistent(nameof(Formula))]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false)]
        //[ModelDefault("Width", "50")]
        [ModelDefault("RowCount", "3")]
        public string Formula
        {
            get => formula;
            set => SetPropertyValue(nameof(Formula), ref formula, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), RuleRequiredField("Tributo.Activo_Requerido", DefaultContexts.Save)]
        [Index(6), VisibleInLookupListView(true)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        #endregion

        #region Colecciones
        [Association("Tributo-TributoCategorias"), DevExpress.Xpo.Aggregated, XafDisplayName("Categorías"), Index(0)]
        public XPCollection<TributoCategoria> Categorias => GetCollection<TributoCategoria>(nameof(Categorias));


        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}