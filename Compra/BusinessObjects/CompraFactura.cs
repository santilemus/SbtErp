using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a los encabezados de las facturas de compra
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Compra Factura"), NavigationItem("Compras"), CreatableItem(false)]
    [DefaultProperty(nameof(NumeroDocumento))]
    [Persistent(nameof(CompraFactura))]
    [Appearance("CompraFactura_Servicios_Intangibles", AppearanceItemType = "Any", Criteria = "[Tipo] In (0, 2)",
        Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "Detalles;Ingresos")]
    [ImageName(nameof(CompraFactura))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CompraFactura : XPCustomFacturaBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CompraFactura(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            renta = 0.0m;

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal renta;
        string concepto;
        string numeroDocumento;
        ETipoCompra tipo = ETipoCompra.Servicio;
        Tercero.Module.BusinessObjects.Tercero proveedor;
        EOrigenCompra origen = EOrigenCompra.Local;
        OrdenCompra ordenCompra;

        [Association("OrdenCompra-Facturas"), XafDisplayName("Orden Compra"), Persistent(nameof(OrdenCompra)), Index(5)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public OrdenCompra OrdenCompra
        {
            get => ordenCompra;
            set => SetPropertyValue(nameof(OrdenCompra), ref ordenCompra, value);
        }

        /// <summary>
        /// Proveedor al cual se gira la orden de compra
        /// </summary>
        [XafDisplayName("Proveedor"), RuleRequiredField("CompraFactura.Proveedor_Requerido", DefaultContexts.Save), Index(6)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        /// <summary>
        /// Tipo de Compra. Puede ser: Servicio, Producto, ActivoFijo
        /// </summary>
        [DbType("smallint"), XafDisplayName("Tipo Compra"), RuleRequiredField("CompraFactura.Tipo_Requerido", "Save"), Index(7)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public ETipoCompra Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("smallint"), XafDisplayName("Origen Compra"), Index(8)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public EOrigenCompra Origen
        {
            get => origen;
            set => SetPropertyValue(nameof(Origen), ref origen, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("No Documento"), Index(10)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string NumeroDocumento
        {
            get => numeroDocumento;
            set => SetPropertyValue(nameof(NumeroDocumento), ref numeroDocumento, value);
        }

        /// <summary>
        /// Concepto de la compra
        /// </summary>
        [Size(200), DbType("varchar(200)"), XafDisplayName("Concepto"), Index(10)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Renta Retenida")]
        [ToolTip("Monto de la retención de renta cuando es compra de servicios a personas naturales, compra de intangibles o de sujetos no domiciliados")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Renta
        {
            get => renta;
            set => SetPropertyValue(nameof(Renta), ref renta, value);
        }

        #endregion

        #region Colecciones
        [Association("CompraFactura-Detalles"), DevExpress.Xpo.Aggregated, Index(0), ModelDefault("Caption", "Detalles")]
        public XPCollection<CompraFacturaDetalle> Detalles => GetCollection<CompraFacturaDetalle>(nameof(Detalles));

        [Association("CompraFactura-Ingresos"), Index(1), ModelDefault("Caption", "Ingresos")]
        public XPCollection<InventarioMovimiento> Ingresos => GetCollection<InventarioMovimiento>(nameof(Ingresos));
        [Association("CompraFactura-CxPTransacciones"), Index(2), XafDisplayName("Transacciones CxP"), DevExpress.Xpo.Aggregated]
        public XPCollection<CxPTransaccion> CxPTransacciones => GetCollection<CxPTransaccion>(nameof(CxPTransacciones));
        #endregion

        #region Metodos

        protected override void DoGravadaChanged(bool forceChangeEvents, decimal? oldValue)
        {
            if (this.Detalles.Count == 0)
            {
                // calculamos el Iva, cuando no hay detalles porque en ese caso solo se esta ingresando el encabezado de la compra
                iva = CalcularTributo(3);
                OnChanged(nameof(Iva));
            }
            ivaRetenido = CalcularTributo(4);
            renta = CalcularTributo(5);
            OnChanged(nameof(IvaRetenido));
            OnChanged(nameof(Renta));
            base.DoGravadaChanged(forceChangeEvents, oldValue);
        }

        private decimal CalcularTributo(int oidTributo)
        {
            Tributo tributo = Session.GetObjectByKey<Tributo>(oidTributo);
            if (tributo != null)
            {
                // la formula debe tener las reglas o condiciones para calcular el tributo cuando aplique
                ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(tributo.TipoBO), tributo.Formula);
                return Convert.ToDecimal(eval.Evaluate(this));
            }
            else
                return 0.0m;
        }

        public override void UpdateTotalExenta(bool forceChangeEvents)
        {
            base.UpdateTotalExenta(forceChangeEvents);
            decimal? oldExenta = exenta;
            exenta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Exenta])")));
            if (forceChangeEvents)
                OnChanged(nameof(Exenta), oldExenta, exenta);
        }

        public override void UpdateTotalGravada(bool forceChangeEvents)
        {
            base.UpdateTotalGravada(forceChangeEvents);
            decimal? oldGravada = gravada;
            decimal tempGravada = 0.0m;
            foreach (CompraFacturaDetalle detalle in Detalles)
                tempGravada += Convert.ToDecimal(detalle.Gravada);
            gravada = tempGravada;
            if (forceChangeEvents)
                OnChanged(nameof(Gravada), oldGravada, gravada);
        }

        /// <summary>
        /// Actualiza el total Iva a partir de los detalles de la compra
        /// </summary>
        /// <param name="forceChangeEvents"></param>
        public override void UpdateTotalIva(bool forceChangeEvents)
        {
            base.UpdateTotalIva(forceChangeEvents);
            decimal? oldIva = iva;
            iva = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Iva])")));
            if (forceChangeEvents)
                OnChanged(nameof(Iva), oldIva, iva);
        }

        /// <summary>
        ///  calcular la renta cuando son compras de personas naturales
        /// </summary>
        /// <remarks>
        /// Revisar este metodo porque hay diferentes casos a evaluar: servicios personas naturales locales, domiciliados,
        /// no domiciliados, compra de intangibles, etc
        /// </remarks>
        public void UpdateRenta(bool forceChangeEvents)
        {
            if (Proveedor.TipoPersona == TipoPersona.Juridica &&
                (Proveedor.DireccionPrincipal == null || Proveedor.DireccionPrincipal.Pais.Codigo == "SLV"))
                return;
            // pendiente revisar con el caso de los intangibles, como se van a determinar
            if ((Tipo == ETipoCompra.Servicio) || 
                (Proveedor.DireccionPrincipal != null && Proveedor.DireccionPrincipal.Pais.Codigo != "SLV"))
            {
                // calcular aqui la renta, revisar las condiciones
            }
        }

        /// <summary>
        /// Cacular el saldo de la factura de compra, cuando se ingresan transacciones de cuentas por pagar
        /// REVISAR ES POSIBLE QUE SE DEBA QUITAR LA HERENCIA DE XPCustomFacturaBO, porque en este caso puede ingresarse
        /// una factura de compra sin detalles (solo el maestro para casos de servicios) y en esos casos se debe poder
        /// ingresar los datos de exento, gravado, etc.
        /// </summary>
        public override void ActualizarSaldo(decimal valor, EEstadoFactura status, bool forceChangeEvents)
        {
            if (CondicionPago == ECondicionPago.Credito)
            {
                // TipoOperacion == 1 son cargos que incrementan el saldo, 2 son abonos que disminuyen
                decimal? totSaldo = Total + Convert.ToDecimal(CriteriaOperator.Parse("[CxPTransacciones].Sum(Iif([Tipo.TipoOperacion] == 1, [Monto], Iif([Tipo.TipoOperacion] == 2, -[Monto]))"));
                base.ActualizarSaldo(valor, status, forceChangeEvents);
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