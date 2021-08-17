using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al encabezado de las ordenes de compra
    /// </summary>
    /// <remarks>
    /// Faltan las siguientes relaciones: 
    /// 1. Cuenta o Rubro de presupuestos (el detalle, de alli se va al encabezado del presupuesto)
    /// 2. Solicitud de Compra
    /// 3. Comparativo de Ofertas (ver si sigue siendo valido en este modelo)
    /// 4. No de Oferta
    /// </remarks>

    [DefaultClassOptions, ModelDefault("Caption", "Orden Compra"), NavigationItem("Compras"), CreatableItem(false),
        DefaultProperty(nameof(Proveedor)), Persistent(nameof(OrdenCompra))]
    [ImageName(nameof(OrdenCompra))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class OrdenCompra : XPCustomFacturaBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public OrdenCompra(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Empleado.Module.BusinessObjects.Empleado aprobo;
        EEstadoOrdenCompra estadoOrden;
        string concepto;
        ETipoCompra tipo = ETipoCompra.Servicio;
        Tercero.Module.BusinessObjects.Tercero proveedor;

        /// <summary>
        /// Proveedor al cual se gira la orden de compra
        /// </summary>
        [XafDisplayName("Proveedor"), RuleRequiredField("OrdenCompra.Proveedor_Requerido", DefaultContexts.Save), Index(5)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        /// <summary>
        /// Tipo de Compra. Puede ser: Servicio, Producto, ActivoFijo
        /// </summary>
        [DbType("smallint"), XafDisplayName("Tipo Compra"), RuleRequiredField("OrdenCompra.Tipo_Requerido", "Save"), Index(6)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public ETipoCompra Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        /// <summary>
        /// Concepto de la compra
        /// </summary>
        [Size(200), DbType("varchar(200)"), XafDisplayName("Concepto"), Index(10)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        /// <summary>
        /// Estado de la orden de compra
        /// </summary>
        [DbType("smallint"), XafDisplayName("Estado Orden"), Index(11)]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        public EEstadoOrdenCompra EstadoOrden
        {
            get => estadoOrden;
            set => SetPropertyValue(nameof(EstadoOrden), ref estadoOrden, value);
        }

        /// <summary>
        /// Empleado que aprobo la orden de compra
        /// </summary>
        [XafDisplayName("Aprobada Por"), VisibleInListView(false), VisibleInLookupListView(false), Index(12)]
        [ExplicitLoading]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public Empleado.Module.BusinessObjects.Empleado Aprobo
        {
            get => aprobo;
            set => SetPropertyValue(nameof(Aprobo), ref aprobo, value);
        }


        #endregion

        #region colecciones
        [Association("OrdenCompra-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle Orden Compra"), Index(0)]
        public XPCollection<OrdenCompraDetalle> Detalles => GetCollection<OrdenCompraDetalle>(nameof(Detalles));

        [Association("OrdenCompra-Facturas"), Index(1)]
        public XPCollection<CompraFactura> Facturas => GetCollection<CompraFactura>(nameof(Facturas));

        [Association("OrdenCompra-Ingresos"), Index(2), XafDisplayName("Ingresos")]
        public XPCollection<InventarioMovimiento> Ingresos => GetCollection<InventarioMovimiento>(nameof(Ingresos));

        #endregion

        #region Metodos
        public override void UpdateTotalExenta(bool forceChangeEvents)
        {
            base.UpdateTotalExenta(forceChangeEvents);
            decimal? oldExenta = exenta;
            exenta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Exenta])")));
            if (forceChangeEvents)
                OnChanged(nameof(Exenta), oldExenta, exenta);
        }

        public override void UpdateTotalGravada(bool forceChangeEvents)
        {
            base.UpdateTotalGravada(forceChangeEvents);
            decimal? oldGravada = gravada;
            decimal tempGravada = 0.0m;
            foreach (OrdenCompraDetalle detalle in Detalles)
                tempGravada += Convert.ToDecimal(detalle.Gravada);
            gravada = tempGravada;
            if (forceChangeEvents)
                OnChanged(nameof(Gravada), oldGravada, gravada);
        }

        public override void UpdateTotalIva(bool forceChangeEvents)
        {
            base.UpdateTotalIva(forceChangeEvents);
            decimal? oldIva = iva;
            iva = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Iva])")));
            if (forceChangeEvents)
                OnChanged(nameof(Iva), oldIva, iva);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}