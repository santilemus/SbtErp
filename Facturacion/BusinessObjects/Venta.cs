﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Inventario.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    /// <summary>
    /// Documento de Venta
    /// BO que corresponde al encabezado de los diferentes documentos de venta.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [DefaultClassOptions, ModelDefault("Caption", "Documento de Venta"), NavigationItem("Facturación"), DefaultProperty(nameof(NoFactura))]
    [Persistent("Venta")]
    [Appearance("Venta.CreditoFiscal", Criteria = "[TipoFactura.Categoria] == 15 && [TipoFactura.Codigo] != 'COVE01'",
        AppearanceItemType = "ViewItem",
        Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "DetailView", 
        TargetItems = "Caja;NRC;NotaRemision;Iva;IvaPercibido;IvaRetenido;ResumenTributos")]
    // la siguiente regla de apariencia es para deshabilitar la modificacion de propiedades criticas. Solo es posible cuando es un objeto nuevo
    [Appearance("Venta - Nuevo Registro", AppearanceItemType = "Any", Enabled = true, Context = "DetailView",
        TargetItems = "Bodega;Agencia;Caja;TipoFactura;NoFactura;Cliente", Criteria = "IsNewObject(This)")]

    [Appearance("Venta - Condicion Pago Contado", Criteria = "[CondicionPago] == 0",
        Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, AppearanceItemType = "LayoutItem", Context = "DetailView",
        TargetItems = "DiasCredito;Saldo;CxCDocumentos")]
    [Appearance("Venta - Condicion Pago Credito", Criteria = "[CondicionPago] == 1",
        Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, AppearanceItemType = "LayoutItem", Context = "DetailView",
        TargetItems = "FormaPago;TipoTarjeta;NoTarjeta;Banco;NoReferenciaPago")]

    [Appearance("Venta - Pago en Efectivo", Criteria = "[FormaPago] == 0",
        Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, AppearanceItemType = "LayoutItem", Context = "DetailView",
        TargetItems = "TipoTarjeta;NoTarjeta;Banco;NoReferenciaPago")]
    [Appearance("Venta - Pago con Cheque o Transferencia", Criteria = "[FormaPago] In (1, 3)",
        Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide,
        AppearanceItemType = "LayoutItem", Context = "DetailView", TargetItems = "TipoTarjeta;NoTarjeta")]

    [ImageName("factura")]
    [RuleObjectExists("", CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, LooksFor = typeof(Empresa))]
    [OptimisticLockingReadBehavior(OptimisticLockingReadBehavior.Default, true)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Venta : XPCustomFacturaBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Venta(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (((Usuario)SecuritySystem.CurrentUser).Agencia != null)
                agencia = ((Usuario)SecuritySystem.CurrentUser).Agencia;
            giro = null;
            // PENDIENTE asignar la caja de forma automatica, a partir de la agencia

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        bool gravadaTasaCero;
        SBT.Apps.Tercero.Module.BusinessObjects.TerceroGiro giro;
        [Persistent(nameof(Agencia))]
        EmpresaUnidad agencia;
        Caja caja;
        [Persistent(nameof(AutorizacionDocumento))]
        AutorizacionDocumento autorizacionCorrelativo;
        int? noFactura;
        Tercero.Module.BusinessObjects.Tercero cliente;
        TerceroSucursal clienteAgencia;
        TerceroDocumento clienteDocumento;
        [Persistent(nameof(Nrc))]
        TerceroDocumento nrc;
        TerceroDireccion direccionEntrega;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado vendedor;
        int notaRemision;
        EFormaPago formaPago = EFormaPago.Efectivo;
        ETipoTarjeta tipoTarjeta;
        string noTarjeta;
        SBT.Apps.Tercero.Module.BusinessObjects.Banco banco;
        string noReferenciaPago;
        [Persistent(nameof(DiaCerrado)), DbType("bit"), Browsable(false)]
        bool diaCerrado = false;


        /// <summary>
        /// Agencia o sucursal
        /// </summary>
        [XafDisplayName("Agencia"), PersistentAlias(nameof(agencia)), VisibleInListView(false), Index(1)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ExplicitLoading]
        public EmpresaUnidad Agencia
        {
            get => agencia;
        }

        [Association("Caja-Facturas"), XafDisplayName("Caja"), Index(2)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public Caja Caja
        {
            get => caja;
            set => SetPropertyValue(nameof(Caja), ref caja, value);
        }

        /// <summary>
        /// Autorizacion de los correlativos de documentos
        /// </summary>
        [XafDisplayName("Autorización Correlativo"), PersistentAlias(nameof(autorizacionCorrelativo)),
            VisibleInListView(false), Index(4)]
        [RuleRequiredField("Venta.NoResolucion", "Save")]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ExplicitLoading]
        public AutorizacionDocumento AutorizacionCorrelativo
        {
            get => autorizacionCorrelativo;
        }

        [DbType("int"), XafDisplayName("No Factura"), RuleRequiredField("Venta.NoFactura_Requerido", "Save"), Index(5)]
        [RuleRange("Venta.NoFactura_RangoValido", DefaultContexts.Save, "[AutorizacionCorrelativo.NoDesde]", "[AutorizacionCorrelativo.NoHasta]",
            ParametersMode.Expression, SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "El [NoFactura] debe estar en el rango entre [NoDesde] y [NoHasta] de la autorizacion de documentos")]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public int? NoFactura
        {
            get => noFactura;
            set => SetPropertyValue(nameof(NoFactura), ref noFactura, value);
        }

        /// <summary>
        /// Cliente. Es requerido en el caso de documento es: credito fiscal, factura de consumidor final, factura de exportacion
        /// </summary>
        [XafDisplayName("Cliente"), RuleRequiredField("Venta.Cliente_Requerido", DefaultContexts.Save,
            TargetCriteria = "[TipoFactura.Codigo] In ('COVE01', 'COVE02', 'COVE03')")]
        [Index(6), VisibleInLookupListView(true)]
        [DetailViewLayout("Datos del Cliente", LayoutGroupType.SimpleEditorsGroup, 1)]
        [ExplicitLoading]
        public Tercero.Module.BusinessObjects.Tercero Cliente
        {
            get => cliente;
            set
            {
                bool changed = SetPropertyValue(nameof(Cliente), ref cliente, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    if (Cliente.DireccionPrincipal != null)
                        DireccionEntrega = Cliente.DireccionPrincipal;
                    if (Cliente.Giros.Count > 0)
                        Giro = Cliente.Giros.FirstOrDefault<TerceroGiro>();
                    if (string.Compare(TipoFactura.Codigo, "COVE01", StringComparison.Ordinal) == 0 || TipoFactura.Codigo == "COVE06")
                    {
                        ClienteDocumento = Cliente.Documentos.FirstOrDefault(TerceroDocumento =>
                                          (TerceroDocumento.Tipo.Codigo == "NIT" && TerceroDocumento.Vigente == true));
                        nrc = Cliente.Documentos.FirstOrDefault(TerceroDocumento =>
                                          TerceroDocumento.Tipo.Codigo == "NRC" && TerceroDocumento.Vigente == true);
                        OnChanged(nameof(Nrc));
                    }
                    else if (string.Compare(TipoFactura.Codigo, "COVE02", StringComparison.Ordinal) == 0)
                    {
                        TerceroDocumento doc = Cliente.Documentos.FirstOrDefault(TerceroDocumento =>
                                           (string.Compare(TerceroDocumento.Tipo.Codigo, "NIT", StringComparison.Ordinal) == 0 && TerceroDocumento.Vigente == true));
                        if (doc == null)
                            doc = Cliente.Documentos.FirstOrDefault();
                        if (doc != null)
                            ClienteDocumento = doc;
                    }
                }
            }
        }


        [XafDisplayName("Cliente Agencia"), VisibleInListView(false), Index(7)]
        [DetailViewLayout("Datos del Cliente", LayoutGroupType.SimpleEditorsGroup, 1)]
        public TerceroSucursal ClienteAgencia
        {
            get => clienteAgencia;
            set => SetPropertyValue(nameof(ClienteAgencia), ref clienteAgencia, value);
        }

        /// <summary>
        /// No de registro del contribuyente. Es requerido cuando el documento es credito fiscal o sujeto excluido
        /// Implementar una validacion probablemente una RuleFromBoolProperty o una mas compleja
        /// </summary>
        [XafDisplayName("NRC"), ToolTip("No Registro Contribuyente")]
        [VisibleInListView(false), Index(8), PersistentAlias(nameof(nrc))]
        // Documento de venta a sujeto excluido debe ser el 'COVE08'
        [RuleRequiredField("Venta.NRC_Requerido", DefaultContexts.Save, TargetCriteria = "[TipoFactura.Categoria] == 15 && [TipoFactura.Codigo] In ('COVE01', 'COVE08')")]
        [DetailViewLayout("Datos del Cliente", LayoutGroupType.SimpleEditorsGroup, 1)]
        public TerceroDocumento Nrc => nrc;


        [XafDisplayName("Giro"), Index(9), Persistent(nameof(Giro))]
        [RuleRequiredField("Venta.Giro_Requerido", DefaultContexts.Save, TargetCriteria = "[TipoFactura.Codigo] In ('COVE01', 'COVE06')",
            ResultType = ValidationResultType.Information)]
        [DataSourceProperty("Cliente.Giros"), VisibleInListView(false)]
        [DetailViewLayout("Datos del Cliente", LayoutGroupType.SimpleEditorsGroup, 1)]
        public SBT.Apps.Tercero.Module.BusinessObjects.TerceroGiro Giro
        {
            get => giro;
            set => SetPropertyValue(nameof(Giro), ref giro, value);
        }
        /// <summary>
        /// En creditos fiscales es el NIT, en factura de consumidor puede ser: DUI, NIT, PASAPORTE
        /// </summary>
        [XafDisplayName("Cliente Documento"), ToolTip("Documento de identificación del cliente, cuando es requerido")]
        [DataSourceCriteria("Tercero == '@This.Cliente' && Vigente == True")]
        [VisibleInListView(false), Index(10)]
        [RuleRequiredField("Venta.ClienteDocumento_Requerido", DefaultContexts.Save, TargetCriteria = "[TipoFactura.Categoria] == 15 && [TipoFactura.Codigo] In ('COVE01', 'COVE02')")]
        [DetailViewLayout("Datos del Cliente", LayoutGroupType.SimpleEditorsGroup, 1)]
        public TerceroDocumento ClienteDocumento
        {
            get => clienteDocumento;
            set => SetPropertyValue(nameof(ClienteDocumento), ref clienteDocumento, value);
        }

        /// <summary>
        /// Direccion de entrega. Es requerido cuando el documento es cualquiera de los siguientes: credito fiscal, factura de
        /// consumidor final, factura de exportacion
        /// </summary>
        [XafDisplayName("Dirección Entrega"), VisibleInListView(false), Index(11)]
        [RuleRequiredField("Venta.DireccionEntrega", DefaultContexts.Save,
            TargetCriteria = "[TipoFactura.Codigo] In ('COVE01', 'COVE02', 'COVE03')", ResultType = ValidationResultType.Information)]
        [DataSourceCriteria("Tercero == @This.Cliente And Activa == True")]
        [DetailViewLayout("Datos del Cliente", LayoutGroupType.SimpleEditorsGroup, 1)]
        public TerceroDireccion DireccionEntrega
        {
            get => direccionEntrega;
            set => SetPropertyValue(nameof(DireccionEntrega), ref direccionEntrega, value);
        }

        /// <summary>
        /// Vendedor. En los formularios es la columna Venta a Cuenta de
        /// Es requerido cuando el documento es: credito fiscal, factura consumidor final, factura de exportacion
        /// </summary>
        /// <remarks>
        /// PENDIENTE. Completar el DataSourceCriteria con la Unidad = Ventas y Cargo = Vendedor. Puede hacerse en el model
        /// </remarks>
        [XafDisplayName("Vendedor")]
        [RuleRequiredField("Venta.Vendedor_Requerido", "Save",
            TargetCriteria = "[TipoFactura.Codigo] In ('COVE01', 'COVE02', 'COVE03') ", SkipNullOrEmptyValues = true)]
        [DataSourceCriteria("[Empresa.Oid] = EmpresaActualOid() And [Unidad] == ? And [Cargo] == ?")]
        [VisibleInListView(false), Index(14)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ExplicitLoading]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Vendedor
        {
            get => vendedor;
            set => SetPropertyValue(nameof(Vendedor), ref vendedor, value);
        }

        /// <summary>
        /// Nota de Remisión. 
        /// </summary>
        /// <remarks>
        /// PENDIENTE. Crear el BO NotaRemision, remplazar int por el tipo correspondiente al BO y crear la asociacion
        /// </remarks>
        [XafDisplayName("Nota Remisión"), VisibleInListView(false), Index(15)]
        [DetailViewLayout("Datos del Cliente", LayoutGroupType.SimpleEditorsGroup, 1)]
        public int NotaRemision
        {
            get => notaRemision;
            set => SetPropertyValue(nameof(NotaRemision), ref notaRemision, value);
        }

        [XafDisplayName("Forma de Pago"), Index(16)]
        [DbType("smallint")]
        [RuleRequiredField("Venta.FormaPago_Requerido", "Save")]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public EFormaPago FormaPago
        {
            get => formaPago;
            set => SetPropertyValue(nameof(FormaPago), ref formaPago, value);
        }
        [XafDisplayName("Tipo Tarjeta"), DbType("varchar(12)"), VisibleInListView(false), Index(17)]
        [RuleRequiredField("Venta.TipoTarjeta_Requerido", "Save", TargetCriteria = "[FormaPago] In (2, 3)",
             ResultType = ValidationResultType.Warning)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        [ExplicitLoading]
        public ETipoTarjeta TipoTarjeta
        {
            get => tipoTarjeta;
            set => SetPropertyValue(nameof(TipoTarjeta), ref tipoTarjeta, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("No Tarjeta"), VisibleInListView(false), Index(18)]
        [RuleRequiredField("Venta.NoTarjeta_Requerido", DefaultContexts.Save, TargetCriteria = "[TipoTarjeta] Is Not Null",
             ResultType = ValidationResultType.Warning)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string NoTarjeta
        {
            get => noTarjeta;
            set => SetPropertyValue(nameof(NoTarjeta), ref noTarjeta, value);
        }

        [XafDisplayName("Banco Emisor"), VisibleInListView(false), Index(19)]
        [ToolTip("Banco o tercero relacionado al cheque, tarjeta o pago electrónico", "Banco o Tercero", ToolTipIconType.Information)]
        [RuleRequiredField("Venta.Banco_Emisor", DefaultContexts.Save, TargetCriteria = "[FormaPago] != 0 And [NoTarjeta] Is Not Null",
            ResultType = ValidationResultType.Warning)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Banco Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }
        /// <summary>
        /// No de Referencia de Pago: Puede ser el numero de cheque, Id de transferencia, remesa, No de Vaucher por pago con tarjeta, etc.
        /// </summary>
        [Size(25), DbType("varchar(25)"), XafDisplayName("No Referencia Pago"), VisibleInListView(false), Index(20)]
        [ToolTip("No de referencia del pago: No de cheque, ID Remesa, ID pago electrónico, No vaucher", "Referencia Pago",
            ToolTipIconType.Information)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string NoReferenciaPago
        {
            get => noReferenciaPago;
            set => SetPropertyValue(nameof(NoReferenciaPago), ref noReferenciaPago, value);
        }

        [PersistentAlias(nameof(diaCerrado)), XafDisplayName("Día Cerrado"), VisibleInListView(false), Index(29)]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        public bool DiaCerrado
        {
            get { return diaCerrado; }
        }
   
        [XafDisplayName("Tercero no Domiciliado"), VisibleInListView(false), VisibleInLookupListView(false)]
        [ToolTip("La venta es a cuenta de este Tercero No Domiciliado")]
        [Delayed(true)]
        [DataSourceCriteria("[Activo] == True && [TipoContribuyente] == 0 && [DireccionPrincipal.Pais] != 'SLV'")]
        public Tercero.Module.BusinessObjects.Tercero TerceroNoDomiciliado
        {
            get => GetDelayedPropertyValue<Tercero.Module.BusinessObjects.Tercero>(nameof(TerceroNoDomiciliado));
            set => SetDelayedPropertyValue<Tercero.Module.BusinessObjects.Tercero>(nameof(TerceroNoDomiciliado), value);
        }

        /// <summary>
        /// Ventas gravadas con tasa cero, normalmente son ventas a zonas francas y depositos de perfeccionamiento de activos (DPA)
        /// </summary>
        /// <remarks>
        /// Originalmente se habia agregado una clasificacion de TipoContribuyente para el tercero (cliente), pero no a todos
        /// los bienes o servicios que adquiere el cliente se le aplica la tasa cero. 
        /// Se intento agregar la clasificacion en Producto.Categoria.ClasificacionIva, pero no aplica por producto, porque
        /// la ley establece que solo se debe aplicar a los bienes y servicios necesarios para la transformacion de los bienes 
        /// que fabrica el cliente, de esta manera solo en algunos casos puntuales un bien se aplica tasa cero.
        /// Se opto por dejarlo como propiedad de la Venta porque si bien se comportan como si fuesen exportaciones, no 
        /// siempre se emite un comprobante de este tipo; puede ser factura de consumidor final y trasladado a la columna
        /// de exportaciones en el libro de ventas.
        /// Mas info en: https://www.mh.gob.sv/downloads/pdf/DC9226_Ley_del_Impuesto_a_la_Transferencia_de_Bienes_Muebles_y_a_la_Prestacion_de_Servicios.pdf art. 65
        ///              https://elsalvador.eregulations.org/media/DACG-006-2008%20Zonas%20francas.pdf -- inciso primero art. 25
        ///              http://ri.ues.edu.sv/id/eprint/19810/1/Trabajo%20de%20Graduaci%C3%B3n%20T3%202018.pdf pagina 77
        /// </remarks>
        [XafDisplayName("Gravada Tasa Cero"), DbType("bit")]
        [ToolTip("Ventas gravadas con tasa cero a zonas francas y depositos para perfeccionamiento de activo (DPA)")]
        [VisibleInListView(false)]
        public bool GravadaTasaCero
        {
            get => gravadaTasaCero;
            set => SetPropertyValue(nameof(GravadaTasaCero), ref gravadaTasaCero, value);
        }

        #endregion

        #region Colecciones
        [Association("Venta-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle"), Index(0)]
        public XPCollection<VentaDetalle> Detalles
        {
            get
            {
                return GetCollection<VentaDetalle>(nameof(Detalles));
            }
        }

        [Association("Factura-ResumenTributos"), DevExpress.Xpo.Aggregated, XafDisplayName("Resumen Tributos"), Index(1)]
        public XPCollection<VentaResumenTributo> ResumenTributos
        {
            get
            {
                return GetCollection<VentaResumenTributo>(nameof(ResumenTributos));
            }
        }

        [Association("Venta-CxCDocumentos"), Index(2), XafDisplayName("Cuenta por Cobrar")]
        public XPCollection<SBT.Apps.CxC.Module.BusinessObjects.CxCDocumento> CxCDocumentos
        {
            get
            {
                return GetCollection<SBT.Apps.CxC.Module.BusinessObjects.CxCDocumento>(nameof(CxCDocumentos));
            }
        }

        #endregion


        #region Metodos
        /// <summary>
        /// Reescribimos el metodo para generar un correlativo de documento por empresa caja tipo de documento y año
        /// </summary>
        /// <returns></returns>
        protected override int CorrelativoDoc()
        {
            string sCriteria = "Empresa.Oid == ? && Caja.Oid == ? && TipoFactura.Codigo == ? && GetYear(Fecha) == ?";
            object max = Session.Evaluate<Venta>(CriteriaOperator.Parse("Max(Numero)"),
                                                        CriteriaOperator.Parse(sCriteria, Empresa.Oid, Caja.Oid, TipoFactura.Codigo, Fecha));
            return (max != null) ? Convert.ToInt32(max) : 1;
        }

        protected override void DoTipoFacturaChanged(bool forceChangeEvents, Listas oldValue)
        {
            base.DoTipoFacturaChanged(forceChangeEvents, oldValue);
            AutorizacionDocumento resoluc;
            if (Caja != null)
                resoluc = Session.FindObject<AutorizacionDocumento>(CriteriaOperator.Parse("Agencia.Oid == ? && Caja.Oid == ? && Tipo.Codigo == ? && Activo == True", 
                    Agencia.Oid, Caja.Oid, TipoFactura.Codigo));
            else
                resoluc = Session.FindObject<AutorizacionDocumento>(CriteriaOperator.Parse("Agencia.Oid == ? && Tipo.Codigo == ? && Activo == True",
                    Agencia.Oid, TipoFactura.Codigo));
            var oldresoluc = autorizacionCorrelativo;
            autorizacionCorrelativo = resoluc;
            OnChanged(nameof(AutorizacionCorrelativo), oldresoluc, resoluc);
        }

        public override void UpdateTotalExenta(bool forceChangeEvents)
        {
            base.UpdateTotalExenta(forceChangeEvents);
            decimal? oldExenta = exenta;
            decimal tempExenta = 0.0m;
            foreach (VentaDetalle detalle in Detalles)
                tempExenta += Convert.ToDecimal(detalle.Exenta);
            exenta = tempExenta;
            if (forceChangeEvents)
                OnChanged(nameof(Exenta), oldExenta, exenta);
        }

        public override void UpdateTotalGravada(bool forceChangeEvents)
        {
            base.UpdateTotalGravada(forceChangeEvents);
            decimal? oldGravada = gravada;
            gravada = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Gravada])")));
            if (forceChangeEvents)
                OnChanged(nameof(Gravada), oldGravada, gravada);
        }

        public override void UpdateTotalIva(bool forceChangeEvents)
        {
            base.UpdateTotalIva(forceChangeEvents);
            decimal? oldIva = iva;
            iva = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Iva])")));
            if (forceChangeEvents)
                OnChanged(nameof(Iva), oldIva, iva);
        }

        public override void UpdateTotalNoSujeta(bool forceChangeEvents)
        {
            base.UpdateTotalNoSujeta(forceChangeEvents);
            decimal? oldNoSujeta = noSujeta;
            noSujeta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([NoSujeta])")));
            if (forceChangeEvents)
                OnChanged(nameof(NoSujeta), oldNoSujeta, noSujeta);
        }

        protected override void OnSaving()
        {
            foreach (VentaDetalle item in Detalles)
            {
                if (!Session.IsNewObject(item))
                    continue;
                switch (item.Producto.Categoria.MetodoCosteo)
                {
                    case EMetodoCosteoInventario.Promedio:
                        item.Costo = item.Producto.CostoPromedio;
                        break;
                    case EMetodoCosteoInventario.Unitario:
                        item.Costo = item.PrecioUnidad;
                        break;
                    default:
                        {
                            InventarioLote lotep = item.ObtenerLote();
                            item.Lote = lotep;
                            item.Costo = lotep != null ? lotep.Costo : item.Producto.CostoPromedio;
                            break;
                        }
                }
            }
            base.OnSaving();
        }

        protected override void RefreshTiposDeFacturas()
        {
            base.RefreshTiposDeFacturas();
            if (fTiposDeFacturas == null)
                return;
            fTiposDeFacturas.Criteria = CriteriaOperator.Parse("[Categoria] == 15 && [Activo] == True && [Codigo] In ('COVE01', 'COVE02', 'COVE03', 'COVE04', 'COVE05', 'COVE06')");
        }

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}