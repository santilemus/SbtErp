using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp.ConditionalAppearance;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using DevExpress.Xpo.Metadata;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al detalle de los documentos de venta
    /// </summary>
    /// <remarks>
    /// Info del atributo OptimisticLockingReadBehavior en https://docs.devexpress.com/XPO/DevExpress.Xpo.Session.OptimisticLockingReadBehavior
    /// </remarks>
    [DefaultClassOptions, ModelDefault("Caption", "Venta Detalle"), DefaultProperty(nameof(Producto)), NavigationItem(false),
        Persistent(nameof(VentaDetalle))]
    [Appearance("Venta Detalle - Credito Fiscal", AppearanceItemType = "ViewItem", TargetItems = "PrecioConIva",
        Criteria = "[Venta.Tipo.Codigo] == 'COVE01'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Venta Detalle - Consumidor Final y Ticket", AppearanceItemType = "ViewItem", TargetItems = "IVA",
        Criteria = "[Venta.Tipo.Codigo] In ('COVE02', 'COVE04', 'COVE05')", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Venta Detalle - Factura Exportacion", AppearanceItemType = "ViewItem", TargetItems = "IVA;Exenta;NoSujeta",
        Criteria = "[Venta.Tipo.Codigo] == 'COVE03'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    // la siguiente regla es para habilitar la edicion de estas propiedades solo cuando es un objeto nuevo. Si tiene que modificarlos
    // debe borrarlos y crear nuevos, o poner la cantidad a cero
    [Appearance("Venta Detalle - Nuevo Registro", AppearanceItemType = "Any", Enabled = true, TargetItems = "Bodega;Producto;CodigoBarra",
        Criteria = "IsNewObject(This)")]
    [OptimisticLockingReadBehavior(OptimisticLockingReadBehavior.Default, true)]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
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

        ProductoLote lote;
        bool fProdChanged;

        SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad bodega;
        Venta venta;
        [Persistent(nameof(Costo)), DbType("numeric(14,6)")]
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
        public SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad Bodega
        {
            get => bodega;
            set => SetPropertyValue(nameof(Bodega), ref bodega, value);
        }

        [PersistentAlias("[PrecioUnidad] + [IVA]"), XafDisplayName("Precio Con Iva"), Index(6)]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal PrecioConIva => Convert.ToDecimal(EvaluateAlias(nameof(PrecioConIva)));

        [PersistentAlias(nameof(costo)), XafDisplayName("Costo"), Index(11), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal Costo => costo;


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
        public ProductoLote Lote
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
                    CriteriaOperator.Parse("[Bodega.Oid] == ? && [Producto.Oid] == ? && [Lote.Oid] == ? && [Oid] != ?", Bodega.Oid, Producto.Oid, Lote.Oid, Oid)));
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


        #endregion

        #region Metodos
        private void CalcularMontoVenta()
        {
            if (Producto != null && Cantidad > 0.0m && PrecioUnidad > 0.0m)
            {
                if (Producto.Categoria.ClasificacionIva == EClasificacionIVA.Gravado)
                {
                    TributoCategoria tp = Session.FindObject<TributoCategoria>(
                        CriteriaOperator.Parse("Producto.Categoria.Oid == ? && [Tributo][[^.NombreAbreviado] == 'IVA']", Producto.Categoria.Oid));
                    if (tp != null)
                    {
                        ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Venta)), ((TributoRegla)tp.Tributo).Formula);
                        iva = Math.Round(Convert.ToDecimal(eval.Evaluate(this)), 2);

                    }
                }
                else
                {
                    gravada = 0.0m;
                    iva = 0.0m;
                    exenta = Math.Round(Cantidad * PrecioUnidad, 2);
                    noSujeta = 0.0m;
                }
            }
        }

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
            switch (Producto.Categoria.MetodoCosteo)
            {
                case EMetodoCosteoInventario.Promedio:
                    costo = Producto.CostoPromedio;
                    break;
                case EMetodoCosteoInventario.Unitario:
                    costo = Producto.Precios.FirstOrDefault<ProductoPrecio>(p => p.Producto.Oid == Producto.Oid && p.Activo == true).PrecioUnitario;
                    break;
                default:
                    {
                        ProductoLote lotep = ObtenerLote();
                        Lote = lotep;
                        costo = lotep != null ? lotep.Costo : Producto.CostoPromedio;
                        break;
                    }
            }
            OnChanged(nameof(Costo));
        }

        protected override void DoCantidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoCantidadChanged(forceChangeEvents, oldValue);
            decimal oldPrecio = PrecioUnidad;
            DoPrecioUnidadChanged(true, oldPrecio);
        }

        protected override void DoPrecioUnidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoPrecioUnidadChanged(forceChangeEvents, oldValue);
            if (Venta == null)
                return;
            OnPrecioUnidadChanged(Cantidad, Venta.TipoFactura.Codigo);
            if (forceChangeEvents)
                OnChanged(nameof(PrecioUnidad), oldValue, PrecioUnidad);
            Venta.UpdateTotalExenta(true);
            Venta.UpdateTotalGravada(true);
            Venta.UpdateTotalIva(true);
            Venta.UpdateTotalNoSujeta(true);
        }

        private ProductoLote ObtenerLote()
        {
            ProductoLote lotep = null;
            Producto.Lotes.Criteria = CriteriaOperator.Parse("[Producto.Oid] == ? && [Entrada] > [Salida]", Producto.Oid);
            if (Producto.Categoria.MetodoCosteo == EMetodoCosteoInventario.PEPS)
                lotep = Producto.Lotes.Where(x => x.Producto.Oid == Producto.Oid && x.Entrada > x.Salida)
                                         .OrderBy(x => x.Fecha).FirstOrDefault(x => (x.Entrada - x.Salida) >= Cantidad);
            else
                lotep = Producto.Lotes.Where(x => x.Producto.Oid == Producto.Oid && x.Entrada > x.Salida)
                         .OrderBy(x => x.Fecha).LastOrDefault(x => (x.Entrada - x.Salida) >= Cantidad);
            return lotep;
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}