using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Inventario.Module.BusinessObjects
{
    [ModelDefault("Caption", "Movimiento Inventario - Detalle"), CreatableItem(false), NavigationItem(false),
        DefaultProperty(nameof(Producto)), Persistent(nameof(InventarioMovimientoDetalle))]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    [Appearance("InventarioMovimientoDetalle_IngresoDeCompra", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show, 
        Criteria = "([Movimiento.TipoMovimiento.Codigo] In ('201', '202', '203'))", Context = "*", AppearanceItemType = "Any",
        TargetItems = "OrdenCompraItem;FacturaItem;CodigoLote;FechaFabricacion;UnidadesTeorica;FechaVence")]
    //[ImageName("BO_Contact")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InventarioMovimientoDetalle : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InventarioMovimientoDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            cantidadAnulada = null;
            unidadesTeorica = 0.0m;
            oid = -1;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal unidades;
        DateTime? fechaFabricacion;
        [Persistent(nameof(CantidadAnulada))]
        decimal? cantidadAnulada;
        DateTime? fechaVence;
        string codigoLote;
        decimal unidadesTeorica;
        CompraFacturaDetalle facturaItem;
        OrdenCompraDetalle ordenCompraItem;
        Producto.Module.BusinessObjects.Producto producto;
        InventarioMovimiento movimiento;
        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public long Oid => oid;

        [Association("InventarioMovimiento-Detalles")]
        public InventarioMovimiento Movimiento
        {
            get => movimiento;
            set => SetPropertyValue(nameof(Movimiento), ref movimiento, value);
        }


        [XafDisplayName("Orden Compra Item"), VisibleInListView(false)]
        public OrdenCompraDetalle OrdenCompraItem
        {
            get => ordenCompraItem;
            set => SetPropertyValue(nameof(OrdenCompraItem), ref ordenCompraItem, value);
        }

        [Browsable(false), XafDisplayName("Factura Detalle"), VisibleInListView(false)]
        public CompraFacturaDetalle FacturaItem
        {
            get => facturaItem;
            set => SetPropertyValue(nameof(FacturaItem), ref facturaItem, value);
        }

        [XafDisplayName("Producto"), RuleRequiredField("InventarioMovimientoDetalle.Producto_Requerido", "Save")]
        public Producto.Module.BusinessObjects.Producto Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        /// <summary>
        /// Total de unidades para el inventario según documentos
        /// </summary>
        [DbType("numeric(12,2)"), Persistent(nameof(UnidadesTeorica)), XafDisplayName("Unidades Teórica"), Index(6)]
        public decimal UnidadesTeorica
        {
            get => unidadesTeorica;
            set => SetPropertyValue(nameof(UnidadesTeorica), ref unidadesTeorica, value);
        }

        /// <summary>
        /// Total de unidades reales al momento de ingresar o salir del inventario
        /// </summary>
        [DbType("numeric(12,2)"), Persistent(nameof(Unidades)), XafDisplayName("Unidades Reales"), Index(6)]
        public decimal Unidades
        {
            get => unidades;
            set => SetPropertyValue(nameof(Unidades), ref unidades, value);
        }

        [Size(48), DbType("varchar(48)"), XafDisplayName("Código Lote")]
        [RuleRequiredField("InventarioMovimientoDetalle.CodigoLote_Requerido", DefaultContexts.Save,
                 TargetCriteria = "[Producto.Categoria.MetodoCosteo] In (1, 2)",
                 CustomMessageTemplate = "Cuando el método de costeo es PEPS o UEPS, el código de lote es requerido")]
        [Indexed(Name = "idxCodigoLote_InventarioMovimientoDetalle")]
        public string CodigoLote
        {
            get => codigoLote;
            set => SetPropertyValue(nameof(CodigoLote), ref codigoLote, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Fabricación")]
        public DateTime? FechaFabricacion
        {
            get => fechaFabricacion;
            set => SetPropertyValue(nameof(FechaFabricacion), ref fechaFabricacion, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Vencimiento")]
        [RuleRequiredField("InventarioMovimientoDetalle.FechaVenceLote_Requerido", DefaultContexts.Save,
            TargetCriteria = "[Producto.Categoria.MetodoCosteo] In (1, 2)",
            CustomMessageTemplate = "Cuando el método de costeo es PEPS o UEPS, la fecha de vencimiento es requerida")]
        [RuleValueComparison("InventarioMovimientoDetalle.FechaVenceLote > FechaFabricacion", DefaultContexts.Save,
            ValueComparisonType.GreaterThan, "[FechaFabricacion]", ParametersMode.Expression, SkipNullOrEmptyValues = true,
            TargetCriteria = "[Producto.Categoria.MetodoCosteo] In (1, 2)",
            CustomMessageTemplate = "La Fecha de Vencimiento debe ser mayor que la Fecha de Fabricación")]
        public DateTime? FechaVence
        {
            get => fechaVence;
            set => SetPropertyValue(nameof(FechaVence), ref fechaVence, value);
        }

        /// <summary>
        /// Cantidad anulada
        /// </summary>
        [PersistentAlias(nameof(cantidadAnulada))]
        [XafDisplayName("Cantidad Anulada"), ModelDefault("DisplayFormat", "{0:N2}"), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal? CantidadAnulada => cantidadAnulada;

        #endregion


        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}