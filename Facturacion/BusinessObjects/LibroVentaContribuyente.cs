using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.CxC.Module.BusinessObjects;

namespace SBT.Apps.Iva.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al libro de Ventas a Contribuyentes (IVA)
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Libro Ventas Contribuyente"), NavigationItem(false)]
    [DefaultProperty(nameof(NoControlInterno))]
    [Persistent(nameof(LibroVentaContribuyente))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class LibroVentaContribuyente : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public LibroVentaContribuyente(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            oid = -1;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Persistent(nameof(ValorMoneda))]
        decimal valorMoneda;
        [Persistent(nameof(Moneda))]
        Moneda moneda;
        decimal ivaRetenido;
        decimal ivaPercibido;
        Tercero.Module.BusinessObjects.Tercero cliente;
        CxCTransaccion cxcDocumento;
        AutorizacionDocumento autorizacionDocumento;
        Venta venta;
        decimal debitoFiscalVtaTercero;
        decimal ventaTercero;
        decimal debitoFiscal;
        decimal gravadaLocal;
        decimal noSujeta;
        decimal exenta;
        string nit;
        string noControlInterno;
        string numero;
        string tipoDocumento;
        DateTime fechaEmision;
        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public long Oid => oid;

        [DbType("datetime"), XafDisplayName("Fecha Emisión")]
        public DateTime FechaEmision
        {
            get => fechaEmision;
            set => SetPropertyValue(nameof(FechaEmision), ref fechaEmision, value);
        }

        /// <summary>
        /// Para relacionar con AutorizacionDocumento y obtener de alli: ClaseDocumento, NoResolucion, NoSerie
        /// </summary>
        [XafDisplayName("Autorización Documento")]
        public AutorizacionDocumento AutorizacionDocumento
        {
            get => autorizacionDocumento;
            set => SetPropertyValue(nameof(AutorizacionDocumento), ref autorizacionDocumento, value);
        }

        [Size(2), DbType("varchar(2)"), XafDisplayName("Tipo Documento")]
        public string TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }

        [Size(8), DbType("varchar(8)"), XafDisplayName("No Documento")]
        public string Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        [Size(8), DbType("varchar(8)"), XafDisplayName("No Control Interno")]
        public string NoControlInterno
        {
            get => noControlInterno;
            set => SetPropertyValue(nameof(NoControlInterno), ref noControlInterno, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Nit")]
        public string Nit
        {
            get => nit;
            set => SetPropertyValue(nameof(Nit), ref nit, value);
        }


        [XafDisplayName("Cliente")]
        public Tercero.Module.BusinessObjects.Tercero Cliente
        {
            get => cliente;
            set => SetPropertyValue(nameof(Cliente), ref cliente, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Exenta")]
        public decimal Exenta
        {
            get => exenta;
            set => SetPropertyValue(nameof(Exenta), ref exenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta No Sujeta")]
        public decimal NoSujeta
        {
            get => noSujeta;
            set => SetPropertyValue(nameof(NoSujeta), ref noSujeta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Gravada Local")]
        public decimal GravadaLocal
        {
            get => gravadaLocal;
            set => SetPropertyValue(nameof(GravadaLocal), ref gravadaLocal, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Débito Fiscal")]
        public decimal DebitoFiscal
        {
            get => debitoFiscal;
            set => SetPropertyValue(nameof(DebitoFiscal), ref debitoFiscal, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Iva Percibido")]
        public decimal IvaPercibido
        {
            get => ivaPercibido;
            set => SetPropertyValue(nameof(IvaPercibido), ref ivaPercibido, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Iva Retenido")]
        public decimal IvaRetenido
        {
            get => ivaRetenido;
            set => SetPropertyValue(nameof(IvaRetenido), ref ivaRetenido, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Tercero No Domiciliado")]
        public decimal VentaTercero
        {
            get => ventaTercero;
            set => SetPropertyValue(nameof(VentaTercero), ref ventaTercero, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Débito Fiscal Vta Tercero")]
        public decimal DebitoFiscalVtaTercero
        {
            get => debitoFiscalVtaTercero;
            set => SetPropertyValue(nameof(DebitoFiscalVtaTercero), ref debitoFiscalVtaTercero, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Total Venta")]
        [PersistentAlias("[Exenta] + [NoSujeta] + [GravadaLocal] + [DebitoFiscal] +[VentaTercero] + [DebitoFiscalTercero]")]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));


        /// <summary>
        /// Es importante para hacer el update cuando ya se inserto el registro de ventas y evitar que se dupliquen
        /// </summary>
        [XafDisplayName("Venta"), Browsable(false)]
        public Venta Venta
        {
            get => venta;
            set => SetPropertyValue(nameof(Venta), ref venta, value);
        }

        /// <summary>
        /// Es importante para hacer el update cuando ya se insertaron registros que corresponden a notas de crédito y
        /// débito; para evitar que se dupliquen.
        /// </summary>
        [XafDisplayName("CxC Transacción"), Browsable(false)]
        public CxCTransaccion CxCDocumento
        {
            get => cxcDocumento;
            set => SetPropertyValue(nameof(CxCDocumento), ref cxcDocumento, value);
        }

        [XafDisplayName("No de Anexo"), PersistentAlias("1")]
        public string NumeroAnexo => Convert.ToString(EvaluateAlias(nameof(NumeroAnexo)));

        [XafDisplayName("Moneda"), PersistentAlias(nameof(moneda))]
        public Moneda Moneda => moneda;

        [PersistentAlias(nameof(valorMoneda)), XafDisplayName("Valor Moneda"), Browsable(false)]
        public decimal ValorMoneda => valorMoneda;

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}