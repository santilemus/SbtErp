using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName(nameof(ProductoAtributo))]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Atributo"), NavigationItem(false), Persistent("ProductoAtributo")]
    public class ProductoAtributo : XPObjectBaseBO
    {
        private Producto _producto;
        private System.String _descripcion;
        private Listas _atributo;
        public ProductoAtributo(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Atributo")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleRequiredField("ProductoAtributo.Atributo_Requerido", DefaultContexts.Save, "Atributo es requerido")]
        [DataSourceCriteria("[Categoria] = 13")]
        public Listas Atributo
        {
            get
            {
                return _atributo;
            }
            set
            {
                SetPropertyValue("Atributo", ref _atributo, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Descripción"), DbType("varchar(100)")]
        [RuleRequiredField("ProductoAtributo.Descripcion_Requerido", "Save")]
        public System.String Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                SetPropertyValue("Descripcion", ref _descripcion, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Atributos-Producto")]
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
