using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    [ModelDefault("Caption", "Compra Factura Detalle"), NavigationItem(false), CreatableItem(false),
        Persistent(nameof(CompraFacturaDetalle)), DefaultProperty("Producto")]
    //[ImageName("BO_Contact")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
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
                OnChanged(nameof(Unidades));
            }
        }

        protected override void DoPrecioUnidadChanged(bool forceChangeEvents, decimal oldValue)
        {
            base.DoPrecioUnidadChanged(forceChangeEvents, oldValue);
            if (Factura == null || Producto == null)
                return;
            if (Producto.Categoria.ClasificacionIva == EClasificacionIVA.Gravado)
            {
                //gravada = Math.Round(Cantidad * PrecioUnidad, 2);
                gravada = Factura.CalcularTributo(4);
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
            factura.UpdateTotalExenta(true);
            factura.UpdateTotalGravada(true);
            factura.UpdateTotalIva(true);
            factura.UpdateTotalNoSujeta(true);
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}