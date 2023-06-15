using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.CxC.Module.BusinessObjects;

namespace SBT.Apps.Iva.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al libro de Ventas a Contribuyentes (IVA)
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Libro Ventas Contribuyente"), NavigationItem("Contabilidad")]
    [DefaultProperty(nameof(Numero)), CreatableItem(false), VisibleInReports(true), VisibleInDashboards(true)]
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
            cerrado = false;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        int correlativo;
        [Persistent(nameof(Cerrado)), DbType("bit")]
        bool cerrado;
        string dui;
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

        [DbType("int"), XafDisplayName("Correlativo"), VisibleInListView(false)]
        public int Correlativo
        {
            get => correlativo;
            set => SetPropertyValue(nameof(Correlativo), ref correlativo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime Fecha
        {
            get => fechaEmision;
            set => SetPropertyValue(nameof(Fecha), ref fechaEmision, value);
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
        [ModelDefault("DisplayFormat", "F2")]
        public decimal Exenta
        {
            get => exenta;
            set => SetPropertyValue(nameof(Exenta), ref exenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta No Sujeta")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal NoSujeta
        {
            get => noSujeta;
            set => SetPropertyValue(nameof(NoSujeta), ref noSujeta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Gravada Local")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal GravadaLocal
        {
            get => gravadaLocal;
            set => SetPropertyValue(nameof(GravadaLocal), ref gravadaLocal, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Débito Fiscal")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal DebitoFiscal
        {
            get => debitoFiscal;
            set => SetPropertyValue(nameof(DebitoFiscal), ref debitoFiscal, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Iva Percibido")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal IvaPercibido
        {
            get => ivaPercibido;
            set => SetPropertyValue(nameof(IvaPercibido), ref ivaPercibido, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Iva Retenido")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal IvaRetenido
        {
            get => ivaRetenido;
            set => SetPropertyValue(nameof(IvaRetenido), ref ivaRetenido, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Tercero No Domiciliado")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal VentaTercero
        {
            get => ventaTercero;
            set => SetPropertyValue(nameof(VentaTercero), ref ventaTercero, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Débito Fiscal Vta Tercero")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal DebitoFiscalTercero
        {
            get => debitoFiscalVtaTercero;
            set => SetPropertyValue(nameof(DebitoFiscalTercero), ref debitoFiscalVtaTercero, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Total Venta")]
        [PersistentAlias("[Exenta] + [NoSujeta] + [GravadaLocal] + [DebitoFiscal] +[VentaTercero] + [DebitoFiscalTercero]")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));


        [Size(9), XafDisplayName("Dui")]
        public string Dui
        {
            get => dui;
            set => SetPropertyValue(nameof(Dui), ref dui, value);
        }

        /// <summary>
        /// Es importante para hacer el update cuando ya se inserto el registro de ventas y evitar que se dupliquen
        /// </summary>
        [XafDisplayName("Venta"), VisibleInListView(false)]
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

        [PersistentAlias(nameof(cerrado)), VisibleInListView(false)]
        public bool Cerrado
        {
            get => cerrado;
        }

        [XafDisplayName("Clase"), VisibleInListView(true), VisibleInDetailView(false)]
        public int Clase => (int)AutorizacionDocumento.Clase;

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}