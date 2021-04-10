using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que representa los códigos de barra asociados a un producto
    /// </summary>
    [NavigationItem(false), ModelDefault("Caption", "Código de Barra"), XafDefaultProperty("CodigoBarra"), Persistent("ProductoCodigoBarra")]
    [ImageName(nameof(ProductoCodigoBarra)), CreatableItem(false)]
    public class ProductoCodigoBarra : XPObject
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
        }

        private Producto _producto;
        private System.Boolean _activo;
        private System.String _codigoBarra;
        public ProductoCodigoBarra(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código de Barra")]
        [DevExpress.Xpo.SizeAttribute(20), DbType("varchar(20)"), Indexed(Unique = true)]
        [RuleRequiredField("ProductoCodigo.CodigoBarra_Requerido", "Save")]
        public System.String CodigoBarra
        {
            get
            {
                return _codigoBarra;
            }
            set
            {
                SetPropertyValue("CodigoBarra", ref _codigoBarra, value);
            }
        }
        [RuleRequiredField("ProductoCodigo.Activo_Requerido", "Save")]
        public System.Boolean Activo
        {
            get
            {
                return _activo;
            }
            set
            {
                SetPropertyValue("Activo", ref _activo, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("CodigosBarra-Producto"), DbType("int")]
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
