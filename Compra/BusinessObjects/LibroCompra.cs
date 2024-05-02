using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Iva.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al Libro de Compras (Iva)
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <cambios>
    /// 29/febrero/2024 por SELM
    /// 700-DGII-GTR-2024-0001. Líneamientos que entran en vigencia para declaraciones de IVA y pago a cuenta de febrero del 2024.
    /// </cambios>
    [DefaultClassOptions, ModelDefault("Caption", "Libro Compras"), NavigationItem("Contabilidad")]
    [DefaultProperty(nameof(Numero))]
    [Persistent(nameof(LibroCompra))]
    [CreatableItem(false), VisibleInReports(true), VisibleInDashboards(true)]
    [FriendlyKeyProperty(nameof(Numero))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class LibroCompra : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public LibroCompra(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            oid = -1;
            claseDocumento = EClaseDocumento.Imprenta;
            cerrado = false;
            tipoOperacion = ETipoOperacionCompra.Gravada;
            clasificacionRenta = EClasificacionRenta.Costo;
            sector = ESectorSujetoPasivo.Servicio;
            tipoCostoGasto = ETipoCostoGasto.GastoVenta;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades


        decimal ivaRetenido;
        decimal ivaPercibido;
        decimal cotrans;
        decimal fovial;
        string dui;
        [Persistent(nameof(Cerrado)), DbType("bit")]
        bool cerrado;
        decimal compraExcluido;
        Tercero.Module.BusinessObjects.Tercero proveedor;
        CompraFactura compraFactura;
        decimal creditoFiscal;
        decimal importacionGravadaServicio;
        decimal importacionGravadaBien;
        decimal internacionGravadaBien;
        decimal internaGravada;
        decimal importacionExenta;
        decimal internacionExenta;
        decimal internaExenta;
        string nit;
        string numero;
        string tipoDocumento;
        EClaseDocumento claseDocumento;
        DateTime fecha;
        int correlativo;
        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid;
        private string serie;
        [Persistent(nameof(TipoOperacion)), FetchOnly]
        private ETipoOperacionCompra tipoOperacion;
        [Persistent(nameof(ClasificacionRenta)), FetchOnly]
        private EClasificacionRenta clasificacionRenta;
        [Persistent(nameof(Sector)), FetchOnly]
        private ESectorSujetoPasivo sector;
        [Persistent(nameof(TipoCostoGasto)), FetchOnly]
        private ETipoCostoGasto tipoCostoGasto;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public long Oid => oid;

        [DbType("int"), XafDisplayName("Correlativo"), VisibleInListView(false), VisibleInDetailView(false)]
        public int Correlativo
        {
            get => correlativo;
            set => SetPropertyValue(nameof(Correlativo), ref correlativo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Emisión")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DbType("smallint"), XafDisplayName("Clase Documento"), VisibleInListView(false)]
        public EClaseDocumento ClaseDocumento
        {
            get => claseDocumento;
            set => SetPropertyValue(nameof(ClaseDocumento), ref claseDocumento, value);
        }

        public int Clase => (int)ClaseDocumento;

        [Size(2), DbType("varchar(2)"), XafDisplayName("Tipo Documento")]
        public string TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }

        [Size(100), DbType("varchar(100)"), XafDisplayName("No Documento")]
        public string Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Nit")]
        public string Nit
        {
            get => nit;
            set => SetPropertyValue(nameof(Nit), ref nit, value);
        }

        [XafDisplayName("Proveedor")]
        public Tercero.Module.BusinessObjects.Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Interna Exenta"), ModelDefault("DisplayFormat", "F2")]
        public decimal InternaExenta
        {
            get => internaExenta;
            set => SetPropertyValue(nameof(InternaExenta), ref internaExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Internación Exenta"), ModelDefault("DisplayFormat", "F2")]
        public decimal InternacionExenta
        {
            get => internacionExenta;
            set => SetPropertyValue(nameof(InternacionExenta), ref internacionExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Importación Exenta"), ModelDefault("DisplayFormat", "F2")]
        public decimal ImportacionExenta
        {
            get => importacionExenta;
            set => SetPropertyValue(nameof(ImportacionExenta), ref importacionExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Interna Gravada"), ModelDefault("DisplayFormat", "F2")]
        public decimal InternaGravada
        {
            get => internaGravada;
            set => SetPropertyValue(nameof(InternaGravada), ref internaGravada, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Internación Gravada Bien"), ModelDefault("DisplayFormat", "F2")]
        public decimal InternacionGravadaBien
        {
            get => internacionGravadaBien;
            set => SetPropertyValue(nameof(InternacionGravadaBien), ref internacionGravadaBien, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Importación Gravada Bien"), ModelDefault("DisplayFormat", "F2")]
        public decimal ImportacionGravadaBien
        {
            get => importacionGravadaBien;
            set => SetPropertyValue(nameof(ImportacionGravadaBien), ref importacionGravadaBien, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Importación Gravada Servicio"), ModelDefault("DisplayFormat", "F2")]
        public decimal ImportacionGravadaServicio
        {
            get => importacionGravadaServicio;
            set => SetPropertyValue(nameof(ImportacionGravadaServicio), ref importacionGravadaServicio, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Crédito Fiscal"), ModelDefault("DisplayFormat", "F2")]
        public decimal CreditoFiscal
        {
            get => creditoFiscal;
            set => SetPropertyValue(nameof(CreditoFiscal), ref creditoFiscal, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Total Compra")]
        [PersistentAlias(@"[InternaExenta] + [InternacionExenta] + [ImportacionExenta] + [InternaGravada] +[InternacionGravadaBien] + 
                           [ImportacionGravadaBien] + [ImportacionGravadaServicio] + [CreditoFiscal] + [CompraExcluido]")]
        [ModelDefault("DisplayFormat", "F2")]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));


        [DbType("numeric(14,2)"), XafDisplayName("Compra Sujeto Excluido")]
        [ToolTip("Compras a sujetos excluidos"), ModelDefault("DisplayFormat", "F2")]
        public decimal CompraExcluido
        {
            get => compraExcluido;
            set => SetPropertyValue(nameof(CompraExcluido), ref compraExcluido, value);
        }

        [XafDisplayName("Compra Factura")]
        public CompraFactura CompraFactura
        {
            get => compraFactura;
            set => SetPropertyValue(nameof(CompraFactura), ref compraFactura, value);
        }


        [Size(9), DbType("varchar(9)"), XafDisplayName("Dui")]
        public string Dui
        {
            get => dui;
            set => SetPropertyValue(nameof(Dui), ref dui, value);
        }

        [DbType("numeric(14,2)"), VisibleInListView(false), XafDisplayName("Fovial")]
        public decimal Fovial
        {
            get => fovial;
            set => SetPropertyValue(nameof(Fovial), ref fovial, value);
        }

        [DbType("numeric(14,2)"), VisibleInListView(false), XafDisplayName("Cotrans")]
        public decimal Cotrans
        {
            get => cotrans;
            set => SetPropertyValue(nameof(Cotrans), ref cotrans, value);
        }

        [DbType("numeric(14,2)"), VisibleInListView(false), XafDisplayName("Iva Percbido")]
        public decimal IvaPercibido
        {
            get => ivaPercibido;
            set => SetPropertyValue(nameof(IvaPercibido), ref ivaPercibido, value);
        }

        [DbType("numeric(14,2)"), VisibleInListView(false), XafDisplayName("Iva Retenido")]
        public decimal IvaRetenido
        {
            get => ivaRetenido;
            set => SetPropertyValue(nameof(IvaRetenido), ref ivaRetenido, value);
        }

        [VisibleInListView(false), PersistentAlias(nameof(cerrado))]
        public bool Cerrado
        {
            get => cerrado;
        }

        [XafDisplayName("No de Anexo"), PersistentAlias("3")]
        public string NumeroAnexo => Convert.ToString(EvaluateAlias(nameof(NumeroAnexo)));

        [Size(100), VisibleInListView(false), DbType("varchar(100)")]
        public string Serie
        {
            get => serie;
            set => SetPropertyValue(nameof(Serie), ref serie, value);
        }

        [XafDisplayName("Mes"), VisibleInListView(false), VisibleInDetailView(false)]
        public string Mes => string.Format("{0:MM-yyyy}", Fecha);

        [VisibleInListView(false), XafDisplayName("Anexo Percepción")]
        public string AnexoPercepcion => "8";

        [System.ComponentModel.DisplayName("Tipo Operación")]
        [PersistentAlias(nameof(tipoOperacion))]
        public ETipoOperacionCompra TipoOperacion => tipoOperacion;

        [PersistentAlias(nameof(clasificacionRenta))]
        [System.ComponentModel.DisplayName("Clasificación Renta")]
        public EClasificacionRenta ClasificacionRenta => clasificacionRenta;

        [PersistentAlias(nameof(sector))]
        [System.ComponentModel.DisplayName("Sector")]
        public ESectorSujetoPasivo Sector => sector;

        [PersistentAlias(nameof(tipoCostoGasto))]
        [System.ComponentModel.DisplayName("Tipo Costo o Gasto")]
        public ETipoCostoGasto TipoCostoGasto => tipoCostoGasto;

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}