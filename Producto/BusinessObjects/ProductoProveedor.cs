using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a la relación de los proveedores de un producto
    /// </summary>
    [DefaultClassOptions]
    [ImageName("Tercero")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Producto Proveedor"), NavigationItem(false), Persistent("ProductoProveedor"),
        XafDefaultProperty("Proveedor")]
    public class ProductoProveedor : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DiasOrden = 0;
        }

        private System.Int16 _diasOrden;
        private Producto _producto;
        private ZonaGeografica _paisOrigen;
        private Tercero.Module.BusinessObjects.Tercero _fabricante;
        private System.String _codigo;
        private Tercero.Module.BusinessObjects.Tercero _proveedor;
        public ProductoProveedor(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        /// <summary>
        /// Proveedor del Producto. Es un tercero con role proveedor
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Proveedor")]
        [RuleRequiredField("ProductoProveedor.Proveedor_Requerido", DefaultContexts.Save, "Proveedor es requerido")]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get
            {
                return _proveedor;
            }
            set
            {
                SetPropertyValue("Proveedor", ref _proveedor, value);
            }
        }
        /// <summary>
        /// Codigo del producto de acuerdo con la nomenclatura o catalogo del proveedor
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código Según Proveedor")]
        [DevExpress.Xpo.SizeAttribute(20), DbType("varchar(20)")]
        public System.String Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                SetPropertyValue("Codigo", ref _codigo, value);
            }
        }
        /// <summary>
        /// Fabricante del Producto. Es un tercero
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Fabricante")]
        public Tercero.Module.BusinessObjects.Tercero Fabricante
        {
            get
            {
                return _fabricante;
            }
            set
            {
                SetPropertyValue("Fabricante", ref _fabricante, value);
            }
        }
        /// <summary>
        /// Pais de origen del producto
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("País de Origen")]
        [DataSourceCriteria("ZonaPadre is null And Activa = True")]
        public ZonaGeografica PaisOrigen
        {
            get
            {
                return _paisOrigen;
            }
            set
            {
                SetPropertyValue("PaisOrigen", ref _paisOrigen, value);
            }
        }
        /// <summary>
        /// Días estimados para que el proveedor entregue el producto, una vez se ha colocado una orden de compra
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Días Entrega de Orden")]
        public System.Int16 DiasOrden
        {
            get
            {
                return _diasOrden;
            }
            set
            {
                SetPropertyValue("DiasOrden", ref _diasOrden, value);
            }
        }
        /// <summary>
        /// Referencia al Producto al cual corresponde el proveedor
        /// </summary>
        [DevExpress.Xpo.AssociationAttribute("Proveedores-Producto")]
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
