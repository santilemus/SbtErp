using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
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
    [DefaultClassOptions, ModelDefault("Caption", "Venta"), NavigationItem("Facturación"), DefaultProperty(nameof(NoFactura))]
    [Persistent("Venta")]
    //[Appearance("Venta.CreditoFiscal", Criteria = "[TipoFactura.Categoria] == 15 && [TipoFactura.Codigo] != 'COVE01'",
    //    AppearanceItemType = "ViewItem",
    //    Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "DetailView",
    //    TargetItems = "Caja;NRC;NotaRemision;Iva;IvaPercibido;IvaRetenido;ResumenTributos")]
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
            {
                var unidad = this.Session.GetObjectByKey<EmpresaUnidad>(((Usuario)SecuritySystem.CurrentUser).Agencia.Oid);
                agencia = unidad;
            }
            giro = null;
            formaPago = EFormaPago.Efectivo;
            ExportacionServicio = false;
            // PENDIENTE asignar la caja de forma automatica, a partir de la agencia

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        bool exportacionServicio;
        bool gravadaTasaCero;
        SBT.Apps.Tercero.Module.BusinessObjects.TerceroGiro giro;
        [Persistent(nameof(Agencia))]
        EmpresaUnidad agencia;
        Caja caja;
        [Persistent(nameof(AutorizacionDocumento))]
        AutorizacionDocumento autorizacionDocumento;
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
        ETipoTarjeta? tipoTarjeta;
        string noTarjeta;
        SBT.Apps.Tercero.Module.BusinessObjects.Banco banco;
        string noReferenciaPago;
        [Persistent(nameof(DiaCerrado)), DbType("bit"), Browsable(false)]
        readonly bool diaCerrado = false;


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
        [XafDisplayName("Autorización Correlativo"), PersistentAlias(nameof(autorizacionDocumento)),
            VisibleInListView(false), Index(4)]
        [RuleRequiredField("Venta.NoResolucion", "Save")]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [ExplicitLoading]
        public AutorizacionDocumento AutorizacionDocumento
        {
            get => autorizacionDocumento;
        }

        [DbType("int"), XafDisplayName("No Factura"), RuleRequiredField("Venta.NoFactura_Requerido", "Save"), Index(5)]
        [RuleRange("Venta.NoFactura_RangoValido", DefaultContexts.Save, "[AutorizacionDocumento.NoDesde]", "[AutorizacionDocumento.NoHasta]",
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
            TargetCriteria = "[TipoFactura.Codigo] In ('COVE01', 'COVE02', 'COVE03')", SkipNullOrEmptyValues = false)]
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
                    else if (Cliente.Direcciones.Count > 0)
                        DireccionEntrega = Cliente.Direcciones.FirstOrDefault<TerceroDireccion>();
                    if (Cliente.Giros.Count > 0)
                        Giro = Cliente.Giros.FirstOrDefault<TerceroGiro>();
                    TerceroDocumento doc;
                    if (Cliente.TipoPersona == TipoPersona.Juridica)
                        doc = Cliente.Documentos.FirstOrDefault(TerceroDocumento => (TerceroDocumento.Tipo.Codigo == "NIT" && TerceroDocumento.Vigente == true));
                    else
                        doc = Cliente.Documentos.FirstOrDefault(TerceroDocumento =>
                              ((TerceroDocumento.Tipo.Codigo == "NIT" || TerceroDocumento.Tipo.Codigo == "DUI") && TerceroDocumento.Vigente == true));
                    ClienteDocumento = doc;
                    if (string.Compare(TipoFactura.Codigo, "COVE01", StringComparison.Ordinal) == 0 || TipoFactura.Codigo == "COVE06")
                    {
                        nrc = Cliente.Documentos.FirstOrDefault(TerceroDocumento =>
                                          TerceroDocumento.Tipo.Codigo == "NRC" && TerceroDocumento.Vigente == true);
                        OnChanged(nameof(Nrc));
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
        /// En creditos fiscales es el NIT o DUI, en factura de consumidor puede ser: DUI, NIT, PASAPORTE
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
        [DataSourceCriteria("[Tercero] == '@This.Cliente' And Activa == True")]
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
        [DataSourceCriteria("[Empresa.Oid] = EmpresaActualOid()")] // And [Unidad] == ? And [Cargo] == ?")]
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
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public EFormaPago FormaPago
        {
            get => formaPago;
            set
            {
                bool changed = SetPropertyValue(nameof(FormaPago), ref formaPago, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    if (FormaPago == EFormaPago.Efectivo || FormaPago == EFormaPago.Otro)
                    {
                        Banco = null;
                        NoReferenciaPago = null;
                    }
                    if (FormaPago == EFormaPago.Tarjeta)
                        TipoTarjeta = ETipoTarjeta.Debito;
                    else
                    {
                        TipoTarjeta = null;
                        NoTarjeta = null;
                    }
                }
            }
        }
        [XafDisplayName("Tipo Tarjeta"), DbType("varchar(12)"), VisibleInListView(false), Index(17)]
        [RuleRequiredField("Venta.TipoTarjeta_Requerido", "Save", TargetCriteria = "[FormaPago] == 2",
             ResultType = ValidationResultType.Warning)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public ETipoTarjeta? TipoTarjeta
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

        /// <summary>
        /// Total del documento
        /// </summary>
        /// <remarks>
        /// El artículo del siguiente link dice que es mejor calcular la propiedad directamente porque es preferible en
        /// lugar de persistentalias para calculos pesados (o continuos quiz). Por eso se hizo el cambio, sino produce el
        /// efecto esperado regresar a la expresion del PersistentAlias. Hay que ver que funcione bien en app web
        /// https://github.com/DevExpress/XPO/blob/master/Tutorials/WinForms/Classic/create-persistent-classes-and-connect-xpo-to-database.md
        /// </remarks>
        [PersistentAlias("[SubTotal] + [IvaPercibido] - [IvaRetenido] + [NoSujeta] + [Exenta] ")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [XafDisplayName("Total"), Index(28)]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        [VisibleInListView(true)]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total))); // SubTotal + IvaPercibido ?? 0.0m - IvaRetenido ?? 0.0m + NoSujeta ?? 0.0m + Exenta ?? 0.0m;


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

        /// <summary>
        /// Exportacion de servicios, válido solo para la factura de exportacion  
        /// </summary>
        /// <remarks>
        /// Incorporado el 09/junio/2023. Originalmente se penso manejar esto que es importante para la separacion en los  
        /// libros de IVA, a través de la categoría de producto, pero es complicado por dos razones: 1) Puede mezclar en 
        /// una misma factura de exportacion productos y servicios y 2) lo más importante complica innecesariamente la
        /// generacion del libro de contribuyentes porque la tabla principal para la generación es el detalle de la venta.
        /// </remarks>
        public bool ExportacionServicio
        {
            get => exportacionServicio;
            set => SetPropertyValue(nameof(ExportacionServicio), ref exportacionServicio, value);
        }

        #endregion

        #region Colecciones
        [Association("Venta-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<VentaDetalle> Detalles => GetCollection<VentaDetalle>(nameof(Detalles));

        [Association("Factura-ResumenTributos"), DevExpress.Xpo.Aggregated, XafDisplayName("Resumen Tributos"), Index(1)]
        public XPCollection<VentaResumenTributo> ResumenTributos => GetCollection<VentaResumenTributo>(nameof(ResumenTributos));


        [Association("Venta-CxCTransacciones"), Index(2), XafDisplayName("Transacciones CxC"), DevExpress.Xpo.Aggregated]
        public XPCollection<CxC.Module.BusinessObjects.CxCTransaccion> CxCTransacciones => GetCollection<CxC.Module.BusinessObjects.CxCTransaccion>(nameof(CxCTransacciones));


        #endregion


        #region Metodos
        /// <summary>
        /// Reescribimos el metodo para generar un correlativo de documento por empresa caja tipo de documento y año
        /// </summary>
        /// <returns></returns>
        protected override int CorrelativoDoc()
        {
            object max;
            string sCriteria = "Empresa.Oid == ? && TipoFactura.Codigo == ? && GetYear(Fecha) == ?";
            if (Caja != null)
            {
                sCriteria = "Empresa.Oid == ? && Caja.Oid == ? && TipoFactura.Codigo == ? && GetYear(Fecha) == ?";
                max = Session.Evaluate<Venta>(CriteriaOperator.Parse("Max(Numero)"),
                                              CriteriaOperator.Parse(sCriteria, Empresa.Oid, Caja.Oid, TipoFactura.Codigo, Fecha.Year));
            }
            else
                max = Session.Evaluate<Venta>(CriteriaOperator.Parse("Max(Numero)"),
                                              CriteriaOperator.Parse(sCriteria, Empresa.Oid, TipoFactura.Codigo, Fecha.Year));
            return Convert.ToInt32(max ?? 0) + 1;
        }

        private int? GetNoFactura(int autorizacionCorrelat)
        {
            if (AutorizacionDocumento != null)
            {
                string sCriteria = "Empresa.Oid == ? && TipoFactura.Codigo == ? && AutorizacionDocumento.Oid == ?";
                return Convert.ToInt32(Session.Evaluate<Venta>(CriteriaOperator.Parse("Max(NoFactura) + 1"),
                    CriteriaOperator.Parse(sCriteria, Empresa.Oid, TipoFactura.Codigo, autorizacionCorrelat)));
            }
            else
                return null;
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
            var oldresoluc = autorizacionDocumento;
            autorizacionDocumento = resoluc;
            OnChanged(nameof(AutorizacionDocumento), oldresoluc, resoluc);
            if (autorizacionDocumento != null)
            {
                NoFactura = GetNoFactura(autorizacionDocumento.Oid);
                OnChanged(nameof(NoFactura));
            }
        }

        protected override void DoGravadaChanged(bool forceChangeEvents, decimal? oldValue)
        {
            /// NOTA: Revisar este metodo porque hay que considerar el BO TributoCategoria, para que se calculen unicamente 
            /// cuando hay una relacion entre el tributo y la categoria del producto

            // no calculamos el IVA porque tuvo que calcularse en los detalles (por producto)
            // calculamos la retencion IVA Tributo Oid = 2; cuando el cliente es gran contribuyente. Se calcula porque
            // del saldo a cobrar se deduce la retencion que hara el gran contribuyente, por lo tanto si el cliente
            // es de la misma categoria que la empresa no se aplica (considerarlo en la formula)
            IvaRetenido = CalcularTributo(2);
            //OnChanged(nameof(IvaRetenido));
            // calculamos la percepcion cuando aplica. Cuando la empresa es gran contribuyente y el cliente no lo es
            IvaPercibido = CalcularTributo(3);
            //OnChanged(nameof(IvaPercibido));
            base.DoGravadaChanged(forceChangeEvents, oldValue);
        }

        /// <summary>
        /// Calcula el valor del tributo cuyo Oid se recibe en el parametro, evaluando la formula correspondiente. 
        /// El tributo o tasa se define en el BO Tributo, que incluye dos propiedades claves: 1) El tipo de BO en el cual se calcula, 
        /// 2) La propiedad con la formula que sera evaluada al calcular el tributo
        /// </summary>
        /// <param name="oidTributo">Oid del tributo a calcular</param>
        /// <returns>Retorna el valor del tributo calculado y que resulta de evaluar la Formula</returns>
        /// <remarks>Por el momento El IVA no se evalua aca porque es un tributo de uso global. Se define el porcentaje a aplicar por Categoria de Producto</remarks>
        private decimal CalcularTributo(int oidTributo)
        {
            Tributo tributo = Session.GetObjectByKey<Tributo>(oidTributo);
            if (tributo != null)
            {
                ExpressionEvaluator eval = new (TypeDescriptor.GetProperties(tributo.TipoBO), tributo.Formula);

                //alternativa 1
                //CriteriaOperator op = CriteriaOperator.Parse(tributo.Formula);              
                //ExpressionEvaluator eval2 = new ExpressionEvaluator(TypeDescriptor.GetProperties(tributo.TipoBO), op);

                //alternativa 2. Cuando son muchos datos
                //CriteriaOperator op = CriteriaOperator.Parse(tributo.Formula);
                //Func<object, object> delegat = CriteriaCompiler.ToUntypedDelegate(op, CriteriaCompilerDescriptor.Get(tributo.TipoBO));
                //return Convert.ToDecimal(delegat(this));

                return Convert.ToDecimal(eval.Evaluate(this));
            }
            else
                return 0.0m;
        }

        /// <summary>
        /// Itera por los tributos o tasas que deben aplicarse a las categorias de los productos amparados en el detalle
        /// de la venta
        /// </summary>
        /// <remarks>
        /// Pendiente de agregar una propiedad en el BO Tributo o una manera de distinguir cuando se aplican de manera
        /// global a la venta e independiente del producto y cuando se calculan por item de la venta (dependen del producto 
        /// y por eso deben estar vinculados a una categoria)\r\n.
        /// Ademas evaluar donde invocar el metodo: una opcion es en el commiting del vcVenta, agregar una accion para
        /// calcularlos o cuando cambia el monto gravado de la venta. Con esta alterativa puede ser lento porque tiene que 
        /// invocar cada vezel metodo y hay dos bucles anidados: uno por el detalle de la venta y otro por los atributos 
        /// que aplican a cada producto (via la categoria
        /// </remarks>
        private void GenerarVentaResumenTributos()
        {
            var resumen = from d in Detalles
                          where (d.Gravada != 0.0m && d.Producto.Categoria.ClasificacionIva == EClasificacionIVA.Gravado)
                          group d by new { d.Producto.Categoria } into x
                          select new
                          {
                              x.Key.Categoria,
                              Gravada = x.Sum(y => y.Gravada)
                          };
            foreach (var item in resumen)
            {
                foreach (TributoCategoria tributoCategoria in item.Categoria.TributosCategoria)
                {
                    var vtaResumenTributo = ResumenTributos.FirstOrDefault<VentaResumenTributo>(y => y.Tributo.Oid == tributoCategoria.Tributo.Oid);
                    if (vtaResumenTributo == null)
                        this.ResumenTributos.Add(new VentaResumenTributo(Session)
                        {
                            Tributo = Session.GetObjectByKey<Tributo>(tributoCategoria.Tributo.Oid),
                            Valor = CalcularTributo(tributoCategoria.Tributo.Oid)
                        });
                    else
                    {
                        vtaResumenTributo.Valor = CalcularTributo(tributoCategoria.Tributo.Oid);
                        vtaResumenTributo.Save();
                    }
                }
            }
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
            ActualizarSaldo(Convert.ToDecimal(exenta) + SubTotal + Convert.ToDecimal(IvaPercibido) -
                    Convert.ToDecimal(IvaRetenido) + Convert.ToDecimal(NoSujeta), EEstadoFactura.Debe, true);
        }

        public override void UpdateTotalGravada(bool forceChangeEvents)
        {
            base.UpdateTotalGravada(forceChangeEvents);
            decimal? oldGravada = gravada;
            gravada = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Gravada])")));
            DoGravadaChanged(forceChangeEvents, oldGravada);
            if (forceChangeEvents)
                OnChanged(nameof(Gravada), oldGravada, gravada);
            ActualizarSaldo(Convert.ToDecimal(exenta) + SubTotal + Convert.ToDecimal(IvaPercibido) -
                    Convert.ToDecimal(IvaRetenido) + Convert.ToDecimal(NoSujeta), EEstadoFactura.Debe, true);
        }

        public override void UpdateTotalIva(bool forceChangeEvents)
        {
            base.UpdateTotalIva(forceChangeEvents);
            decimal? oldIva = iva;
            iva = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Iva])")));
            if (forceChangeEvents)
                OnChanged(nameof(Iva), oldIva, iva);
            ActualizarSaldo(Convert.ToDecimal(exenta) + SubTotal + Convert.ToDecimal(IvaPercibido) -
                    Convert.ToDecimal(IvaRetenido) + Convert.ToDecimal(NoSujeta), EEstadoFactura.Debe, true);
        }

        public override void UpdateTotalNoSujeta(bool forceChangeEvents)
        {
            base.UpdateTotalNoSujeta(forceChangeEvents);
            decimal? oldNoSujeta = noSujeta;
            noSujeta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([NoSujeta])")));
            if (forceChangeEvents)
                OnChanged(nameof(NoSujeta), oldNoSujeta, noSujeta);
            ActualizarSaldo(Convert.ToDecimal(exenta) + SubTotal + Convert.ToDecimal(IvaPercibido) -
                    Convert.ToDecimal(IvaRetenido) + Convert.ToDecimal(NoSujeta), EEstadoFactura.Debe, true);
        }

        public override void ActualizarSaldo(decimal valor, EEstadoFactura status, bool forceChangeEvents)
        {
            if (CondicionPago == ECondicionPago.Credito)
            {
                saldo = Total;
            }
            else
            {
                saldo = 0.0m;
            }
            base.ActualizarSaldo(valor, status, forceChangeEvents);
        }

        protected override void OnSaving()
        {
            if (Session is not NestedUnitOfWork && (Session.DataLayer != null) && Session.IsNewObject(this) &&
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

        protected override void RefreshTiposDeFacturas()
        {
            base.RefreshTiposDeFacturas();
            if (fTiposDeFacturas == null)
                return;
            fTiposDeFacturas.Criteria = CriteriaOperator.Parse("[Categoria] == 15 && [Activo] == True && [Codigo] In ('COVE01', 'COVE02', 'COVE03', 'COVE04', 'COVE05')");
        }

        public override void Anular(AnularParametros AnularParams)
        {
            try
            {
                base.Anular(AnularParams);
                DoAnular();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error al anular el documento", ex);
            }
        }

        protected override decimal GetTotal()
        {
            return Total;
        }
        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}