using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    /// <summary>
    /// Facturacion
    /// BO con los tributos calculados para una venta
    /// </summary>
    [ModelDefault("Caption", "Resumen Tributos"), NavigationItem(false), CreatableItem(false),
        XafDefaultProperty(nameof(Tributo)), Persistent(nameof(VentaResumenTributo))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class VentaResumenTributo : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VentaResumenTributo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            valor = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        decimal valor;
        Tributo tributo;
        Venta venta;

        [Association("Factura-ResumenTributos"), XafDisplayName("Venta"), Persistent(nameof(Venta))]
        public Venta Venta
        {
            get => venta;
            set => SetPropertyValue(nameof(Venta), ref venta, value);
        }

        //[Association("Tributo-VentaResumenTributos")]
        [XafDisplayName("Tributo")]
        public Tributo Tributo
        {
            get => tributo;
            set => SetPropertyValue(nameof(Tributo), ref tributo, value);
        }

        [DbType("numeric(14,4)"), XafDisplayName("Valor"), Persistent(nameof(Valor))]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
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