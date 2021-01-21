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
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Banco.Module.BusinessObjects;


namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// Cuenta por Cobrar.
    /// BO que corresponde a las transacciones de cuentas por cobrar. Es el encabezado y es generico para cualquier tipo
    /// de documento. Pueden ser notas de credito, debito, abonos, cheques rechazados. En resumen cualquiera para el cual
    /// existe una tipifiacion o concepto
    /// </summary>
    /// <remarks>
    /// 1. Faltan los siguientes properties: fecha del cheque,
    ///                                      moneda.  (evaluar que otros datos harian falta)
    ///                                  
    /// </remarks>
    
    [DefaultClassOptions, ModelDefault("Caption", "CxC Transacción"), NavigationItem("Cuenta por Cobrar")]
    [CreatableItem(false), Persistent(nameof(CxCTransaccion)), DefaultProperty("Numero")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxCTransaccion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxCTransaccion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(FactorCambio))]
        decimal factorCambio = 1.0m;
        Moneda moneda;
        [Persistent(nameof(Valor))]
        decimal ? valor;
        BancoTransaccion bancoTransaccion;
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado gestorCobro;
        string noTarjeta;
        string referencia;
        ECxcTransaccionEstado estado;
        SBT.Apps.Tercero.Module.BusinessObjects.Banco banco;
        string comentario;
        [Persistent(nameof(NRC))]
        TerceroDocumento nRC;
        SBT.Apps.Tercero.Module.BusinessObjects.Tercero cliente;
        [Persistent(nameof(Numero)), DbType("int")]
        int numero;
        [Persistent(nameof(FechaAnulacion)), DbType("datetime2")]
        DateTime? fechaAnulacion;
        [Persistent(nameof(UsuarioAnulo)), DbType("varchar(25)"), Size(25)]
        string usuarioAnulo;
        DateTime fecha;
        Concepto concepto;
        Listas tipoDocumento;


        [XafDisplayName("Cliente"), RuleRequiredField("CxCTransaccion.Cliente_Requerido", DefaultContexts.Save), Index(0)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Cliente
        {
            get => cliente;
            set => SetPropertyValue(nameof(Cliente), ref cliente, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha"), Index(1)]
        [RuleValueComparison("CxCTransaccion.Fecha > Fecha Factura", DefaultContexts.Save,
            ValueComparisonType.GreaterThanOrEqual, "[Venta.Fecha]", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        /// <summary>
        /// Tipo de concepto o de transaccion de cuenta por cobrar
        /// </summary>
        [Association("Concepto-Transacciones"), XafDisplayName("Tipo Concepto")]
        [RuleRequiredField("CxcTransaccion.Concepto_Requerido", DefaultContexts.Save)]
        [Index(2), VisibleInLookupListView(true)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public Concepto Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [XafDisplayName("Gestor de Cobro"), Index(3)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado GestorCobro
        {
            get => gestorCobro;
            set => SetPropertyValue(nameof(GestorCobro), ref gestorCobro, value);
        }

        /// <summary>
        /// Solo aplica para los conceptos que requieren de una autorizacion de correlativos
        /// </summary>
        [XafDisplayName("Tipo Documento"), RuleRequiredField("Venta.TipoDocumento_Requerido", DefaultContexts.Save)]
        [DataSourceCriteria("[Categoria] == 16 And [Activo] == True"), VisibleInLookupListView(true), Index(4)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public Listas TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }

        [PersistentAlias(nameof(nRC)), XafDisplayName("NRC"), Index(5), VisibleInListView(false)]
        [DetailViewLayout("Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public TerceroDocumento NRC => nRC;

        /// <summary>
        /// Numero de documento por Concepto (revisar si se maneja una agrupacion de menor nivel, asi podremos tener
        /// notas de credito==> descuento, devolucion. Pagos ==> Efectivo, Cheque, Transferencia, etc
        /// </summary>
        [PersistentAlias(nameof(numero)), XafDisplayName("Número"), Index(6)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public int Numero => numero;

        [XafDisplayName("Banco"), Index(6)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Banco Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }

        /// <summary>
        /// No de tarjeta de credito o debito
        /// </summary>
        [Size(25), DbType("varchar(25)"), XafDisplayName("No Tarjeta"), ToolTip("No de Tarjeta de debito o credito, cuando es el medio de pago")]
        [Index(7), VisibleInListView(false)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string NoTarjeta
        {
            get => noTarjeta;
            set => SetPropertyValue(nameof(NoTarjeta), ref noTarjeta, value);
        }

        /// <summary>
        ///  No de cheque, No de pago electronico, Id de la remesa, transferencia, no vaucher etc.
        /// </summary>
        [Size(40), DbType("varchar(40)"), XafDisplayName("No Referencia"), Index(8), VisibleInListView(false)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public string Referencia
        {
            get => referencia;
            set => SetPropertyValue(nameof(Referencia), ref referencia, value);
        }

        /// <summary>
        /// La trasaccion de bancos cuando es: transferencia bancaria, remesa a cuenta, pago electronico.
        /// En los tres casos anteriores el movimiento de bancos es previo al registro del pago de la cuenta por cobrar
        /// Cuando son pagos en efectivo o cheques, evaluar si puede ser la remesa del deposito a cuenta de la empresa
        /// </summary>
        /// <remarks>
        /// Agregar validacion para que este dato sea requerido unicamente en los casos de transferencia, remesa o pago electronico
        /// Agregar regla de apariencia, para que en los casos donde no es necesario este dato, aparezca deshabilitado u ocultarlo
        /// </remarks>
        [XafDisplayName("Banco Transacción"), Index(9), VisibleInListView(false),
            ToolTip("La transacción de bancos, cuando es transferencia, remesa, o pago electronico")]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public BancoTransaccion BancoTransaccion
        {
            get => bancoTransaccion;
            set => SetPropertyValue(nameof(BancoTransaccion), ref bancoTransaccion, value);
        }

        [XafDisplayName("Moneda"), Index(10), VisibleInListView(false)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public Moneda Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        
        [PersistentAlias(nameof(factorCambio)), XafDisplayName("Valor Moneda"), VisibleInListView(false)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public decimal FactorCambio => factorCambio;

        [PersistentAlias(nameof(valor)), XafDisplayName("Valor"), Index(10)]
        [DetailViewLayout("Datos Transacción", LayoutGroupType.SimpleEditorsGroup, 1)]
        public decimal ? Valor => valor;


        [DbType("smallint"), XafDisplayName("Estado"), Index(15), RuleRequiredField("CxCTransaccion.Estado_Requerido", "Save")]
        [DetailViewLayout("Otros Datos", LayoutGroupType.SimpleEditorsGroup, 2)]
        public ECxcTransaccionEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario"), Index(16), VisibleInListView(false)]
        [DetailViewLayout("Otros Datos", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [PersistentAlias(nameof(usuarioAnulo)), XafDisplayName("Usuario Anulo"), Index(17), VisibleInListView(false)]
        [DetailViewLayout("Otros Datos", LayoutGroupType.SimpleEditorsGroup, 2)]
        public string UsuarioAnulo => usuarioAnulo;
        
        [PersistentAlias(nameof(fechaAnulacion)), XafDisplayName("Fecha Anulación"), Index(18), VisibleInListView(false)]
        [DetailViewLayout("Otros Datos", LayoutGroupType.SimpleEditorsGroup, 2)]
        public DateTime ? FechaAnulacion => fechaAnulacion;

        #endregion

        #region colecciones
        [Association("CxCTransaccion-Documentos"), DevExpress.Xpo.Aggregated, XafDisplayName("Documentos"), Index(0)]
        
        public XPCollection<CxCDocumento> Documentos
        {
            get
            {
                return GetCollection<CxCDocumento>(nameof(Documentos));
            }
        }
        #endregion
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}