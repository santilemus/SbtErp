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
    /// Clase para Objetos persistentes que representa los Lotes por Producto. Cada Lote corresponde a una compra, importación u Orden de Producción.
    /// Pendiente de agregar propiedad informativa que identifique la compra (ingreso, importación) u Orden de Producción. No se podrá
    /// relacionar porque esa información estará en los módulos de transacciones correspondientes, que no se conocen en este momento.
    /// Los items de este objeto serán generados por el sistema y el usuario no podrá modificar ninguna de sus propiedades
    /// </summary>
    [DefaultClassOptions]
    [ImageName(nameof(ProductoLote))]
    [RuleIsReferenced("ProductoLote_Referencia", DefaultContexts.Delete, typeof(ProductoLote), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [RuleCriteria("ProductoLote.Entrada_Mayor_o_Igual_Salidas", DefaultContexts.Save, @"Entrada >= AcumSalida", "Entrada >= Acumulado Salidas")]
    [ModelDefault("Caption", "Lote"), NavigationItem(false), XafDefaultProperty("NoLote"), Persistent("ProductoLote")]
    public class ProductoLote : XPObjectBaseBO
    {
        /// <summary>
        /// Existencia disponible para el lote. Entrada - AcumSalida
        /// </summary>
        [PersistentAlias("Entrada - AcumSalida")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Existencia")]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(true)]
        public System.Decimal Existencia
        {
            get { return Convert.ToDecimal(EvaluateAlias("Existencia")); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Entrada = 0.0m;
            AcumSalida = 0.0m;
            Costo = 0.0m;
            Promedio = 0.0m;
            PromedioAnterior = 0.0m;
        }

        private Producto _producto;
        private System.Decimal _acumSalida;
        private System.Decimal _entrada;
        private System.Int32 _noLote;
        private System.Decimal _promedioAnterior;
        private System.Decimal _promedio;
        private System.Decimal _costo;
        private System.DateTime _fecha;

        public ProductoLote(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        /// <summary>
        /// No de Lote. Revisar si cambiamos a un string, porque debe representar el numero de compra, importacion u orden de produccion
        /// Debido a que se encuentra en un BaseModule que no conoce la existencia del proyecto que contiene el detalle de la transaccion
        /// no podemos establecer la relacion (referencia) y por lo tanto solo sera informativo, esa es la razon de revisar si cambia a string
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("No Lote")]
        [RuleRequiredField("ProductoLote.NoLote_Requerido", "Save")]
        public System.Int32 NoLote
        {
            get
            {
                return _noLote;
            }
            set
            {
                SetPropertyValue("NoLote", ref _noLote, value);
            }
        }
        /// <summary>
        /// Fecha de lote, corresponde a la fecha de la compra, orden de produccion o importacion. Para obtener la ultima fecha de compra,
        /// se debera recuperar el ultimo lote, se reemplaza de esta manera la propiedad ultima fecha de compra del objeto persistente Producto
        /// </summary>
        [RuleRequiredField("ProductoLote.Fecha_Requerido", "Save")]
        public System.DateTime Fecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                SetPropertyValue("Fecha", ref _fecha, value);
            }
        }

        /// <summary>
        /// Costo de la compra, importacion u orden de produccion. Para obtener el costo de la ultima compra se debe ir al ultimo lote,
        /// de esta manera se reemplaza la propiedad Ultimo Costo de Compra del objeto persistene Producto
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Costo"), ModelDefault("DisplayFormat", "N6"), ModelDefault("EditMask", "N6")]
        public System.Decimal Costo
        {
            get
            {
                return _costo;
            }
            set
            {
                SetPropertyValue("Costo", ref _costo, value);
            }
        }

        /// <summary>
        /// Costo Promedio hasta el lote actual. Para obtener el costo promedio vigente se debe ir al ultimo lote
        /// y se reemplaza la propiedad Costo Promedio del objeto Persistente Producto
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Costo Promedio"), ModelDefault("DisplayFormat", "N6"), ModelDefault("EditMask", "N6")]
        public System.Decimal Promedio
        {
            get
            {
                return _promedio;
            }
            set
            {
                SetPropertyValue("Promedio", ref _promedio, value);
            }
        }

        /// <summary>
        /// Costo Promedio al lote anterior. Se crea esta propiedad para poder revertir en caso de ser necesario. 
        /// Por ejemplo, eliminación de un lote.
        /// NOTA: Es necesario evaluar como se implementaría la funcionalidad de reversión. No sería en este module
        /// o proyecto, sino en los módulos de transacciones, que puede ser: Inventarios, Compras o Producción
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Promedio Anterior")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), ModelDefault("DisplayFormat", "N6"), ModelDefault("EditMask", "N6")]
        public System.Decimal PromedioAnterior
        {
            get
            {
                return _promedioAnterior;
            }
            set
            {
                SetPropertyValue("PromedioAnterior", ref _promedioAnterior, value);
            }
        }

        /// <summary>
        /// Cantidad de la entrada de lote. Corresponde a la cantidad de compra, importacion o Produccion. Sera
        /// igual a la cantidad en la correspondiente transaccion (compra, importacion, orden de produccion)
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Entradas"), ModelDefault("DisplayFormat", "N2"), ModelDefault("EditMask", "N2")]
        public System.Decimal Entrada
        {
            get
            {
                return _entrada;
            }
            set
            {
                SetPropertyValue("Entrada", ref _entrada, value);
            }
        }

        /// <summary>
        /// Cantidad acumulada de las salidas del lote. Las salidas debe ser menores o iguales a la entrada
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Salidas")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), ModelDefault("DisplayFormat", "N2"), ModelDefault("EditMask", "N2")]
        public System.Decimal AcumSalida
        {
            get
            {
                return _acumSalida;
            }
            set
            {
                SetPropertyValue("AcumSalida", ref _acumSalida, value);
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Lotes-Producto")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
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
