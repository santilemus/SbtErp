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

namespace SBT.Apps.Iva.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al LIbro de Ventas a Consumidor Final (IVA)
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Libro Ventas Consumidor"), NavigationItem(false)]
    [DefaultProperty(nameof(FechaEmision))]
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
            claseDocumento = "1";
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal totalVenta;
        decimal ventaTerceroNoDomiciliado;
        decimal ventaZonaFranca;
        decimal exportacionServicio;
        decimal exportacionFueraCA;
        decimal exportacionCA;
        decimal ventaGravadaLocal;
        decimal ventaNoSujeta;
        decimal ventaInternaExenta;
        decimal ventaExenta;
        string noCaja;
        string noDocumentoAl;
        string noDocumentoDel;
        string noControlInternoAl;
        string noControlInternoDel;
        string noSerie;
        string noResolucion;
        string tipoDocumento;
        string claseDocumento;
        DateTime fechaEmision;
        [Persistent(nameof(Oid)), DbType("int"), Key(true)]
        int oid;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public int Oid => oid;

        [DbType("datetime"), XafDisplayName("Fecha Emisión")]
        public DateTime FechaEmision
        {
            get => fechaEmision;
            set => SetPropertyValue(nameof(FechaEmision), ref fechaEmision, value);
        }

        [Size(1), DbType("varchar(1)"), XafDisplayName("Clase Documento")]
        public string ClaseDocumento
        {
            get => claseDocumento;
            set => SetPropertyValue(nameof(ClaseDocumento), ref claseDocumento, value);
        }

        [Size(2), DbType("varchar(2)"), XafDisplayName("Tipo Documento")]
        public string TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }

        [Size(19), DbType("varchar(19)"), XafDisplayName("No Resolución")]
        public string NoResolucion
        {
            get => noResolucion;
            set => SetPropertyValue(nameof(NoResolucion), ref noResolucion, value);
        }

        [Size(8), DbType("varchar(8)"), XafDisplayName("No Serie")]
        public string NoSerie
        {
            get => noSerie;
            set => SetPropertyValue(nameof(NoSerie), ref noSerie, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("No Control Interno Del")]
        public string NoControlInternoDel
        {
            get => noControlInternoDel;
            set => SetPropertyValue(nameof(NoControlInternoDel), ref noControlInternoDel, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("No Control Interno Al")]
        public string NoControlInternoAl
        {
            get => noControlInternoAl;
            set => SetPropertyValue(nameof(NoControlInternoAl), ref noControlInternoAl, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("No Documento Del")]
        public string NoDocumentoDel
        {
            get => noDocumentoDel;
            set => SetPropertyValue(nameof(NoDocumentoDel), ref noDocumentoDel, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("No Documento Al")]
        public string NoDocumentoAl
        {
            get => noDocumentoAl;
            set => SetPropertyValue(nameof(NoDocumentoAl), ref noDocumentoAl, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("No Caja")]
        [ToolTip("No de máquina registradora")]
        public string NoCaja
        {
            get => noCaja;
            set => SetPropertyValue(nameof(NoCaja), ref noCaja, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Exenta")]
        public decimal VentaExenta
        {
            get => ventaExenta;
            set => SetPropertyValue(nameof(VentaExenta), ref ventaExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Interna Exenta")]
        [ToolTip("Ventas internas exentas no sujetas a proporcionalidad")]
        public decimal VentaInternaExenta
        {
            get => ventaInternaExenta;
            set => SetPropertyValue(nameof(VentaInternaExenta), ref ventaInternaExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta No Sujeta")]
        public decimal VentaNoSujeta
        {
            get => ventaNoSujeta;
            set => SetPropertyValue(nameof(VentaNoSujeta), ref ventaNoSujeta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Gravada Local")]
        public decimal VentaGravadaLocal
        {
            get => ventaGravadaLocal;
            set => SetPropertyValue(nameof(VentaGravadaLocal), ref ventaGravadaLocal, value);
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

        [DbType("numeric(14,2)"), XafDisplayName("Venta Zona Franca")]
        public decimal VentaZonaFranca
        {
            get => ventaZonaFranca;
            set => SetPropertyValue(nameof(VentaZonaFranca), ref ventaZonaFranca, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Tercero No Domiciliado")]
        public decimal VentaTerceroNoDomiciliado
        {
            get => ventaTerceroNoDomiciliado;
            set => SetPropertyValue(nameof(VentaTerceroNoDomiciliado), ref ventaTerceroNoDomiciliado, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Total Venta")]
        public decimal TotalVenta
        {
            get => totalVenta;
            set => SetPropertyValue(nameof(TotalVenta), ref totalVenta, value);
        }

        [XafDisplayName("No de Anexo"), PersistentAlias("2")]
        public string NumeroAnexo => Convert.ToString(EvaluateAlias(nameof(NumeroAnexo)));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}