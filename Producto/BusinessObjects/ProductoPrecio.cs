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
    /// Objeto Persistente que corresponde a la Representación de los Precios de Productos
    /// </summary>
    [DefaultClassOptions, CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Precio"), NavigationItem(false), ModelDefault("Caption", "Precio"),
        Persistent("ProductoPrecio"), XafDefaultProperty("Tipo")]
    [ImageName(nameof(ProductoPrecio))]
    [RuleCriteria("ProductoPrecio.CantidadDesde_CantidadHasta", DefaultContexts.Save, @"CantidadHasta >= CantidadDesde", "Cantidad Hasta >= Cantidad Desde"),
     RuleCriteria("ProductoPrecio.HoraDesde_HoraHasta", DefaultContexts.Save, @"HoraHasta >= HoraDesde", "Hora Hasta >= Hora Desde")]
    public class ProductoPrecio : XPObjectBaseBO
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CantidadDesde = 0.0m;
            CantidadHasta = 0.0m;
            PrecioUnitario = 0.0m;
            Activo = true;
        }

        private System.Decimal _cantidadHasta;
        private System.Decimal _cantidadDesde;
        private System.Decimal _precioUnitario;
        private Producto _producto;
        private System.Boolean _activo;
        private DateTime _horaHasta;
        private DateTime _horaDesde;
        private System.String _descripcion;
        private Listas _tipo;
        public ProductoPrecio(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        /// <summary>
        /// Tipo de Precio del Producto. Se obtiene de la lista de valores y corresponde a los de categoria = TipoPrecio
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Tipo")]
        [RuleRequiredField("ProductoPrecio.Tipo_Requerido", DefaultContexts.Save, "Tipo Precio es Requerido")]
        [DataSourceCriteria("Categoria = 'TipoPrecio'")]
        public Listas Tipo
        {
            get
            {
                return _tipo;
            }
            set
            {
                SetPropertyValue("Tipo", ref _tipo, value);
            }
        }

        /// <summary>
        /// Descripción del tipo de precio
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Descripción"), DbType("varchar(100)")]
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

        /// <summary>
        /// Precio unitario
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Precio Unitario")]
        [RuleValueComparison("ProductoPrecio.PrecioUnitario_Mayor_Cero", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4"), Persistent(nameof(PrecioUnitario))]
        [DbType("numeric(14,4)")]
        public System.Decimal PrecioUnitario
        {
            get
            {
                return _precioUnitario;
            }
            set
            {
                SetPropertyValue("PrecioUnitario", ref _precioUnitario, value);
            }
        }

        /// <summary>
        /// Cantidad Desde. Cuando el precio se aplica en función de la cantidad comprada
        /// </summary>
        [XafDisplayName("Cantidad Desde"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [RuleValueComparison("ProductoPrecio.CantidadDesde_Mayor_Igual_Cero", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [DbType("numeric(12,2)")]
        public System.Decimal CantidadDesde
        {
            get
            {
                return _cantidadDesde;
            }
            set
            {
                SetPropertyValue("CantidadDesde", ref _cantidadDesde, value);
            }
        }

        /// <summary>
        /// Cantidad Hasta. Cuando el precio se aplica en función de la cantidad de compra (se encuentra dentro de un rango)
        /// </summary>
        [XafDisplayName("Cantidad Hasta"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DbType("numeric(12,2)")]
        public System.Decimal CantidadHasta
        {
            get
            {
                return _cantidadHasta;
            }
            set
            {
                SetPropertyValue("CantidadHasta", ref _cantidadHasta, value);
            }
        }

        /// <summary>
        /// Hora Desde. Cuando el precio esta vigente para determinadas horas
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Hora Desde")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public DateTime HoraDesde
        {
            get
            {
                return _horaDesde;
            }
            set
            {
                SetPropertyValue("HoraDesde", ref _horaDesde, value);
            }
        }

        /// <summary>
        /// Hora Hasta. Cuando el precio esta vigente para determinadas horas entre Hora Desde y Hora Hasta
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Hora Hasta")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public DateTime HoraHasta
        {
            get
            {
                return _horaHasta;
            }
            set
            {
                SetPropertyValue("HoraHasta", ref _horaHasta, value);
            }
        }

        /// <summary>
        ///  Indica sí el precio está activo
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Activo")]
        [RuleRequiredField("ProductoPrecio.Activo_Requerido", "Save")]
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

        /// <summary>
        /// Referencia al Producto al cual corresponde el precio
        /// </summary>
        [DevExpress.Xpo.AssociationAttribute("Precios-Producto")]
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
