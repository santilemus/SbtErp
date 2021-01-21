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
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al detalle de la orden de compra
    /// </summary>
    
    [DefaultClassOptions, ModelDefault("Caption", "Orden Compra Detalle"), NavigationItem(false), CreatableItem(false),
        DefaultProperty(nameof(Producto)), Persistent(nameof(OrdenCompraDetalle))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class OrdenCompraDetalle : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public OrdenCompraDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string descripcion;
        int? diasGarantia;
        string modelo;
        string marca;
        [Persistent(nameof(CantidadAnulada))]
        decimal? cantidadAnulada;
        [Persistent(nameof(Total))]
        decimal total;
        decimal costoUnidad;
        [DbType("numeric(12,2)"), Persistent(nameof(Unidades))]
        decimal unidades;
        decimal cantidad;
        Presentacion presentacion;
        Producto.Module.BusinessObjects.Producto producto;
        OrdenCompra ordenCompra;
        [DbType("bigint"), Persistent(nameof(Oid)), Key(AutoGenerate = true), XafDisplayName("Oid")]
        long oid = -1;

        [DbType("numeric(14,2)"), Persistent(nameof(Exenta))]
        decimal exenta;
        [DbType("numeric(14,2)"), Persistent(nameof(Gravada))]
        decimal gravada;
        [DbType("numeric(14,2)"), Persistent(nameof(Iva))]
        decimal iva;

        /// <summary>
        /// Oid del detalle de la orden de compra. Es la llave primaria
        /// </summary>
        [PersistentAlias(nameof(oid)), Index(0)]
        public long Oid => oid;

        /// <summary>
        /// La asociacion a la orden de compra
        /// </summary>
        [Association("OrdenCompra-Detalles"), XafDisplayName("Orden Compra"), Index(1)]
        public OrdenCompra OrdenCompra
        {
            get => ordenCompra;
            set => SetPropertyValue(nameof(OrdenCompra), ref ordenCompra, value);
        }

        /// <summary>
        /// Producto requerido en la orden de compra, cuando el tipo de compra es producto para inventario. Solo en 
        /// ese caso tiene valor y es requerido
        /// </summary>
        [XafDisplayName("Producto"), Index(2)]
        [ToolTip("Producto requerido al proveedor. Solo es obligatorio cuando el tipo de orden es Producto")]
        public Producto.Module.BusinessObjects.Producto Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        /// <summary>
        /// Presentacion o empaque. Importante porque cada presentacion contiene diferente cantidad de unidades
        /// </summary>
        [XafDisplayName("Presentación"), Index(3)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [ToolTip("Presentación del Producto. Cada presentacion tiene diferente cantidad de unidades")]
        public Presentacion Presentacion
        {
            get => presentacion;
            set => SetPropertyValue(nameof(Presentacion), ref presentacion, value);
        }

        /// <summary>
        /// Descripcion del bien o servicio que se esta contratando
        /// </summary>
        [Size(250), DbType("varchar(250)"), XafDisplayName("Descripción"), Index(4)]
        [ToolTip("Util solo cuando se trata de ordenes de compra de bienes para activo fijo o servicios")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        /// <summary>
        /// Cantidad por presentacion
        /// </summary>
        [XafDisplayName("Presentación Cantidad"), Index(5)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [ToolTip("Cantidad por presentación")]
        [RuleValueComparison("OrdenCompraDetalle.Cantidad >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = true)]
        public decimal Cantidad
        {
            get => cantidad;
            set
            {
                bool changed = SetPropertyValue(nameof(Cantidad), ref cantidad, value);
                if (!IsLoading && !IsSaving && changed)
                    unidades = Math.Round(value * this.Presentacion.Unidades, 2);
            }
        }

        /// <summary>
        /// Total de unidades para el inventario
        /// </summary>
        [PersistentAlias(nameof(unidades)), XafDisplayName("Unidades"), Index(6)]
        public decimal Unidades => unidades;

        /// <summary>
        /// Costo por unidad
        /// </summary>
        [DbType("numeric(14, 4)"), XafDisplayName("Costo Unidad"), Index(7)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [ToolTip("Cantidad por presentación")]
        [RuleValueComparison("OrdenCompraDetalle.CostoUnidad > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public decimal CostoUnidad
        {
            get => costoUnidad;
            set => SetPropertyValue(nameof(CostoUnidad), ref costoUnidad, value);
        }

        /// <summary>
        /// Compra exenta
        /// </summary>
        [DbType("numeric(14,2)"), XafDisplayName("Exento"), PersistentAlias(nameof(exenta)), Index(8)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Exenta => exenta;

        /// <summary>
        /// Compra gravada
        /// </summary>
        [DbType("numeric(14,2)"), XafDisplayName("Gravado"), PersistentAlias(nameof(gravada)), Index(9)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Gravada => gravada;

        /// <summary>
        /// IVA del detalle de la compra
        /// </summary>
        [XafDisplayName("Iva"), Index(10), PersistentAlias(nameof(iva))]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        public decimal Iva => iva;

        /// <summary>
        /// Total del detalle de la compra
        /// </summary>
        [PersistentAlias("[Exenta] + [Gravada] + [Iva]")]
        [ModelDefault("DisplayFormat", "{0:N2}"), XafDisplayName("Total"), Index(11)]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));

        /// <summary>
        /// Cantidad anulada
        /// </summary>
        [PersistentAlias(nameof(cantidadAnulada)), Index(12), XafDisplayName("Cantidad Anulada")]
        [ModelDefault("DisplayFormat", "{0:N2}"), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal? CantidadAnulada => cantidadAnulada;

        /// <summary>
        /// Marca del bien
        /// </summary>
        /// <remarks>
        ///  Solo es importante cuando se trata de compras de activo fijo e intangibles
        /// </remarks>
        [Size(50), DbType("varchar(50)"), XafDisplayName("Marca"), Index(19)]
        public string Marca
        {
            get => marca;
            set => SetPropertyValue(nameof(Marca), ref marca, value);
        }

        /// <summary>
        /// Modelo del bien
        /// </summary>
        /// <remarks>
        /// Solo es importante cuando se trata de compras de activo fijo e intangibles
        /// </remarks>
        [Size(50), DbType("varchar(50)"), XafDisplayName("Modelo"), Index(20)]
        public string Modelo
        {
            get => modelo;
            set => SetPropertyValue(nameof(Modelo), ref modelo, value);
        }

        /// <summary>
        /// Dias de garantia del bien
        /// </summary>
        /// <remarks>
        /// Solo es importante cuando se trata de compras de activo fijo e intangibles
        /// </remarks>
        [DbType("smallint"), XafDisplayName("Días Garantía"), Index(21)]
        public int? DiasGarantia
        {
            get => diasGarantia;
            set => SetPropertyValue(nameof(DiasGarantia), ref diasGarantia, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}