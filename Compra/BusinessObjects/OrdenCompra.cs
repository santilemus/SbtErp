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
using SBT.Apps.Tercero.Module.BusinessObjects;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al encabezado de las ordenes de compra
    /// </summary>
    /// <remarks>
    /// Faltan las siguientes relaciones: 
    /// 1. Cuenta o Rubro de presupuestos (el detalle, de alli se va al encabezado del presupuesto)
    /// 2. Solicitud de Compra
    /// 3. Comparativo de Ofertas (ver si sigue siendo valido en este modelo)
    /// 4. No de Oferta
    /// </remarks>
    
    [DefaultClassOptions, ModelDefault("Caption", "Orden Compra"), NavigationItem("Compras"), CreatableItem(false), 
        DefaultProperty(nameof(Proveedor)), Persistent("OrdenCompra")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class OrdenCompra : XPOBaseDoc
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public OrdenCompra(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Empleado.Module.BusinessObjects.Empleado aprobo;
        EEstadoOrdenCompra estado;
        string concepto;
        Listas tipoFactura;
        ETipoCompra tipo = ETipoCompra.Servicio;
        int? diasCredito;
        Listas condicionPago;
        Tercero.Module.BusinessObjects.Tercero proveedor;
        [DbType("numeric(14,2)"), Persistent(nameof(CompraGravada))]
        decimal? compraGravada;
        [DbType("numeric(14,2)"), Persistent(nameof(CompraExenta))]
        decimal? compraExenta;
        [DbType("numeric(14,2)"), Persistent(nameof(Iva))]
        decimal? iva;
        [DbType("numeric(14,2)"), Persistent(nameof(IvaRetenido))]
        decimal? ivaRetenido;
        [DbType("numeric(14,2)"), Persistent(nameof(IvaPercibido))]
        decimal? ivaPercibido;

        /// <summary>
        /// Proveedor al cual se gira la orden de compra
        /// </summary>
        [XafDisplayName("Proveedor"), RuleRequiredField("OrdenCompra.Proveedor_Requerido", DefaultContexts.Save), Index(5)]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        /// <summary>
        /// Tipo de Compra. Puede ser: Servicio, Producto, ActivoFijo
        /// </summary>
        [DbType("smallint"), XafDisplayName("Tipo Compra"), RuleRequiredField("OrdenCompra.Tipo_Requerido", "Save"), Index(6)]
        public ETipoCompra Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        /// <summary>
        /// Tipo de Factura que el proveedor debe emitir para la orden de compra
        /// </summary>
        [XafDisplayName("Tipo Factura"), RuleRequiredField("OrdenCompra.TipoFactura_Requerido", DefaultContexts.Save)]
        [DataSourceCriteria("[Categoria] == 15 And [Activo] == True"), VisibleInLookupListView(true), Index(7)]
        public Listas TipoFactura
        {
            get => tipoFactura;
            set => SetPropertyValue(nameof(TipoFactura), ref tipoFactura, value);
        }

        /// <summary>
        /// Condicion de Pago. Es obligatorio cuando se trata de Credito Fiscal o Factura Consumidor Final
        /// </summary>
        [XafDisplayName("Condición Pago"), RuleRequiredField("OrdenCompra.CondicionPago_Requerido", "Save",
            TargetCriteria = "@This.Tipo.Codigo In ('COVE01', 'COVE02')")]
        [DataSourceCriteria("[Categoria] == 17 And [Activo] == True")]   // Categoria = 17 es condicion de pago
        [VisibleInListView(false), Index(8)]
        public Listas CondicionPago
        {
            get => condicionPago;
            set => SetPropertyValue(nameof(CondicionPago), ref condicionPago, value);
        }

        /// <summary>
        /// Dias de crédito, cuando la condicion de pago es al crédito
        /// </summary>
        [DbType("smallint"), XafDisplayName("Días Crédito"), Index(9), VisibleInLookupListView(false)]
        public int? DiasCredito
        {
            get => diasCredito;
            set => SetPropertyValue(nameof(DiasCredito), ref diasCredito, value);
        }

        /// <summary>
        /// Concepto de la compra
        /// </summary>
        [Size(200), DbType("varchar(200)"), XafDisplayName("Concepto"), Index(10)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        /// <summary>
        /// Estado de la orden de compra
        /// </summary>
        [DbType("smallint"), XafDisplayName("Estado"), Index(11)]
        public EEstadoOrdenCompra Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        /// <summary>
        /// Empleado que aprobo la orden de compra
        /// </summary>
        [XafDisplayName("Aprobada Por"), VisibleInListView(false), VisibleInLookupListView(false), Index(12)]
        [ExplicitLoading]
        public Empleado.Module.BusinessObjects.Empleado Aprobo
        {
            get => aprobo;
            set => SetPropertyValue(nameof(Aprobo), ref aprobo, value);
        }

        /// <summary>
        /// Valor gravado de la orden de compra
        /// </summary>
        [PersistentAlias(nameof(compraGravada)), XafDisplayName("Gravado"), Index(13)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? CompraGravada
        {
            get
            {
                if (!IsLoading && !IsSaving && compraGravada == null)
                    UpdateTotalGravado(false);
                return compraGravada;
            }
        }

        /// <summary>
        /// Monto del IVA de la orden de compra
        /// </summary>
        [PersistentAlias(nameof(iva)), XafDisplayName("IVA"), Index(14)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? Iva
        {
            get
            {
                if (!IsLoading && !IsSaving && iva == null)
                    UpdateTotalIva(false);
                return iva;
            }
        }

        /// <summary>
        /// Subtotal Gravado + IVA de la orden de compra
        /// </summary>
        [PersistentAlias("[CompraGravada] + [Iva]")]
        [XafDisplayName("SubTotal"), Index(15)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SubTotal
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(SubTotal))); }
        }

        /// <summary>
        ///  IVA Percibido de la orden de compra
        /// </summary>
        [PersistentAlias(nameof(ivaPercibido)), XafDisplayName("Iva Percibido"), VisibleInListView(false), Index(16)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? IvaPercibido
        {
            get { return ivaPercibido; }
        }

        /// <summary>
        /// IVA Retenido de la orden de compra
        /// </summary>
        [PersistentAlias(nameof(ivaRetenido)), XafDisplayName("Iva Retenido"), VisibleInListView(false), Index(17)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? IvaRetenido
        {
            get { return ivaRetenido; }
        }

        /// <summary>
        /// Monto exento de la orden de compra
        /// </summary>
        [PersistentAlias(nameof(compraExenta)), XafDisplayName("Exento"), VisibleInListView(true), Index(18)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? CompraExenta
        {
            get
            {
                if (!IsLoading && !IsSaving && compraExenta == null)
                    UpdateTotalExento(false);
                return compraExenta;
            }
        }

        /// <summary>
        ///  Total de la orden de compra
        /// </summary>
        [PersistentAlias("[SubTotal] + [IvaPercibido] - [IvaRetenido] + [CompraExenta] ")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [XafDisplayName("Total"), Index(18)]
        public decimal Total
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(Total))); }
        }

        #endregion

        #region colecciones
        [Association("OrdenCompra-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle Orden Compra"), Index(0)]
        public XPCollection<OrdenCompraDetalle> Detalles
        {
            get
            {
                return GetCollection<OrdenCompraDetalle>(nameof(Detalles));
            }
        }

        [Association("OrdenCompra-Facturas"), Index(1)]
        public XPCollection<FacturaCompra> Facturas
        {
            get
            {
                return GetCollection<FacturaCompra>(nameof(Facturas));
            }
        }
        #endregion

        #region Metodos
        public void UpdateTotalExento(bool forceChangeEvents)
        {
            decimal? oldCompraExenta = compraExenta;
            decimal tempCompraExenta = 0.0m;
            foreach (OrdenCompraDetalle detalle in Detalles)
                tempCompraExenta += detalle.Exenta;
            compraExenta = tempCompraExenta;
            if (forceChangeEvents)
                OnChanged(nameof(CompraExenta), oldCompraExenta, compraExenta);
        }

        public void UpdateTotalGravado(bool forceChangeEvents)
        {
            decimal? oldCompraGravada = compraGravada;
            decimal tempCompraGravada = 0.0m;
            foreach (OrdenCompraDetalle detalle in Detalles)
                tempCompraGravada += detalle.Gravada;
            compraGravada = tempCompraGravada;
            if (forceChangeEvents)
                OnChanged(nameof(CompraGravada), oldCompraGravada, compraGravada);
        }

        public void UpdateTotalIva(bool forceChangeEvents)
        {
            decimal? oldIva = iva;
            decimal tempIva = 0.0m;
            foreach (OrdenCompraDetalle detalle in Detalles)
                tempIva += detalle.Iva;
            iva = tempIva;
            if (forceChangeEvents)
                OnChanged(nameof(Iva), oldIva, iva);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}