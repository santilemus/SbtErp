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

namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// Cuenta por Cobrar.
    /// BO que corresponde a las transacciones de cuentas por cobrar. Es el encabezado y es generico para cualquier tipo
    /// de documento. Pueden ser notas de credito, debito, abonos, cheques rechazados. En resumen cualquiera para el cual
    /// existe una tipifiacion o concepto
    /// </summary>
    /// <remarks>
    /// 1. Faltan los siguientes properties: no de cheque, vendedor o cobrador (ver si aplica), fecha del cheque,
    ///                                      banco del cheque, moneda, 
    ///                                      numero de movimiento de bancos (si se va a tener interface a ese modulo) por
    ///                                      los pagos con transferencias, remesas o pagos electronicos, numero de pago
    ///                                      electronico. (evaluar que otros harian falta)
    ///                                  
    /// </remarks>
    
    [DefaultClassOptions, ModelDefault("Caption", "CxC Transacción"), NavigationItem("Cuenta por Cobrar")]
    [CreatableItem(false), Persistent("CxCTransaccion"), DefaultProperty("Numero")]
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

        SBT.Apps.Empleado.Module.BusinessObjects.Empleado gestorCobro;
        string noTarjeta;
        string referencia;
        ECxcTransaccionEstado estado;
        Banco banco;
        string comentario;
        [Persistent(nameof(NRC))]
        TerceroDocumento nRC;
        SBT.Apps.Tercero.Module.BusinessObjects.Tercero cliente;
        [Persistent(nameof(Numero)), DbType("int")]
        int numero = 0;
        [Persistent(nameof(FechaAnulacion)), DbType("datetime2")]
        DateTime? fechaAnulacion;
        [Persistent(nameof(UsuarioAnulo)), DbType("varchar(25)"), Size(25)]
        string usuarioAnulo;
        Venta venta;
        DateTime fecha;
        Concepto concepto;
        Listas tipo;


        [XafDisplayName("Cliente"), RuleRequiredField("CxCTransaccion.Cliente_Requerido", DefaultContexts.Save), Index(0)]
        public SBT.Apps.Tercero.Module.BusinessObjects.Tercero Cliente
        {
            get => cliente;
            set => SetPropertyValue(nameof(Cliente), ref cliente, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha"), Index(1)]
        [RuleValueComparison("CxCTransaccion.Fecha > Fecha Factura", DefaultContexts.Save,
            ValueComparisonType.GreaterThanOrEqual, "[Venta.Fecha]", ParametersMode.Expression, SkipNullOrEmptyValues = false)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        /// <summary>
        /// Tipo de concepto o de transaccion de cuenta por cobrar
        /// </summary>
        [Association("Concepto-CxCTransacciones"), XafDisplayName("Tipo Concepto")]
        [RuleRequiredField("CxcTransaccion.Concepto_Requerido", DefaultContexts.Save)]
        [Index(2), VisibleInLookupListView(true)]
        public Concepto Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [XafDisplayName("Gestor de Cobro"), Index(3)]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado GestorCobro
        {
            get => gestorCobro;
            set => SetPropertyValue(nameof(GestorCobro), ref gestorCobro, value);
        }

        /// <summary>
        /// Solo aplica para los conceptos que requieren de una autorizacion de correlativos
        /// </summary>
        [XafDisplayName("Tipo"), RuleRequiredField("Venta.Tipo_Requerido", DefaultContexts.Save)]
        [DataSourceCriteria("[Categoria] == 16 And [Activo] == True"), VisibleInLookupListView(true), Index(4)]
        public Listas Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [PersistentAlias(nameof(nRC)), XafDisplayName("NRC"), Index(5), VisibleInListView(false)]
        public TerceroDocumento NRC => nRC;

        /// <summary>
        /// Numero de documento por Concepto (revisar si se maneja una agrupacion de menor nivel, asi podremos tener
        /// notas de credito==> descuento, devolucion. Pagos ==> Efectivo, Cheque, Transferencia, etc
        /// </summary>
        [PersistentAlias(nameof(numero)), XafDisplayName("Número"), Index(6)]
        public int Numero => numero;

        [XafDisplayName("Banco"), Index(6)]
        public Banco Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }
        
        [Size(25), DbType("varchar(25)"), XafDisplayName("No Tarjeta"), ToolTip("No de Tarjeta de debito o credito, cuando es el medio de pago")]
        [Index(7)]
        public string NoTarjeta
        {
            get => noTarjeta;
            set => SetPropertyValue(nameof(NoTarjeta), ref noTarjeta, value);
        }

        /// <summary>
        ///  No de cheque, No de pago electronico, Id de la remesa, transferencia, no vaucher etc.
        /// </summary>
        [Size(40), DbType("varchar(40)"), XafDisplayName("No Referencia"), Index(8)]
        public string Referencia
        {
            get => referencia;
            set => SetPropertyValue(nameof(Referencia), ref referencia, value);
        }


        [DbType("smallint"), XafDisplayName("Estado"), Index(15), RuleRequiredField("CxCTransaccion.Estado_Requerido", "Save")]
        public ECxcTransaccionEstado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario"), Index(16)]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }

        [PersistentAlias(nameof(usuarioAnulo)), XafDisplayName("Usuario Anulo"), Index(17)]
        public string UsuarioAnulo => usuarioAnulo;
        
        [PersistentAlias(nameof(fechaAnulacion)), XafDisplayName("Fecha Anulación"), Index(18)]
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

    public enum ECxcTransaccionEstado
    {
        Digitado = 0,
        Aplicado = 1,
        Anulado = 2
    }
}