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
        protected override void DoCantidadChanged(bool forceChangeEvents)
        {
            base.DoCantidadChanged(forceChangeEvents);
            unidades = Math.Round(Cantidad * this.Presentacion.Unidades, 2);
        }

        protected override void DoPrecioUnidadChanged(bool forceChangeEvents)
        {
            base.DoPrecioUnidadChanged(forceChangeEvents);
            switch (this.Producto.Clasificacion)
            {
                case EClasificacionIVA.Gravado:
                    if (Factura.TipoFactura.Codigo == "COVE01")
                    {
                        gravada = Math.Round(Unidades * PrecioUnidad, 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * this.Producto.PorcentajeIVA, 2);
                    }
                    else
                    {
                        gravada = Math.Round(Unidades * PrecioUnidad / (this.Producto.PorcentajeIVA + 1), 2);
                        iva = Math.Round(Convert.ToDecimal(Gravada) * this.Producto.PorcentajeIVA, 2);
                    }
                    break;
                default:
                    exenta = Math.Round(Unidades * PrecioUnidad, 2);
                    iva = 0.0m;
                    break;
            }
        }

        protected override void DoChangedExenta(bool forceChangeEvents)
        {
            base.DoChangedExenta(forceChangeEvents);
            if (Factura != null)
                Factura.UpdateTotalExenta(forceChangeEvents);

        }

        protected override void DoChangedGravada(bool forceChangedEvents)
        {
            base.DoChangedGravada(forceChangedEvents);
            if (Factura != null)
                Factura.UpdateTotalGravada(forceChangedEvents);
        }

        protected override void DoChangedIva(bool forceChangeEvents)
        {
            base.DoChangedIva(forceChangeEvents);
            if (Factura != null)
                Factura.UpdateTotalIva(forceChangeEvents);
        }

        protected override void DoChangedNoSujeta(bool forceChangeEvents)
        {
            base.DoChangedNoSujeta(forceChangeEvents);
            if (Factura != null)
                Factura.UpdateTotalNoSujeta(forceChangeEvents);
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