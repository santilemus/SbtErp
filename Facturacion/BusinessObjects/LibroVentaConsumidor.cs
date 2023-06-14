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

namespace SBT.Apps.Iva.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al LIbro de Ventas a Consumidor Final (IVA)
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Libro Ventas Consumidor"), NavigationItem("Contabilidad")]
    [DefaultProperty(nameof(Fecha)), CreatableItem(false), VisibleInReports(true), VisibleInDashboards(true)]
    [Persistent(nameof(LibroVentaConsumidor))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class LibroVentaConsumidor : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public LibroVentaConsumidor(Session session)
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

        int correlativo;
        [Persistent(nameof(Cerrado)), DbType("bit")]
        bool cerrado;
        AutorizacionDocumento autorizacionDocumento;
        decimal ventaTercero;
        decimal ventaZonaFranca;
        decimal exportacionServicio;
        decimal exportacionFueraCA;
        decimal exportacionCA;
        decimal gravadaLocal;
        decimal noSujeta;
        decimal internaExenta;
        decimal exenta;
        string noDocumentoAl;
        string noDocumentoDel;
        string noControlInternoAl;
        string noControlInternoDel;
        string tipoDocumento;
        DateTime fecha;
        Empresa empresa;
        [Persistent(nameof(Oid)), DbType("int"), Key(true)]
        int oid;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public int Oid => oid;

        [XafDisplayName("Empresa"), DbType("int"), VisibleInListView(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("int"), XafDisplayName("Correlativo"), VisibleInListView(false)]
        public int Correlativo
        {
            get => correlativo;
            set => SetPropertyValue(nameof(Correlativo), ref correlativo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha")]
        [Indexed(nameof(AutorizacionDocumento), Name = "idxFechaEmisionAutorizacion_LibroCompra")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        /// <summary>
        /// Para relacionar con AutorizacionDocumento y obtener de alli: ClaseDocumento, NoResolucion, NoSerie, NoCaja
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

        [Size(14), DbType("varchar(14)"), XafDisplayName("Control Interno Del")]
        public string NoControlInternoDel
        {
            get => noControlInternoDel;
            set => SetPropertyValue(nameof(NoControlInternoDel), ref noControlInternoDel, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Control Interno Al")]
        public string NoControlInternoAl
        {
            get => noControlInternoAl;
            set => SetPropertyValue(nameof(NoControlInternoAl), ref noControlInternoAl, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Documento Del")]
        public string NoDocumentoDel
        {
            get => noDocumentoDel;
            set => SetPropertyValue(nameof(NoDocumentoDel), ref noDocumentoDel, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Documento Al")]
        public string NoDocumentoAl
        {
            get => noDocumentoAl;
            set => SetPropertyValue(nameof(NoDocumentoAl), ref noDocumentoAl, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Exenta")]
        public decimal Exenta
        {
            get => exenta;
            set => SetPropertyValue(nameof(Exenta), ref exenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Interna Exenta")]
        [ToolTip("Ventas internas exentas no sujetas a proporcionalidad")]
        public decimal InternaExenta
        {
            get => internaExenta;
            set => SetPropertyValue(nameof(InternaExenta), ref internaExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("No Sujeta")]
        public decimal NoSujeta
        {
            get => noSujeta;
            set => SetPropertyValue(nameof(NoSujeta), ref noSujeta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Gravada Local")]
        public decimal GravadaLocal
        {
            get => gravadaLocal;
            set => SetPropertyValue(nameof(GravadaLocal), ref gravadaLocal, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Exportación CA")]
        public decimal ExportacionCA
        {
            get => exportacionCA;
            set => SetPropertyValue(nameof(ExportacionCA), ref exportacionCA, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Exportación Fuera CA")]
        public decimal ExportacionFueraCA
        {
            get => exportacionFueraCA;
            set => SetPropertyValue(nameof(ExportacionFueraCA), ref exportacionFueraCA, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Exportación Servicio")]
        public decimal ExportacionServicio
        {
            get => exportacionServicio;
            set => SetPropertyValue(nameof(ExportacionServicio), ref exportacionServicio, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Zona Franca")]
        public decimal VentaZonaFranca
        {
            get => ventaZonaFranca;
            set => SetPropertyValue(nameof(VentaZonaFranca), ref ventaZonaFranca, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Tercero No Domiciliado")]
        public decimal VentaTercero
        {
            get => ventaTercero;
            set => SetPropertyValue(nameof(VentaTercero), ref ventaTercero, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Total Venta"),
            PersistentAlias("[Exenta] + [NoSujeta] + [GravadaLocal] + [ExportacionCA] + [ExportacionFueraCA] + [ExportacionServicio] + [VentaZonaFranca] + [VentaTercero]")]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));

        [XafDisplayName("No de Anexo"), PersistentAlias("2")]
        public string NumeroAnexo => Convert.ToString(EvaluateAlias(nameof(NumeroAnexo)));

        [XafDisplayName("Cerrado"), VisibleInListView(false), PersistentAlias(nameof(cerrado))]
        public bool Cerrado => cerrado;


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}