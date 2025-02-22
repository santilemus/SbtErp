﻿using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using SBT.Apps.CxP.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Compra.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a los encabezados de las facturas de compra
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <cambios>
    /// 13/agosto/2024 por SELM
    /// Se agrega propiedad Dte para guardar el Json correspondiente cuando es un documento electrónico el que se recibe
    /// 29/febrero/2024 por SELM
    /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA y pago a cuenta de febrero del 2024.
    /// </cambios>
    [DefaultClassOptions, ModelDefault("Caption", "Factura de Compra"), NavigationItem("Compras"), CreatableItem(false)]
    [DefaultProperty(nameof(NumeroFactura))]
    [Persistent(nameof(CompraFactura))]
    [Appearance("CompraFactura_Servicios_Intangibles", AppearanceItemType = "ViewItem", Criteria = "[Tipo] In ('Servicio', 'Intangible')",
        Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, TargetItems = "Detalles;Ingresos", Context = "DetailView")]
    [Appearance("CompraFactura_SujetoExcluido", AppearanceItemType = "ViewItem", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, 
        Context = "DetailView", TargetItems = "IvaPercibido;IvaRetenido;Fovial;Exenta;NoSujeta;Numero", Criteria = "[TipoFactura.Codigo] == 'COVE06'")]
    [ImageName(nameof(CompraFactura))]
    [FriendlyKeyProperty(nameof(NumeroFactura))]
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
            fovial = 0.0m;
            NoSujeta = 0.0m;
            DiasCredito = 0;
            Clase = EClaseDocumento.Imprenta;
            Tipo = ETipoCompra.Servicio;
            tipoOperacion = ETipoOperacionCompra.Gravada;
            tipoCostoGasto = ETipoCostoGasto.GastoVenta;
            clasificacionRenta = EClasificacionRenta.Costo;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal fovial;
        EClaseDocumento clase;
        decimal renta;
        string concepto;
        string numeroFactura;
        ETipoCompra tipo = ETipoCompra.Servicio;
        Tercero.Module.BusinessObjects.Tercero proveedor;
        EOrigenCompra origen = EOrigenCompra.Local;
        OrdenCompra ordenCompra;
        private string serie;
        private ETipoOperacionCompra tipoOperacion;
        private EClasificacionRenta clasificacionRenta;
        private ETipoCostoGasto tipoCostoGasto;
        private TerceroGiro proveedorGiro;
        private string dte;
        private int partida;

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
            set
            {
                bool changed = SetPropertyValue(nameof(Proveedor), ref proveedor, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    ProveedorGiro = Proveedor.Giros.FirstOrDefault<TerceroGiro>(x => x.Tercero.Oid == value.Oid);
                    if (Proveedor.TipoContribuyente == ETipoContribuyente.Excluido)
                    {
                        var tipoFactura = Session.GetObjectByKey<Listas>("COVE06");
                        if (tipoFactura !=  null)
                            TipoFactura = tipoFactura;
                        TipoOperacion = ETipoOperacionCompra.Excluido;
                    }
                }
            }
        }

        [System.ComponentModel.DisplayName("Giro"), RuleRequiredField("CompraFactura.ProveedorGiro", DefaultContexts.Save)]
        [DataSourceCriteria("[Tercero] == '@This.Proveedor'")]
        public TerceroGiro ProveedorGiro
        {
            get => proveedorGiro;
            set => SetPropertyValue(nameof(ProveedorGiro), ref proveedorGiro, value);
        }

        /// <summary>
        /// Tipo de Compra. Puede ser: Servicio, Producto, ActivoFijo
        /// </summary>
        [DbType("smallint"), XafDisplayName("Tipo Compra"), Index(7)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public ETipoCompra Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("smallint"), XafDisplayName("Origen"), Index(8)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public EOrigenCompra Origen
        {
            get => origen;
            set => SetPropertyValue(nameof(Origen), ref origen, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("No Factura"), Index(10)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string NumeroFactura
        {
            get => numeroFactura;
            set => SetPropertyValue(nameof(NumeroFactura), ref numeroFactura, value);
        }

        [Size(100), DbType("varchar(100)"), VisibleInListView(false), XafDisplayName("Serie"), Index(11)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string Serie
        {
            get => serie;
            set => SetPropertyValue(nameof(Serie), ref serie, value);
        }

        [Size(20), XafDisplayName("Clase"), Index(12)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        [ToolTip("Clase de documento. Es requerido para el libro de compras")]
        public EClaseDocumento Clase
        {
            get => clase;
            set => SetPropertyValue(nameof(Clase), ref clase, value);
        }

        /// <summary>
        /// Se refiere a la naturaleza de la compra: costo o gasto para efectos de la ley de impuesto sobre la renta
        /// </summary>
        /// <remarks>
        /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
        /// y pago a cuenta de febrero del 2024.
        /// </remarks>
        [DbType("smallint"), VisibleInListView(false), System.ComponentModel.DisplayName("Tipo Operación"), Index(13)]
        [ToolTip(@"Naturaleza de la compra: costo o gasto para efectos de la ley de impuesto sobre la renta")]
        public ETipoOperacionCompra TipoOperacion
        {
            get => tipoOperacion;
            set => SetPropertyValue(nameof(TipoOperacion), ref tipoOperacion, value);
        }

        /// <summary>
        /// Tipo de erogación al cual corresponden las deducciones del impuesto sobre la renta, respecto de cada compra
        /// de bienes o servicios que se realice en cada período tributario
        /// </summary>
        /// <remarks>
        /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
        /// y pago a cuenta de febrero del 2024.
        /// </remarks>
        [DbType("smallint"), VisibleInListView(false), System.ComponentModel.DisplayName("Clasificación"), Index(14)]
        [ToolTip("Tipo de erogación al cual corresponden las deducciones del impuesto sobre la renta, respecto de cada compra")]
        public EClasificacionRenta ClasificacionRenta
        {
            get => clasificacionRenta;
            set => SetPropertyValue(nameof(ClasificacionRenta), ref clasificacionRenta, value);
        }

        /// <summary>
        /// Tipo de costo/gasto: según la clasificación que se detalla en el formulario F-11 del impuesto sobre la renta
        /// </summary>
        /// <remarks>
        /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA
        /// y pago a cuenta de febrero del 2024.
        /// </remarks>
        [DbType("smallint"), VisibleInListView(false), System.ComponentModel.DisplayName("Tipo Costo o Gasto"), Index(15)]
        [ToolTip("Tipo de costo o gasto según la clasificación que se detalla en el formulario F-11 del impuesto sobre la renta")]
        public ETipoCostoGasto TipoCostoGasto
        {
            get => tipoCostoGasto;
            set => SetPropertyValue(nameof(TipoCostoGasto), ref tipoCostoGasto, value);
        }

        /// <summary>
        /// Concepto de la compra
        /// </summary>
        [Size(200), DbType("varchar(200)"), XafDisplayName("Concepto"), Index(16)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Renta Retenida")]
        [ToolTip("Monto de la retención de renta cuando es compra de servicios a personas naturales, compra de intangibles o de sujetos no domiciliados")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), Index(17)]
        public decimal Renta
        {
            get => renta;
            set => SetPropertyValue(nameof(Renta), ref renta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Fovial")]
        [ToolTip("Monto de la contribución al fovial cuando se trata de compras de combustible")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), Index(18)]
        public decimal Fovial
        {
            get => fovial;
            set => SetPropertyValue(nameof(Fovial), ref fovial, value);
        }

        /// <summary>
        /// Total del documento
        /// </summary>
        /// <remarks>
        /// El artículo del siguiente link dice que es mejor calcular la propiedad directamente porque es preferible en
        /// lugar de persistentalias para calculos pesados (o continuos quiz). Por eso se hizo el cambio, sino produce el
        /// efecto esperado regresar a la expresion del PersistentAlias. Hay que ver que funcione bien en app web
        /// https://github.com/DevExpress/XPO/blob/master/Tutorials/WinForms/Classic/create-persistent-classes-and-connect-xpo-to-database.md
        /// </remarks>
        [PersistentAlias("[SubTotal] + [IvaPercibido] - [IvaRetenido] + [NoSujeta] + [Exenta] + [Fovial]")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [XafDisplayName("Total"), Index(28)]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        [VisibleInListView(true)]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));

        [ModelDefault("AllowEdit", "False"), VisibleInListView(false), VisibleInDetailView(false)]
        public int Partida
        {
            get => partida;
            set => SetPropertyValue(nameof(Partida), ref partida, value);
        }

        /// <summary>
        /// Json del dte se guarda en el documento de compra
        /// </summary>
        [Browsable(false)]
        [DbType("nvarchar(max)")]
        [Delayed(true)]
        public string Dte
        {
            get => GetDelayedPropertyValue<string>(nameof(Dte));
            set => SetDelayedPropertyValue<string>(nameof(Dte), value);
        }

        #endregion

        #region Colecciones
        [Association("CompraFactura-Detalles"), DevExpress.Xpo.Aggregated, Index(0), ModelDefault("Caption", "Detalles")]
        public XPCollection<CompraFacturaDetalle> Detalles => GetCollection<CompraFacturaDetalle>(nameof(Detalles));

        [Association("CompraFactura-Ingresos"), Index(1), ModelDefault("Caption", "Ingresos")]
        public XPCollection<InventarioMovimiento> Ingresos => GetCollection<InventarioMovimiento>(nameof(Ingresos));

        [Association("CompraFactura-CxPTransacciones"), Index(2), XafDisplayName("Cuenta por Pagar"), DevExpress.Xpo.Aggregated]
        public XPCollection<CxPTransaccion> CxPTransacciones => GetCollection<CxPTransaccion>(nameof(CxPTransacciones));
        #endregion

        #region Metodos

        protected override void DoGravadaChanged(bool forceChangeEvents, decimal? oldValue)
        {
            if (this.Detalles.Count == 0)
            {
                // calculamos el Iva, cuando no hay detalles porque en ese caso solo se esta ingresando el encabezado de la compra
                iva = CalcularTributo(5);

                OnChanged(nameof(Iva));
            }
            base.DoGravadaChanged(forceChangeEvents, oldValue);

            ivaRetenido = CalcularTributo(6);
            IvaPercibido = CalcularTributo(7);
            renta = CalcularTributo(8);
            OnChanged(nameof(IvaRetenido));
            OnChanged(nameof(IvaPercibido));
            OnChanged(nameof(Renta));
        }

        public decimal CalcularTributo(int oidTributo)
        {
            Tributo tributo = Session.GetObjectByKey<Tributo>(oidTributo);
            if (tributo != null)
            {
                // la formula debe tener las reglas o condiciones para calcular el tributo cuando aplique
                ExpressionEvaluator eval = new(TypeDescriptor.GetProperties(tributo.TipoBO), tributo.Formula);

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
            if (Saldo != 0.0m)
            {
                base.ActualizarSaldo(valor, status, forceChangeEvents);
            }
        }

        public override void Anular(AnularParametros AnularParams)
        {
            base.Anular(AnularParams);
            Gravada = 0.0m;
            Iva = 0.0m;
            Exenta = 0.0m;
            NoSujeta = 0.0m;
            Renta = 0.0m;
            saldo = 0.0m;
            DoAnular();
        }

        protected override void RefreshTiposDeFacturas()
        {
            base.RefreshTiposDeFacturas();
            if (fTiposDeFacturas == null)
                return;
            fTiposDeFacturas.Criteria = CriteriaOperator.Parse("[Categoria] == 15 && [Activo] == True && [Codigo] In ('COVE01', 'COVE02', 'COVE04', 'COVE05', 'COVE06', 'COVE10', 'COVE12', 'COVE13')");
        }

        protected override decimal GetTotal()
        {
            return Total;
        }

        protected override void OnSaving()
        {
            if ((Session is not NestedUnitOfWork) && (Session.DataLayer != null) && Session.IsNewObject(this) &&
                (Session.ObjectLayer is SecuredSessionObjectLayer) && (Numero == null || Numero <= 0))
            {
                Numero = CorrelativoDoc();
            }
            base.OnSaving();
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (Oid <= 0)
                Session.Reload(this);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}