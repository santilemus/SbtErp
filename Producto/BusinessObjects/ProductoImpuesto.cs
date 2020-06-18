using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que representa los Impuestos que se aplican a cada producto
    /// </summary>
    [DefaultClassOptions]
    [ImageName(nameof(ProductoImpuesto))]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Impuesto"), NavigationItem(false), ModelDefault("Caption", "Impuesto"),
        Persistent("ProductoImpuesto"), XafDefaultProperty("Codigo")]
    public class ProductoImpuesto : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
            MontoFijo = 0.0m;
            Porcentaje = 0.0m;
            AplicarEn = ClasificacionAplicarImpuesto.Ventas;
        }

        private System.Boolean _activo;
        private System.Decimal _montoFijo;
        private System.Decimal _porcentaje;
        private Producto _producto;
        private ClasificacionAplicarImpuesto _aplicarEn;
        private Listas _codigo;
        public ProductoImpuesto(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Código")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("ProductoImpuesto.Id_Requerido", DefaultContexts.Save, "Id Impuesto es requerido")]
        [DataSourceCriteria("Categoria = 'Impuesto'")]
        public Listas Codigo
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
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Aplicar En")]
        [RuleRequiredField("ProductoImpuesto.AplicarEn_Requerido", "Save")]
        public ClasificacionAplicarImpuesto AplicarEn
        {
            get
            {
                return _aplicarEn;
            }
            set
            {
                SetPropertyValue("AplicarEn", ref _aplicarEn, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Monto Fijo")]
        public System.Decimal MontoFijo
        {
            get
            {
                return _montoFijo;
            }
            set
            {
                SetPropertyValue("MontoFijo", ref _montoFijo, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Porcentaje")]
        public System.Decimal Porcentaje
        {
            get
            {
                return _porcentaje;
            }
            set
            {
                SetPropertyValue("Porcentaje", ref _porcentaje, value);
            }
        }
        [RuleRequiredField("ProductoImpuesto.Activo_Requerido", "Save")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Activo")]
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
        [DevExpress.Xpo.AssociationAttribute("Impuestos-Producto")]
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
