﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Xpo.Metadata;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// BO No persitente que sirve de ancestro para los documentos de compras y ventas
    /// </summary>
    [NonPersistent]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class XPCustomFacturaDetalle : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public XPCustomFacturaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            cantidad = 0.0m;
            cantidadAnulada = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid = -1;
        Producto producto;
        [Persistent(nameof(CantidadAnulada))]
        decimal? cantidadAnulada;
        protected decimal? noSujeta;
        protected decimal? gravada;      
        protected decimal? iva;
        protected decimal? exenta;
        decimal precioUnidad;
        decimal cantidad;


        
        [PersistentAlias(nameof(oid)), XafDisplayName("Oid"), Index(0)]
        public long Oid => oid;

        [XafDisplayName("Producto"), RuleRequiredField("", DefaultContexts.Save, CustomMessageTemplate = "Producto es requerido")]
        //[ImmediatePostData(true)]
        public Producto Producto
        {
            get => producto;
            set
            {
                Producto oldProducto = producto;
                bool changed = SetPropertyValue(nameof(Producto), ref producto, value);
                if (!IsLoading && !IsSaving && changed)
                    DoProductoChanged(true, oldProducto);
            }
        }

        /// <summary>
        /// Cantidad de producto
        /// </summary>
        [DbType("numeric(12,2)"), XafDisplayName("Cantidad"), VisibleInLookupListView(true)]
        [RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0.0, SkipNullOrEmptyValues = false,
                                 CustomMessageTemplate = "Cantidad debe ser mayor o igual a 0.0")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        //[ImmediatePostData(true)]
        public decimal Cantidad
        {
            get => cantidad;
            set
            {
                decimal oldCantidad = cantidad;
                bool changed = SetPropertyValue(nameof(Cantidad), ref cantidad, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    DoCantidadChanged(true, oldCantidad);
                }
            }
        }

        /// <summary>
        /// Precio por unidad
        /// </summary>
        [DbType("numeric(14,4)"), XafDisplayName("Precio Unidad")]
        [RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0.0, SkipNullOrEmptyValues = false,
                                 CustomMessageTemplate = "Precio Unidad debe ser Mayor o Igual a 0.0m")]
        [ModelDefault("DisplayFormat", "{0:N4}"), ModelDefault("EditMask", "n4")]
        //[ImmediatePostData(true)]
        public decimal PrecioUnidad
        {
            get => precioUnidad;
            set
            {
                decimal oldPrecioUnidad = precioUnidad;
                bool changed = SetPropertyValue(nameof(PrecioUnidad), ref precioUnidad, value);
                if (!IsLoading && !IsSaving && changed)
                    DoPrecioUnidadChanged(true, oldPrecioUnidad);
            }
        }

        /// <summary>
        /// Valor exento
        /// </summary>
        [Persistent(nameof(Exenta)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), XafDisplayName("Exenta")]
        public decimal? Exenta
        { 
            get => exenta;
            set => SetPropertyValue(nameof(Exenta), ref exenta, value);
        }

        /// <summary>
        /// Valor del Iva
        /// </summary>
        [Persistent(nameof(Iva)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), XafDisplayName("Iva")]
        public decimal? Iva
        {
            get => iva;
            set => SetPropertyValue(nameof(Iva), ref iva, value);
        }

        [Persistent(nameof(Gravada)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), XafDisplayName("Gravada")]
        public decimal? Gravada
        {
            get => gravada;
            set => SetPropertyValue(nameof(Gravada), ref gravada, value);
        }

        /// <summary>
        /// Valor no sujeto a impuestos
        /// </summary>
        [Persistent(nameof(NoSujeta)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), XafDisplayName("No Sujeta")]
        public decimal? NoSujeta
        {
            get => noSujeta;
            set => SetPropertyValue(nameof(NoSujeta), ref noSujeta, value);
        }

        /// <summary>
        /// Total
        /// </summary>
        [PersistentAlias("[Exenta] + [Iva] + [Gravada] + [NoSujeta]"), Browsable(false), XafDisplayName("Total")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));

        /// <summary>
        /// Cantidad anulada
        /// </summary>
        [PersistentAlias(nameof(cantidadAnulada))]
        [XafDisplayName("Cantidad Anulada"), ModelDefault("DisplayFormat", "{0:N2}"), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal? CantidadAnulada => cantidadAnulada;
        #endregion

        #region Metodos

        /// <summary>
        /// Metodo que se ejecutan cuando cambia el producto. Reescribirlo en las clases heredadas para implementar la 
        /// funcionalidad requerida en cada caso
        /// </summary>
        /// <param name="forceChangeEvents"></param>
        /// <param name="oldValue">El valor de la propiedad Producto antes del setter</param>
        protected virtual void DoProductoChanged(bool forceChangeEvents, Producto oldValue)
        {

        }

        /// <summary>
        /// Metodo que se ejecuta cuando cambia la cantidad. Reescribirlo en las clases heredadas para implementar la
        /// funcionalidad requerida en cada caso
        /// </summary>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        /// <param name="oldValue">El valor de la propiedad Cantidad antes del setter</param>
        protected virtual void DoCantidadChanged(bool forceChangeEvents, decimal oldValue)
        {

        }

        /// <summary>
        /// Metodo que se ejecuta cambia el precio. Reescribirlo en las clases heredadas para implementar la funcionalidad
        /// requerida en cada caso
        /// </summary>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        /// <param name="oldValue">El valor de la propiedad PrecioUnidad antes del setter</param>
        /// <param name="fUnidades">La cantidad de unidades del producto en el documento</param>
        /// <param name="codigoTipoFactura">El codigo del tipo de factura al cual corresponde el documento</param>
        protected virtual void DoPrecioUnidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            InicializarSubtotales();
        }

        protected void OnPrecioUnidadChanged(decimal fUnidades, string fTipoFactura)
        {
            if (Producto == null)
                return;
            switch (Producto.Categoria.ClasificacionIva)
            {
                case EClasificacionIVA.Gravado:
                    if (fTipoFactura == "COVE01")
                    {
                        gravada = Math.Round(fUnidades * PrecioUnidad, 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * this.Producto.Categoria.PorcentajeIVA, 2);
                    }
                    else
                    {
                        gravada = Math.Round(fUnidades * PrecioUnidad / (this.Producto.Categoria.PorcentajeIVA + 1), 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * this.Producto.Categoria.PorcentajeIVA, 2);
                    }
                    break;
                default:
                    exenta = Math.Round(fUnidades * PrecioUnidad, 2);
                    iva = 0.0m;
                    break;
            }
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
        /// </remarks>
        protected virtual void ActualizarInventario(int OidBodega, long OidDocumento, int OidProducto, decimal ACantidad)
        {

        }

        protected void InicializarSubtotales()
        {
            gravada = 0.0m;
            iva = 0.0m;
            exenta = 0.0m;
            noSujeta = 0.0m;
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}