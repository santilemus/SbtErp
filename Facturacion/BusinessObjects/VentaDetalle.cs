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
    [DefaultClassOptions, ModelDefault("Caption", "Venta Detalle"), DefaultProperty(nameof(Producto)), NavigationItem(false),
        Persistent(nameof(VentaDetalle))]
    [Appearance("Venta Detalle - Credito Fiscal", AppearanceItemType = "ViewItem", TargetItems = "PrecioConIva", 
        Criteria = "[Venta.Tipo.Codigo] == 'COVE01'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Venta Detalle - Consumidor Final y Ticket", AppearanceItemType = "ViewItem", TargetItems = "IVA",
        Criteria = "[Venta.Tipo.Codigo] In ('COVE02', 'COVE04', 'COVE05')", Context = "Any", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Venta Detalle - Factura Exportacion", AppearanceItemType = "ViewItem", TargetItems = "IVA;Exenta;NoSujeta",
        Criteria = "[Venta.Tipo.Codigo] == 'COVE03'", Context = "Any", Visibility = ViewItemVisibility.Hide)]
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
                    TributoProducto tp = Session.FindObject<TributoProducto>(
                        CriteriaOperator.Parse("Producto.Oid == ? && [Tributo][[^.NombreAbreviado] == 'IVA']", Producto.Oid));
                    if (tp != null)
                    {
                        ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Venta)), tp.Tributo.Formula);
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

        /// <summary>
        /// Ejecuta metodos que actualiza la informacion en otros BO, como el inventario
        /// </summary>
        /// <remarks>
        /// Ver mas info en: https://supportcenter.devexpress.com/ticket/details/t685638/get-the-context-of-an-update-and-trigger-another-db-operation
        /// </remarks>

        protected override void OnSaving()
        {
            bool isObjectMarkedDeleted = this.Session.IsObjectMarkedDeleted(this);
            bool isNewObject = this.Session.IsNewObject(this);
            XPMemberInfo cantidadInfo = this.Session.GetClassInfo(this).GetMember(nameof(Cantidad));
            XPMemberInfo productoInfo = this.Session.GetClassInfo(this).GetMember(nameof(Producto));
            if (isNewObject)
                ActualizarInventario(Bodega.Oid, Venta.Oid, Producto.Oid, Cantidad);
            if (cantidadInfo.GetModified(this))
            {
                decimal cantidadOld = Convert.ToDecimal(cantidadInfo.GetOldValue(this));
                decimal cantidadNew = Convert.ToDecimal(cantidadInfo.GetValue(this));
                ActualizarInventario(Bodega.Oid, Venta.Oid, Producto.Oid, cantidadNew - cantidadOld);
            }
            // pendiente de implementar cuando se cambia el producto

            if (isObjectMarkedDeleted)
                ActualizarInventario(Bodega.Oid, Venta.Oid, Producto.Oid, -Cantidad);
            base.OnSaving();
        }

        protected override void DoProductoChanged(bool forceChangeEvents, Producto.Module.BusinessObjects.Producto oldValue)
        {
            base.DoProductoChanged(forceChangeEvents, oldValue);
            // intenta obtener codigo de barra y asignarlo solo cuando el usuario cambio el producto, por eso !fProdChanged, Si antes modifico codigo de barra no se ejecuta esta parte
            if (!fProdChanged)
                CodigoBarra = Producto.CodigosBarra.FirstOrDefault<ProductoCodigoBarra>(ProductoCodigoBarra => ProductoCodigoBarra.Producto.Oid == Producto.Oid);
            fProdChanged = false;

            // asignamos el costo
            costo = ObtenerCosto();
        }

        protected override void DoCantidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoCantidadChanged(forceChangeEvents, oldValue);
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

        /// <summary>
        /// Metodo para realizar la actualizacion del inventario. Evaluar si queda aca o se lleva al BO de inventario
        /// porque el mismo metodo se necesitara para las compras, devoluciones y otros documentos internos que afectan inventario
        /// </summary>
        /// <param name="OidBodega">Bodega cuyo inventario será afectado</param>
        /// <param name="OidDocumento">Oid del documento de referencia. Evaluar si se pasa el Oid de la Venta o el Oid del detalle de la venta</param>
        /// <param name="OidProducto">Oid del Producto del inventario a actualizar</param>
        /// <param name="ACantidad">Oid de la cantidad del inventario que sera afectado</param>
        /// <remarks>
        /// Pendiente de completar, faltan los BO de Inventario 
        /// También esta pendiente cuando sean PEPS o UEPS porque en ese caso falta alli el lote
        /// OJO: Debe considerar los casos de Nuevo Registro, Modificacion y Eliminacion
        /// </remarks>
        protected override void ActualizarInventario(int OidBodega, long OidDocumento, int OidProducto, decimal ACantidad)
        {
            base.ActualizarInventario(OidBodega, OidDocumento, OidProducto, ACantidad);
        }

        private decimal ObtenerCosto()
        {
            switch (Producto.Categoria.MetodoCosteo)
            {
                case EMetodoCosteoInventario.Promedio:
                    return Producto.CostoPromedio;
                case EMetodoCosteoInventario.PEPS:
                    var lotep = Producto.Lotes.Where(x => x.Producto.Oid == Producto.Oid && x.Entrada > x.Salida)
                                             .OrderBy(x => x.Fecha).FirstOrDefault(x => (x.Entrada - x.Salida) >= Cantidad);
                    return (lotep != null) ? lotep.Costo : 0.0m;
                case EMetodoCosteoInventario.UEPS:
                    var loteu = Producto.Lotes.Where(x => x.Producto.Oid == Producto.Oid && x.Entrada > x.Salida)
                                             .OrderBy(x => x.Fecha).LastOrDefault(x => (x.Entrada - x.Salida) >= Cantidad);
                    return (loteu != null) ? loteu.Costo : 0.0m;
                default:
                    return Producto.Precios.FirstOrDefault<ProductoPrecio>(p => p.Producto.Oid == Producto.Oid && p.Activo == true).PrecioUnitario;
            }
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}