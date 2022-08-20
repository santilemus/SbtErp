using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.CxC.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al detalle de los documentos de venta
    /// </summary>
    /// <remarks>
    /// Info del atributo OptimisticLockingReadBehavior en https://docs.devexpress.com/XPO/DevExpress.Xpo.Session.OptimisticLockingReadBehavior
    /// </remarks>
    [ModelDefault("Caption", "Venta Detalle"), DefaultProperty(nameof(Producto)), NavigationItem(false),
        Persistent(nameof(VentaDetalle)), CreatableItem(false)]
    [Appearance("Venta Detalle - Credito Fiscal", AppearanceItemType = "Any", TargetItems = "PrecioConIva",
        Criteria = "[Venta.TipoFactura.Codigo] == 'COVE01'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Venta Detalle - Consumidor Final y Ticket", AppearanceItemType = "Any", TargetItems = "IVA",
        Criteria = "[Venta.TipoFactura.Codigo] In ('COVE02', 'COVE04', 'COVE05')", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Venta Detalle - Factura Exportacion", AppearanceItemType = "ViewItem", TargetItems = "IVA;Exenta;NoSujeta",
        Criteria = "[Venta.TipoFactura.Codigo] == 'COVE03'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    // la siguiente regla es para habilitar la edicion de estas propiedades solo cuando es un objeto nuevo. Si tiene que modificarlos
    // debe borrarlos y crear nuevos, o poner la cantidad a cero
    //[Appearance("Venta Detalle - Nuevo Registro", AppearanceItemType = "Any", Enabled = true, TargetItems = "Bodega;CodigoBarra",
    //    Criteria = "IsNewObject(This) && [Producto.Categoria.Clasificacion] <= 3")]
    [OptimisticLockingReadBehavior(OptimisticLockingReadBehavior.Default, true)]
    //[ImageName("BO_Contact")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class VentaDetalle : XPCustomFacturaDetalle
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VentaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            costo = 0.0m;
            fProdChanged = false;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        InventarioLote lote;
        bool fProdChanged;

        SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad bodega;
        Venta venta;
        decimal costo;
        ProductoCodigoBarra codigoBarra;

        [Size(20), DbType("varchar(20)"), XafDisplayName("Código Barra"), Index(1), ToolTip("Código de barra del producto")]
        public ProductoCodigoBarra CodigoBarra
        {
            get => codigoBarra;
            set
            {
                bool changed = SetPropertyValue(nameof(CodigoBarra), ref codigoBarra, value);
                if (!IsLoading && !IsSaving && changed && CodigoBarra.Producto != Producto)
                {
                    Producto = CodigoBarra.Producto;
                    /// para evitar asignar el codigo de barra.
                    /// El codigo de barra debe ser asignado desde el producto, solamente cuando el usuario selecciona y
                    /// cambia directamente el producto
                    fProdChanged = true;
                    OnChanged(nameof(Producto));
                }
            }
        }

        /// <summary>
        /// Bodega de la cual salen los productos, cuando no es servicios
        /// </summary>
        [XafDisplayName("Bodega"), RuleRequiredField("VentaDetalle.Bodega_Requerida", "Save",
            TargetCriteria = "!([Producto.Categoria.Clasificacion] In (4, 5))")]
        [Appearance("VentaDetalle_Bodega_Hide", AppearanceItemType = "ViewItem", Criteria = "[Producto.Categoria.Clasificacion] > 3",
            Visibility = ViewItemVisibility.Hide, Context = "Any")]
        [Appearance("VentaDetalle_Bodega_Show", AppearanceItemType = "ViewItem", Criteria = "[Producto.Categoria.Clasificacion] <= 3",
            Visibility = ViewItemVisibility.Show, Context = "Any")]
        public SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad Bodega
        {
            get => bodega;
            set => SetPropertyValue(nameof(Bodega), ref bodega, value);
        }

        [PersistentAlias("[PrecioUnidad] + [Iva]"), XafDisplayName("Precio Con Iva"), Index(6)]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal PrecioConIva => Convert.ToDecimal(EvaluateAlias(nameof(PrecioConIva)));

        [XafDisplayName("Costo"), Index(11)]
        [ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        [Persistent(nameof(Costo)), DbType("numeric(14,6)")]
        [ModelDefault("AllowEdit", "false"), Browsable(false)]
        public decimal Costo
        {
            get => costo;
            set => SetPropertyValue(nameof(Costo), ref costo, value);
        }


        [Association("Venta-Detalles"), XafDisplayName("Venta"), Index(12)]
        public Venta Venta
        {
            get => venta;
            set
            {
                Venta oldVenta = venta;
                bool changed = SetPropertyValue(nameof(Venta), ref venta, value);
                if (!IsLoading && !IsSaving && changed && oldVenta != venta)
                {
                    oldVenta = oldVenta ?? venta;
                    oldVenta.UpdateTotalExenta(true);
                    oldVenta.UpdateTotalGravada(true);
                    oldVenta.UpdateTotalIva(true);
                    oldVenta.UpdateTotalNoSujeta(true);
                }
            }
        }

        [DbType("int"), XafDisplayName("Lote")]
        [Appearance("VentaDetalle_Lote_Hide", AppearanceItemType = "ViewItem",
            Criteria = "[Producto.Categoria.Clasificacion] > 3 || [Producto.Categoria.MetodoCosteo] In (0, 3, 4)",
            Visibility = ViewItemVisibility.Hide, Context = "*")]
        public InventarioLote Lote
        {
            get => lote;
            set => SetPropertyValue(nameof(Lote), ref lote, value);
        }

        /// <summary>
        /// regla validar disponibilidad en el lote, cuando el metodo de costeo es PEPS, UEPS
        /// </summary>
        [Browsable(false)]
        [RuleFromBoolProperty("VentaDetalle.Cantidad <= Disponible en el Lote", DefaultContexts.Save,
            TargetCriteria = "[Lote] is not null && [Producto.Categoria.MetodoCosteo] In (1, 2) && [Producto.Categoria.Clasificacion] < 4",
            CustomMessageTemplate = "La Cantidad a vender debe ser menor o igual al dispoible en el lote")]
        public bool EsCantidadMenorIgualDisponibleEnLote
        {
            get
            {
                decimal cantVta = Venta.Detalles.Where(x => x.Bodega == Bodega && x.Producto == Producto && x.Lote == Lote).Sum(x => x.Cantidad);
                decimal totVeta = Convert.ToDecimal(Session.Evaluate<VentaDetalle>(CriteriaOperator.Parse("Sum([Cantidad])"),
                    CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ? && [Lote.Oid] == ? && [Oid] != ?", Bodega.Oid, Producto.Oid, Lote.IngresoDetalle, Oid)));
                return cantVta <= (Lote.Entrada - Lote.Salida);
            }
        }

        /// <summary>
        /// Regla validar disponibilidad en el inventario cuando el metodo no es PEPS o UEPS. Ver !([Producto.Categoria.MetodoCosteo] In (1, 2))
        /// </summary>
        [Browsable(false)]
        [RuleFromBoolProperty("VentaDetalle.Cantidad <= Disponible Inventario", DefaultContexts.Save,
           TargetCriteria = "[Bodega.Oid] is not null && !([Producto.Categoria.MetodoCosteo] In (1, 2)) && [Producto.Categoria.Clasificacion] < 4",
           CustomMessageTemplate = "La Cantidad a vender debe ser menor o igual al dispoible en el lote")]
        public bool EsCantidadMenorIgualExistenciaInventario
        {
            get
            {
                decimal cantVta = Venta.Detalles.Where(x => x.Bodega == Bodega && x.Producto == Producto).Sum(x => x.Cantidad);
                decimal existencia = Convert.ToDecimal(Session.Evaluate<Inventario.Module.BusinessObjects.Inventario>(
                    CriteriaOperator.Parse("Sum(Iif([TipoMovimiento.Operacion] In (0, 1), [Cantidad], -[Cantidad]))"),
                    CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ?", Bodega.Oid, Producto.Oid)));
                return cantVta <= existencia;
            }
        }

        #endregion

        #region Colleciones
        [Association("VentaDetalle-CxCDocumentoDetalles"), XafDisplayName("CxC Detalles"), Index(0)]
        public XPCollection<CxCDocumentoDetalle> CxCDocumentoDetalles => GetCollection<CxCDocumentoDetalle>(nameof(CxCDocumentoDetalles));

        #endregion

        #region Metodos

        //protected override void OnSaving()
        //{
        //    bool isObjectMarkedDeleted = this.Session.IsObjectMarkedDeleted(this);
        //    bool isNewObject = this.Session.IsNewObject(this);
        //    XPMemberInfo cantidadInfo = this.Session.GetClassInfo(this).GetMember(nameof(Cantidad));
        //    XPMemberInfo productoInfo = this.Session.GetClassInfo(this).GetMember(nameof(Producto));
        //}

        protected override void DoProductoChanged(bool forceChangeEvents, Producto.Module.BusinessObjects.Producto oldValue)
        {
            base.DoProductoChanged(forceChangeEvents, oldValue);
            // intenta obtener codigo de barra y asignarlo solo cuando el usuario cambio el producto, por eso !fProdChanged, Si antes modifico codigo de barra no se ejecuta esta parte
            if (!fProdChanged)
            {
                CodigoBarra = Producto.CodigosBarra.FirstOrDefault<ProductoCodigoBarra>(ProductoCodigoBarra => ProductoCodigoBarra.Producto.Oid == Producto.Oid);
                OnChanged(nameof(CodigoBarra));
            }
            fProdChanged = false;
            if (Producto.Precios.Count > 0)
            {
                decimal pu = Producto.Precios.FirstOrDefault<ProductoPrecio>(p => p.Producto.Oid == Producto.Oid &&
                             p.Tipo.Categoria == CategoriaLista.TipoPrecio && p.Tipo.Codigo == "TPR001" && p.Activo == true).PrecioUnitario;
                if (PrecioUnidad == 0.0m)
                    PrecioUnidad = pu;                
            }
            Costo = ObtenerCosto();
            OnChanged(nameof(Costo));
        }

        private decimal ObtenerCosto()
        {
            decimal valor = 0.0m;
            if (Producto.Categoria != null)
            {
                if (Producto.Categoria.MetodoCosteo == EMetodoCosteoInventario.Promedio)
                    valor = Producto.CostoPromedio;
                else if (Producto.Categoria.MetodoCosteo == EMetodoCosteoInventario.UEPS ||
                         Producto.Categoria.MetodoCosteo == EMetodoCosteoInventario.PEPS)
                {
                    InventarioLote lotep = ObtenerLote();
                    Lote = lotep;
                    valor = lotep != null ? lotep.Costo : Producto.CostoPromedio;  // sino se encuentra el lote retorna costo promedio
                }
                else if (Producto.Categoria.MetodoCosteo == EMetodoCosteoInventario.Unitario)
                    valor = PrecioUnidad;
            }
            return valor;
        }

        public InventarioLote ObtenerLote()
        {
            InventarioLote lotep = null;
            Producto.Lotes.Criteria = CriteriaOperator.Parse("[Producto.Oid] == ? && [Entrada] > [Salida]", Producto.Oid);
            if (Producto.Categoria.MetodoCosteo == EMetodoCosteoInventario.PEPS)
                lotep = Producto.Lotes.Where(x => x.Producto.Oid == Producto.Oid && x.Entrada > x.Salida)
                                         .OrderBy(x => x.FechaVence).FirstOrDefault(x => (x.Entrada - x.Salida) >= Cantidad);
            else
                lotep = Producto.Lotes.Where(x => x.Producto.Oid == Producto.Oid && x.Entrada > x.Salida)
                         .OrderBy(x => x.FechaVence).LastOrDefault(x => (x.Entrada - x.Salida) >= Cantidad);
            return lotep;
        }

        protected override void DoCantidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoCantidadChanged(forceChangeEvents, oldValue);
            decimal oldPrecio = PrecioUnidad;
            var itemPrecio = ObtenerPrecio();
            if (itemPrecio != null)
            {
                if (oldPrecio != itemPrecio.PrecioUnitario)
                    PrecioUnidad = itemPrecio.PrecioUnitario;
            }
            DoPrecioUnidadChanged(true, oldPrecio);
        }

        /// <summary>
        /// obtener el BO del precio que aplica para el detalle del documento de venta
        /// </summary>
        /// <returns></returns>
        private ProductoPrecio ObtenerPrecio()
        {
            if (Producto == null || Producto.Precios.Count == 0)
                return null;
            ProductoPrecio itemPrecio = Producto.Precios.Where<ProductoPrecio>(x => x.Producto == Producto &&
            Venta.Fecha >= x.HoraDesde && Venta.Fecha <= x.HoraHasta && Cantidad >= x.CantidadDesde && Cantidad <= x.CantidadHasta && x.Activo == true).
            FirstOrDefault();
            if (itemPrecio == null)
            {
                itemPrecio = Producto.Precios.Where<ProductoPrecio>(x => x.Producto == Producto &&
                             Cantidad >= x.CantidadDesde && Cantidad <= x.CantidadHasta && x.Activo == true).FirstOrDefault();
                if (itemPrecio == null)
                    itemPrecio = Producto.Precios.FirstOrDefault<ProductoPrecio>(x => x.Producto == Producto && x.Activo == true);
            }
            return itemPrecio;
        }

        protected override void DoPrecioUnidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoPrecioUnidadChanged(forceChangeEvents, oldValue);
            if (Venta == null || Producto == null)
                return;
            if (Producto.Categoria.ClasificacionIva == EClasificacionIVA.Gravado)
            {
                gravada = Math.Round(Cantidad * PrecioUnidad, 2);
                // cuando la factura es diferente de exportacion ("COVE03")
                if (Venta.Cliente.TipoContribuyente == ETipoContribuyente.Gravado && Venta.TipoFactura.Codigo != "COVE03")
                    iva = Math.Round(Convert.ToDecimal(Gravada) * this.Producto.Categoria.PorcentajeIVA, 2);
            }
            else
                exenta = Math.Round(Cantidad * PrecioUnidad, 2);
            if (forceChangeEvents)
            {
                OnChanged(nameof(PrecioUnidad), oldValue, PrecioUnidad);
                /// Info https://supportcenter.devexpress.com/ticket/details/ka18699/how-to-implement-dependent-and-calculated-properties-in-xpo#
                OnChanged(nameof(Gravada));
                OnChanged(nameof(Iva));
                OnChanged(nameof(Exenta));
            }
            Venta.UpdateTotalExenta(true);
            Venta.UpdateTotalGravada(true);
            Venta.UpdateTotalIva(true);
            Venta.UpdateTotalNoSujeta(true);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}