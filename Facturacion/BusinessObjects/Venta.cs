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
using SBT.Apps.Empleado.Module.BusinessObjects;
using DevExpress.ExpressApp.ConditionalAppearance;

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
    [Appearance("Venta.CreditoFiscal", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, 
        Criteria = "[TipoFactura.Categoria] == 15 && [TipoFactura.Codigo] != 'COVE01'", Context = "*", 
        TargetItems = "Caja;NRC;NotaRemision;Iva;IvaPercibido;IvaRetenido;ResumenTributos")]
    
    //[ImageName("BO_Contact")]
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
            Listas fpago = Session.GetObjectByKey<Listas>("FPA01");
            if (fpago != null)
                FormaPago = fpago;
            if (((Usuario)SecuritySystem.CurrentUser).Agencia != null)
                agencia = ((Usuario)SecuritySystem.CurrentUser).Agencia;
            // PENDIENTE asignar la caja de forma automatica, a partir de la agencia

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        SBT.Apps.Tercero.Module.BusinessObjects.TerceroGiro giro;
        [Persistent(nameof(Agencia))]
        EmpresaUnidad agencia;
        Caja caja;
        [Persistent(nameof(AutorizacionDocumento))]
        AutorizacionDocumento autorizacionCorrelativo;
        int noFactura;
        Tercero.Module.BusinessObjects.Tercero cliente;
        TerceroSucursal clienteAgencia;
        TerceroDocumento clienteDocumento;
        [Persistent(nameof(NRC))]
        TerceroDocumento nRC;
        TerceroDireccion direccionEntrega;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado vendedor;
        int notaRemision;
        Listas formaPago;
        Listas tipoTarjeta;
        string noTarjeta;
        SBT.Apps.Tercero.Module.BusinessObjects.Banco banco;
        string noReferenciaPago;
        [Persistent(nameof(DiaCerrado)), DbType("bit"), Browsable(false)]
        bool diaCerrado = false;


        /// <summary>
        /// Agencia o sucursal
        /// </summary>
        [XafDisplayName("Agencia"), PersistentAlias(nameof(agencia)), VisibleInListView(false), Index(1)]
        public EmpresaUnidad Agencia
        {
            get => agencia;
        }

        [Association("Caja-Facturas"), XafDisplayName("Caja"), Index(2)]
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
        public AutorizacionDocumento AutorizacionCorrelativo
        {
            get => autorizacionCorrelativo;
        }

        [DbType("int"), XafDisplayName("No Factura"), RuleRequiredField("Venta.NoFactura_Requerido", "Save"), Index(5)]
        public int NoFactura
        {
            get => noFactura;
            set => SetPropertyValue(nameof(NoFactura), ref noFactura, value);
        }

        /// <summary>
        /// Cliente. Es requerido en el caso de documento es: credito fiscal, factura de consumidor final, factura de exportacion
        /// </summary>
        [XafDisplayName("Cliente"), RuleRequiredField("Venta.Cliente_Requerido", DefaultContexts.Save,
            TargetCriteria = "'@This.Tipo.Codigo' In ('COVE01', 'COVE02', 'COVE03')")]
        [Index(6), VisibleInLookupListView(true)]
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
                        nRC = Cliente.Documentos.FirstOrDefault(TerceroDocumento =>
                                          TerceroDocumento.Tipo.Codigo == "NRC" && TerceroDocumento.Vigente == true);
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
        [VisibleInListView(false), Index(8), PersistentAlias(nameof(nRC))]
        // Documento de venta a sujeto excluido debe ser el 'COVE08'
        [RuleRequiredField("Venta.NRC_Requerido", DefaultContexts.Save, TargetCriteria = "[TipoFactura.Categoria] == 15 && [TipoFactura.Codigo] In ('COVE01', 'COVE08')")]
        public TerceroDocumento NRC => nRC;


        [XafDisplayName("Giro"), Index(9), Persistent(nameof(Giro))]
        [RuleRequiredField("Venta.Giro_Requerido", DefaultContexts.Save, TargetCriteria = "'@This.Tipo.Codigo' In ('COVE01', 'COVE06')")]
        [DataSourceProperty("Cliente.Giros"), VisibleInListView(false)]
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
            TargetCriteria = "@This.Tipo.Codigo In ('COVE01', 'COVE02', 'COVE03') ")]
        [DataSourceCriteria("Tercero == @This.Cliente And Activa == True")]
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
        [RuleRequiredField("DocumentoVenta.Vendedor_Requerido", "Save", 
            TargetCriteria = "@This.Tipo.Codigo In ('COVE01', 'COVE02', 'COVE03') ")]
        [DataSourceCriteria("[Empresa] == @This.Empresa And [Unidad] == ? And [Cargo] == ?")]
        [VisibleInListView(false), Index(14)]
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
        public int NotaRemision
        {
            get => notaRemision;
            set => SetPropertyValue(nameof(NotaRemision), ref notaRemision, value);
        }

        [XafDisplayName("Forma de Pago"), DbType("varchar(12)"), Index(16)]
        [DataSourceCriteria("[Categoria] == 5 And [Activo] == True")]   // categoria 5 son las formas de pago
        public Listas FormaPago
        {
            get => formaPago;
            set => SetPropertyValue(nameof(FormaPago), ref formaPago, value);
        }
        [XafDisplayName("Tipo Tarjeta"), DbType("varchar(12)"), VisibleInListView(false), Index(17)]
        [DataSourceCriteria("[Categoria] == 6 And [Activo] == True")]   // categoria 6 son tarjetas de credito
        [RuleRequiredField("DocumentoVenta.FomaPago_Requerido", "Save", TargetCriteria = "[FormaPago.Codigo] In ('FPA03', 'FPA04')",
             ResultType = ValidationResultType.Warning)]
        public Listas TipoTarjeta
        {
            get => tipoTarjeta;
            set => SetPropertyValue(nameof(TipoTarjeta), ref tipoTarjeta, value);
        }

        [Size(25), DbType("varchar(25)"), XafDisplayName("No Tarjeta"), VisibleInListView(false), Index(18)]
        [RuleRequiredField("Venta.NoTarjeta_Requerido", DefaultContexts.Save, TargetCriteria = "[TipoTarjeta] Is Not Null",
             ResultType = ValidationResultType.Warning)]
        public string NoTarjeta
        {
            get => noTarjeta;
            set => SetPropertyValue(nameof(NoTarjeta), ref noTarjeta, value);
        }

        [XafDisplayName("Banco Emisor"), VisibleInListView(false), Index(19)]
        [ToolTip("Banco o tercero relacionado al cheque, tarjeta o pago electrónico", "Banco o Tercero", ToolTipIconType.Information)]
        [RuleRequiredField("Venta.Banco_Emisor", DefaultContexts.Save, TargetCriteria = "[FormaPago.Codigo] != 'FPA01' And [NoTarjeta] Is Not Null", 
            ResultType = ValidationResultType.Warning)]
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
        public string NoReferenciaPago
        {
            get => noReferenciaPago;
            set => SetPropertyValue(nameof(NoReferenciaPago), ref noReferenciaPago, value);
        }

        [PersistentAlias(nameof(diaCerrado)), XafDisplayName("Día Cerrado"), VisibleInListView(false), Index(29)]
        public bool DiaCerrado
        {
            get { return diaCerrado; }
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

        [Association("Venta-CxCDocumentos"), Index(2)]
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
            string sCriteria = "Empresa.Oid == ? && Caja.Oid == ? && Tipo.Codigo == ? && GetYear(Fecha) == ?";
            object max = Session.Evaluate<Venta>(CriteriaOperator.Parse("Max(Numero)"), 
                                                        CriteriaOperator.Parse(sCriteria, Empresa.Oid, Caja.Oid, TipoFactura.Codigo, Fecha));
            return (max != null) ? Convert.ToInt32(max) : 1;
        }

        protected override void DoTipoFacturaChanged(bool forceChangeEvents, Listas oldValue)
        {
            base.DoTipoFacturaChanged(forceChangeEvents, oldValue);
            AutorizacionDocumento resolucionDoc = Session.FindObject<AutorizacionDocumento>(
                CriteriaOperator.Parse("Agencia.Oid == ? && Caja.Oid == ? && Tipo.Codigo == ? && Activo == True", Agencia.Oid, Caja.Oid, TipoFactura.Codigo));
            if (resolucionDoc != null)
            {
                autorizacionCorrelativo = resolucionDoc;
                // evaluar si el correlativo esta bien calcularlo aqui
                int noFact = Convert.ToInt32(Session.Evaluate<Venta>(CriteriaOperator.Parse("max([NoFactura])"),
                    CriteriaOperator.Parse("AutorizacionCorrelativo.Oid == ?", autorizacionCorrelativo.Oid)));
                if (noFact >= autorizacionCorrelativo.NoDesde && noFact < autorizacionCorrelativo.NoHasta)
                    noFactura = noFact + 1;
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

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}