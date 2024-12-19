using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a las diferentes presentaciones que pueden tener los productos. Por ejemplo: unidad, blister de 5 unidades, caja de 20, caja de 30 unidades, etc
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Presentacion"), DefaultProperty("Nombre"), NavigationItem("Inventario"),
        CreatableItem(false), Persistent(nameof(ProductoPresentacion))]
    [ImageName("Presentacion")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProductoPresentacion : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProductoPresentacion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
            Unidades = 0;
            Defecto = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string nombre;
        decimal unidades;
        bool activo;
        bool defecto;
        private Producto producto;

        [Association("Presentacion-Producto")]
        public Producto Producto
        {
            get => producto;
            set => SetPropertyValue<Producto>(nameof(Producto), ref producto, value);
        }

        [DbType("varchar(50)"), Persistent("Nombre")]
        [Size(25), XafDisplayName("Nombre"), Index(1), RuleRequiredField("Presentacion.Nombre_Requerido", DefaultContexts.Save)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue("Nombre", ref nombre, value);
        }


        [DbType("numeric(12,2)"), Persistent(nameof(Unidades))]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [XafDisplayName("Unidades Inventario"), Index(2), VisibleInLookupListView(false),
           ToolTip("Cantidad de Unidades de Inventario para el presentacion")]
        public decimal Unidades
        {
            get => unidades;
            set => SetPropertyValue(nameof(Unidades), ref unidades, value);
        }

        [DbType("bit"), Persistent("Activo")]
        [XafDisplayName("Activo"), Index(3)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue("Activo", ref activo, value);
        }

        [DbType("bit"), Persistent("Defecto")]
        [XafDisplayName("Defecto"), Index(4), ToolTip("Presentacion o presentacion por defecto")]
        public bool Defecto
        {
            get => defecto;
            set => SetPropertyValue(nameof(Defecto), ref defecto, value);
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