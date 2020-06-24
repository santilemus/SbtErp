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
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class VentaDetalle : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VentaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            fProdChanged = false;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        SBT.Apps.Base.Module.BusinessObjects.EmpresaUnidad bodega;
        bool fProdChanged = false;

        #region Propiedades

        Venta venta;
        [Persistent(nameof(Costo)), DbType("numeric(14,6)")]
        decimal costo = 0.0m;
        [DbType("numeric(14,2)"), Persistent(nameof(NoSujeta))]
        decimal noSujeta = 0.0m;
        [DbType("numeric(14,2)"), Persistent(nameof(Gravada))]
        decimal gravada = 0.0m;
        [DbType("numeric(14,2)"), Persistent(nameof(Exenta))]
        decimal exenta = 0.0m;
        [Persistent(nameof(IVA)), DbType("numeric(14,2)")]
        decimal iVA = 0.0m;
        decimal precioUnidad;
        decimal cantidad = 1.0m;
        SBT.Apps.Producto.Module.BusinessObjects.Producto producto;
        ProductoCodigoBarra codigoBarra;
        [Persistent(nameof(Oid)), DbType("bigint"), Key(AutoGenerate = true)]
        long oid = -1;
        [Persistent(nameof(CantidadAnulada)), DbType("numeric(12,2)")]
        decimal? cantidadAnulada = null;

        [PersistentAlias(nameof(oid)), XafDisplayName(nameof(Oid)), Index(0)]
        public long Oid => oid;

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

        [XafDisplayName("Producto"), Index(2), RuleRequiredField("VentaDetalle.Producto_Requerido", DefaultContexts.Save)]
        public SBT.Apps.Producto.Module.BusinessObjects.Producto Producto
        {
            get => producto;
            set
            {
                bool changed = SetPropertyValue(nameof(Producto), ref producto, value);
                // se intenta obtener el codigo de barra y asignarlo solo cuando el usuario cambio el producto, por eso !fProdChanged
                if (!IsLoading && !IsSaving && changed && !fProdChanged)
                    CodigoBarra = Producto.CodigosBarra.FirstOrDefault<ProductoCodigoBarra>(ProductoCodigoBarra => ProductoCodigoBarra.Producto.Oid == Producto.Oid);
                fProdChanged = false;
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

        [DbType("numeric(12,2)"), XafDisplayName("Cantidad"), Index(3), VisibleInLookupListView(true),
            RuleValueComparison("VentaDetalle.Cantidad >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0.0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Cantidad
        {
            get => cantidad;
            set => SetPropertyValue(nameof(Cantidad), ref cantidad, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Precio Unidad"), Index(5),
            RuleValueComparison("VentaDetalle.PrecioUnidad >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0.0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal PrecioUnidad
        {
            get => precioUnidad;
            set => SetPropertyValue(nameof(PrecioUnidad), ref precioUnidad, value);
        }

        [PersistentAlias("[PrecioUnidad] + [IVA]"), XafDisplayName("Precio Con Iva"), Index(6)]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal PrecioConIva => Convert.ToDecimal(EvaluateAlias(nameof(PrecioConIva)));

        [XafDisplayName("Iva"), Index(7), PersistentAlias(nameof(iVA))]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal IVA => iVA;

        [DbType("numeric(14,2)"), XafDisplayName("Venta Exenta"), PersistentAlias(nameof(exenta)), Index(8)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Exenta => exenta;

        [DbType("numeric(14,2)"), XafDisplayName("Venta Gravada"), PersistentAlias(nameof(gravada)), Index(9)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Gravada => gravada;

        [XafDisplayName("Venta No Sujeta"), Index(10), PersistentAlias(nameof(noSujeta)), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal NoSujeta => noSujeta;

        [PersistentAlias(nameof(costo)), XafDisplayName("Costo"), Index(11), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal Costo => costo;


        [Association("Venta-Detalles"), XafDisplayName("Venta"), Index(12)]
        public Venta Venta
        {
            get => venta;
            set => SetPropertyValue(nameof(Venta), ref venta, value);
        }

        
        [PersistentAlias(nameof(cantidadAnulada)), XafDisplayName("Cantidad Anulada"), Index(13), VisibleInListView(false)]
        public decimal ? CantidadAnulada => cantidadAnulada;

        #endregion

        #region Colleciones


        #endregion

        #region Metodos
        private void CalcularMontoVenta()
        {
            if (Producto != null && Cantidad > 0.0m && PrecioUnidad > 0.0m)
            {
                if (Producto.Clasificacion == EClasificacionIVA.Gravado)
                {
                    TributoProducto tp = Session.FindObject<TributoProducto>(
                        CriteriaOperator.Parse("Producto.Oid == ? && [Tributo][[^.NombreAbreviado] == 'IVA']", Producto.Oid));
                    if (tp != null)
                    {
                        ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Venta)), tp.Tributo.Formula);
                        iVA = Math.Round(Convert.ToDecimal(eval.Evaluate(this)), 2);

                    }
                }
                else
                {
                    gravada = 0.0m;
                    iVA = 0.0m;
                    exenta = Math.Round(Cantidad * precioUnidad, 2);
                    noSujeta = 0.0m;
                }
            }
        }

        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}