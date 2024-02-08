using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.CxP.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al detalle de un documento de cuenta por pagar, como el detalle de una Nota de Credito, detalle de Nota de Debito
    /// </summary>
    [DefaultClassOptions, NavigationItem(false), CreatableItem(false), DefaultProperty(nameof(FacturaDetalle))]
    [ModelDefault("Caption", "Detalle CxP"), Persistent("CxPTransaccionDetalle")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxPDocumentoDetalle : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxPDocumentoDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            precioUnidad = 0.0m;
            cantidad = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal? cantidadAnulada;
        protected decimal? noSujeta;
        protected decimal? gravada;
        protected decimal? iva;
        protected decimal? exenta;
        decimal precioUnidad;
        decimal cantidad;
        CompraFacturaDetalle facturaDetalle;
        CxPDocumento documento;

        [Association("CxPDocumento-Detalles")]
        [XafDisplayName("Documento")]
        public CxPDocumento Documento
        {
            get => documento;
            set
            {
                CxPDocumento oldDocumento = documento;
                bool changed = SetPropertyValue(nameof(Documento), ref documento, value);
                if (!IsLoading && !IsSaving && changed && oldDocumento != documento)
                {
                    oldDocumento = oldDocumento ?? documento;
                    oldDocumento.UpdateTotalExenta(true);
                    oldDocumento.UpdateTotalGravada(true);
                    oldDocumento.UpdateTotalIva(true);
                    oldDocumento.UpdateTotalNoSujeta(true);
                }
            }
        }

        //[Association("CompraFacturaDetalle-CxPDetalles")]
        [XafDisplayName("Factura Detalle")]
        public CompraFacturaDetalle FacturaDetalle
        {
            get => facturaDetalle;
            set => SetPropertyValue(nameof(FacturaDetalle), ref facturaDetalle, value);
        }

        /// <summary>
        /// Cantidad de producto
        /// </summary>
        [DbType("numeric(12,2)"), XafDisplayName("Cantidad"), VisibleInLookupListView(true)]
        [RuleRange("CxPDocumentoDetalle.Cantidad_Rango", DefaultContexts.Save, "0.0", "[VentaDetalle.Cantidad]",
            ParametersMode.Expression, SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Cantidad debe ser mayor o igual a 0.0 y menor o igual a la cantidad en el documento de compra")]
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
        [RuleRange("CxPDocumentoDetalle.PrecioUnidad_Rango", DefaultContexts.Save, "0.0", "[VentaDetalle.PrecioUnidad]",
            ParametersMode.Expression, SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Precio Unidad debe ser Mayor o Igual a 0.0m y menor o igual al precio en el documento de compra")]
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
        /// Cantidad anulada
        /// </summary>
        [PersistentAlias(nameof(cantidadAnulada))]
        [XafDisplayName("Cantidad Anulada"), ModelDefault("DisplayFormat", "{0:N2}"), VisibleInListView(false), VisibleInLookupListView(false)]
        [ModelDefault("AllowEdit", "False")]
        public decimal? CantidadAnulada
        {
            get => cantidadAnulada;
            set => SetPropertyValue(nameof(CantidadAnulada), ref cantidadAnulada, value);
        }

        #endregion

        #region Metodos
        private void DoCantidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            decimal oldPrecio = PrecioUnidad;
            DoPrecioUnidadChanged(true, oldPrecio);
        }

        private void DoPrecioUnidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            gravada = 0.0m;
            iva = 0.0m;
            exenta = 0.0m;
            noSujeta = 0.0m;
            if (Documento == null || FacturaDetalle.Producto == null)
                return;
            switch (FacturaDetalle.Producto.Categoria.ClasificacionIva)
            {
                case EClasificacionIVA.Gravado:
                    if (FacturaDetalle.Factura.TipoFactura.Codigo == "COVE01")
                    {
                        gravada = Math.Round(Cantidad * PrecioUnidad, 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * FacturaDetalle.Producto.Categoria.PorcentajeIVA, 2);
                    }
                    else
                    {
                        gravada = Math.Round(Cantidad * PrecioUnidad / (FacturaDetalle.Producto.Categoria.PorcentajeIVA + 1), 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * FacturaDetalle.Producto.Categoria.PorcentajeIVA, 2);
                    }
                    break;
                default:
                    exenta = Math.Round(Cantidad * PrecioUnidad, 2);
                    iva = 0.0m;
                    break;
            }
            /// Info https://supportcenter.devexpress.com/ticket/details/ka18699/how-to-implement-dependent-and-calculated-properties-in-xpo#
            OnChanged(nameof(Gravada));
            OnChanged(nameof(Iva));
            OnChanged(nameof(Exenta));
            //if (forceChangeEvents)
            //    OnChanged(nameof(PrecioUnidad), oldValue, PrecioUnidad);
            Documento.UpdateTotalExenta(true);
            Documento.UpdateTotalGravada(true);
            Documento.UpdateTotalIva(true);
            Documento.UpdateTotalNoSujeta(true);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}