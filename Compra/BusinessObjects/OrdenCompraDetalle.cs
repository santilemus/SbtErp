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
    /// <remarks>
    /// Info en: https://docs.devexpress.com/eXpressAppFramework/113179/task-based-help/business-model-design/express-persistent-objects-xpo/how-to-calculate-a-property-value-based-on-values-from-a-detail-collection
    /// </remarks>

    [ModelDefault("Caption", "Orden Compra Detalle"), NavigationItem(false), CreatableItem(false),
        DefaultProperty(nameof(Producto)), Persistent(nameof(OrdenCompraDetalle))]
    //[ImageName("BO_Contact")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class OrdenCompraDetalle : XPCustomFacturaDetalle
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

        int? diasGarantia;
        string modelo;
        string marca;
        [DbType("numeric(12,2)"), Persistent(nameof(Unidades))]
        decimal unidades;
        Presentacion presentacion;
        OrdenCompra ordenCompra;


        /// <summary>
        /// La asociacion a la orden de compra
        /// </summary>
        [Association("OrdenCompra-Detalles"), XafDisplayName("Orden Compra"), Index(1)]
        public OrdenCompra OrdenCompra
        {
            get => ordenCompra;
            set
            {
                OrdenCompra oc = ordenCompra;
                bool changed = SetPropertyValue(nameof(OrdenCompra), ref ordenCompra, value);
                if (!IsLoading && !IsSaving && oc != ordenCompra & changed)
                {
                    oc = oc ?? ordenCompra;
                    oc.UpdateTotalExenta(true);
                    oc.UpdateTotalGravada(true);
                    oc.UpdateTotalIva(true);
                    oc.UpdateTotalNoSujeta(true);
                }
            }
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
        /// Total de unidades para el inventario
        /// </summary>
        [PersistentAlias(nameof(unidades)), XafDisplayName("Unidades"), Index(6)]
        public decimal Unidades => unidades;

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

        #region Metodos
        protected override void DoCantidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoCantidadChanged(forceChangeEvents, oldValue);
            unidades = Math.Round(Cantidad * this.Presentacion.Unidades, 2);
            OnChanged(nameof(Unidades));
        }

        protected override void DoPrecioUnidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoPrecioUnidadChanged(forceChangeEvents, oldValue);
            if (OrdenCompra == null)
                return;
            OnPrecioUnidadChanged(Unidades, OrdenCompra.TipoFactura.Codigo);
            if (forceChangeEvents)
                OnChanged(nameof(PrecioUnidad), oldValue, PrecioUnidad);
            OrdenCompra.UpdateTotalExenta(true);
            OrdenCompra.UpdateTotalGravada(true);
            OrdenCompra.UpdateTotalIva(true);
            OrdenCompra.UpdateTotalNoSujeta(true);
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}