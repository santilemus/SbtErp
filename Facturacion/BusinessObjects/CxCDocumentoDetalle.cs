using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using System;
using System.ComponentModel;


namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al detalle de la cuenta por cobrar cuando existe y esta relacionado con el detalle de la venta.
    /// Ejemplos: detalle de la nota de credito, detalle nota de debito
    /// </summary>

    [ModelDefault("Caption", "CxC Detalle"), NavigationItem(false), CreatableItem(false),
        DefaultProperty(nameof(VentaDetalle))]
    //[ImageName("BO_Contact")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    [Persistent(nameof(CxCDocumentoDetalle))]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxCDocumentoDetalle : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxCDocumentoDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            cantidadAnulada = null;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(CantidadAnulada))]
        decimal? cantidadAnulada;
        protected decimal? noSujeta;
        protected decimal? gravada;
        protected decimal? iva;
        protected decimal? exenta;
        decimal precioUnidad;
        decimal cantidad;
        VentaDetalle ventaDetalle;
        CxC.Module.BusinessObjects.CxCDocumento cxCDocumento;

        [Association("CxCDocumento-Detalles"), XafDisplayName("CxC Documento"), Index(0)]
        public CxC.Module.BusinessObjects.CxCDocumento CxCDocumento
        {
            get => cxCDocumento;
            set => SetPropertyValue(nameof(CxCDocumento), ref cxCDocumento, value);
        }


        [Association("VentaDetalle-CxCDocumentoDetalles"), XafDisplayName("Venta Detalle"), Index(1)]
        public VentaDetalle VentaDetalle
        {
            get => ventaDetalle;
            set => SetPropertyValue(nameof(VentaDetalle), ref ventaDetalle, value);
        }

        /// <summary>
        /// Cantidad de producto
        /// </summary>
        [DbType("numeric(12,2)"), XafDisplayName("Cantidad"), VisibleInLookupListView(true)]
        [RuleRange("CxCDocumentoDetalle.Cantidad_Rango", DefaultContexts.Save, "0.0", "[VentaDetalle.Cantidad]",
            ParametersMode.Expression, SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Cantidad debe ser mayor o igual a 0.0 y menor o igual a la cantidad en el documento de venta")]
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
        [RuleRange("CxCDocumentoDetalle.PrecioUnidad_Rango", DefaultContexts.Save, "0.0", "[VentaDetalle.PrecioUnidad]",
            ParametersMode.Expression, SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Precio Unidad debe ser Mayor o Igual a 0.0m y menor o igual al precio en el documento de venta")]
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
            if (CxCDocumento == null || VentaDetalle.Producto == null)
                return;
            switch (VentaDetalle.Producto.Categoria.ClasificacionIva)
            {
                case EClasificacionIVA.Gravado:
                    if (VentaDetalle.Venta.TipoFactura.Codigo == "COVE01")
                    {
                        gravada = Math.Round(Cantidad * PrecioUnidad, 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * VentaDetalle.Producto.Categoria.PorcentajeIVA, 2);
                    }
                    else
                    {
                        gravada = Math.Round(Cantidad * PrecioUnidad / (VentaDetalle.Producto.Categoria.PorcentajeIVA + 1), 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * VentaDetalle.Producto.Categoria.PorcentajeIVA, 2);
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
            CxCDocumento.UpdateTotalExenta(true);
            CxCDocumento.UpdateTotalGravada(true);
            CxCDocumento.UpdateTotalIva(true);
            CxCDocumento.UpdateTotalNoSujeta(true);
        }

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