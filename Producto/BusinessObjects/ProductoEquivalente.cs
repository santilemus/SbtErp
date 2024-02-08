using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Definicion del objeto persistente que representa las equivalencias entre productos. 
    /// </summary>
    [CreatableItem(false)]
    [ImageName(nameof(ProductoEquivalente))]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Equivalente"), XafDefaultProperty("Equivalente"), NavigationItem(false),
        Persistent("ProductoEquivalente")]
    public class ProductoEquivalente : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private Producto _producto;
        private System.String _comentario;
        private Producto _equivalente;
        public ProductoEquivalente(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        /// <summary>
        /// Representa el Producto equivalente 
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Equivalente")]
        [RuleRequiredField("ProductoEquivalente.Equivalente_Requerido", DefaultContexts.Save, "Producto equivalente es requerido")]
        [ExplicitLoading]
        public Producto Equivalente
        {
            get
            {
                return _equivalente;
            }
            set
            {
                SetPropertyValue("Equivalente", ref _equivalente, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), DbType("varchar(100)")]
        public System.String Comentario
        {
            get
            {
                return _comentario;
            }
            set
            {
                SetPropertyValue("Comentario", ref _comentario, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Equivalentes-Producto")]
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
