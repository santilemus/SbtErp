using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Definicion del objeto persistente que representa el ensamble de un producto.
    /// Un ensamble es un producto que esta compuesto por otros. Por ejemplo: una silla: 4 patas, un espaldar, un asiento
    /// y donde cada item del ensamble tiene su propio inventario. Otro ejemplo es un plato de comida, por ejemplo, donde se
    /// descargan del inventario cada uno de los items que componen el plato
    /// </summary>
    [DefaultClassOptions, CreatableItem(false)]
    [ImageName(nameof(ProductoEnsamble))]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Ensamble"), Persistent("ProductoEnsamble"), NavigationItem(false)]
    public class ProductoEnsamble : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Cantidad = 0.0m;
        }

        private System.Decimal _cantidad;
        private Producto _producto;
        private Producto _item;
        public ProductoEnsamble(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        /// <summary>
        /// Item, corresponde a un producto que representa un elemento del ensamble
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Item"), Persistent(nameof(Item))]
        [RuleRequiredField("ProductoEnsamble.Item_Requerido", DefaultContexts.Save, "Item es requerido")]
        public Producto Item
        {
            get => _item;
            set => SetPropertyValue(nameof(Item), ref _item, value);
        }

        /// <summary>
        /// Cantidad de Items requeridos por ensamble. En el ejemplo de la silla: son 4 patas
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Cantidad")]
        [RuleRequiredField("ProductoEnsamble.Cantidad_Requerido", "Save")]
        [DbType("numeric(12,2)"), Persistent(nameof(Cantidad))]
        public System.Decimal Cantidad
        {
            get => _cantidad;
            set => SetPropertyValue(nameof(Cantidad), ref _cantidad, value);
        }

        /// <summary>
        /// Es un producto que corresponde a un ensamble
        /// </summary>
        [DevExpress.Xpo.AssociationAttribute("ItemsEnsamble-Producto")]
        public Producto Producto
        {
            get
            {
                return _producto;
            }
            set
            {
                SetPropertyValue("Producto", ref _producto, value);
            }
        }

    }
}
