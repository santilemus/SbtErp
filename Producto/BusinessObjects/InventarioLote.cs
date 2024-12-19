using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    /// <summary>
    /// Clase para Objetos persistentes que representa los Lotes por Producto. Cada Lote corresponde a una compra, importación u Orden de Producción.
    /// Pendiente de agregar propiedad informativa que identifique la compra (ingreso, importación) u Orden de Producción. No se podrá
    /// relacionar porque esa información estará en los módulos de transacciones correspondientes, que no se conocen en este momento.
    /// Los items de este objeto serán generados por el sistema y el usuario no podrá modificar ninguna de sus propiedades
    /// </summary>
    [CreatableItem(false)]
    [ImageName(nameof(InventarioLote))]
    [RuleIsReferenced("ProductoLote_Referencia", DefaultContexts.Delete, typeof(InventarioLote), nameof(IngresoDetalle),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [RuleCriteria("ProductoLote.Entrada_Mayor_o_Igual_Salidas", DefaultContexts.Save, @"Entrada >= Salida", "Entrada >= Acumulado Salidas")]
    [ModelDefault("Caption", "Lote"), NavigationItem(false), DefaultProperty(nameof(CodigoLote)), Persistent("ProductoLote")]
    public class InventarioLote : XPObject
    {


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Entrada = 0.0m;
            Salida = 0.0m;
            Costo = 0.0m;
        }

        string codigoLote;
        DateTime? fechaVence;
        DateTime? fechaFabricacion;
        private Producto.Module.BusinessObjects.Producto _producto;
        private System.Decimal _salida;
        private System.Decimal _entrada;
        private object ingresoDetalle;
        private System.Decimal _costo;

        public InventarioLote(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        /// <summary>
        /// No de Lote. Corresponde al Oid del detalle del ingreso
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("No Lote")]
        [RuleRequiredField("ProductoLote.Oid_Requerido", "Save")]
        [DbType("bigint")] // NOTA: Revisar para efectos practicos el lote sera el ingreso
        public object IngresoDetalle
        {
            get => ingresoDetalle;
            set => SetPropertyValue(nameof(IngresoDetalle), ref ingresoDetalle, value);
        }

        [Size(48), DbType("varchar(48)"), XafDisplayName("Código Lote"), Persistent(nameof(CodigoLote))]
        [Indexed(Name = "idxCodigoLote_InventarioLote")]
        public string CodigoLote
        {
            get => codigoLote;
            set => SetPropertyValue(nameof(CodigoLote), ref codigoLote, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Fabricación"), Persistent(nameof(FechaFabricacion))]
        public DateTime? FechaFabricacion
        {
            get => fechaFabricacion;
            set => SetPropertyValue(nameof(FechaFabricacion), ref fechaFabricacion, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Vence"), Persistent(nameof(FechaVence))]
        public DateTime? FechaVence
        {
            get => fechaVence;
            set => SetPropertyValue(nameof(FechaVence), ref fechaVence, value);
        }

        /// <summary>
        /// Costo de la compra, importacion u orden de produccion. Para obtener el costo de la ultima compra se debe ir al ultimo lote,
        /// de esta manera se reemplaza la propiedad Ultimo Costo de Compra del objeto persistene Producto
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Costo"), ModelDefault("DisplayFormat", "N6"), ModelDefault("EditMask", "N6")]
        [DbType("numeric(14,6)")]
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
        /// Cantidad de la entrada de lote. Corresponde a la cantidad de compra, importacion o Produccion. Sera
        /// igual a la cantidad en la correspondiente transaccion (compra, importacion, orden de produccion)
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Entradas"), ModelDefault("DisplayFormat", "N2"), ModelDefault("EditMask", "N2")]
        [DbType("numeric(12,2)"), Persistent(nameof(Entrada))]
        public System.Decimal Entrada
        {
            get => _entrada;
            set => SetPropertyValue(nameof(Entrada), ref _entrada, value);
        }

        /// <summary>
        /// Cantidad acumulada de las salidas del lote. Las salidas debe ser menores o iguales a la entrada
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Salidas")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), ModelDefault("DisplayFormat", "N2"), ModelDefault("EditMask", "N2")]
        [DbType("numeric(12,2)"), Persistent(nameof(Salida))]
        public System.Decimal Salida
        {
            get => _salida;
            set => SetPropertyValue(nameof(Salida), ref _salida, value);
        }
        [DevExpress.Xpo.AssociationAttribute("Lotes-Producto")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        public Producto.Module.BusinessObjects.Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        /// <summary>
        /// Existencia disponible para el lote. Entrada - AcumSalida
        /// </summary>
        [PersistentAlias("[Entrada] - [Salida]")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Existencia")]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(true)]
        public System.Decimal Existencia => Convert.ToDecimal(EvaluateAlias("Existencia"));

    }
}
