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
    [DefaultClassOptions, ModelDefault("Caption", "Factura Compra Detalle"), NavigationItem(false), CreatableItem(false),
        Persistent(nameof(CompraFacturaDetalle)), DefaultProperty("Producto")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CompraFacturaDetalle : XPCustomFacturaDetalle
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CompraFacturaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [DbType("numeric(12,2)"), Persistent(nameof(Unidades))]
        decimal unidades;
        Presentacion presentacion;
        CompraFactura factura;

        [Association("CompraFactura-Detalles"), XafDisplayName("Factura")]
        public CompraFactura Factura
        {
            get => factura;
            set => SetPropertyValue(nameof(Factura), ref factura, value);
        }

        /// <summary>
        /// Presentacion o empaque. Importante porque cada presentacion contiene diferente cantidad de unidades
        /// </summary>
        [XafDisplayName("Presentación")]
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

        #endregion

        #region Metodos
        protected override void DoCantidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoCantidadChanged(forceChangeEvents, oldValue);
            decimal oldPrecioUnidad = PrecioUnidad;
            unidades = Math.Round(Cantidad * this.Presentacion.Unidades, 2);
            if (PrecioUnidad > 0.0m && forceChangeEvents)
            {
                OnChanged(nameof(Cantidad), oldValue, Cantidad);
                DoPrecioUnidadChanged(true, oldPrecioUnidad);
            }
        }

        protected override void DoPrecioUnidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoPrecioUnidadChanged(forceChangeEvents, oldValue);
            if (Factura == null)
                return;
            OnPrecioUnidadChanged(Unidades, Factura.TipoFactura.Codigo);
            if (forceChangeEvents)
                OnChanged(nameof(PrecioUnidad), oldValue, PrecioUnidad);
            if (Factura != null)
            {
                factura.UpdateTotalExenta(true);
                factura.UpdateTotalGravada(true);
                factura.UpdateTotalIva(true);
                factura.UpdateTotalNoSujeta(true);
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