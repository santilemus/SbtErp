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
            claseDocumento = "1";
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal totalVenta;
        decimal debitoFiscalVtaTercero;
        decimal ventaTercero;
        decimal debitoFiscal;
        decimal ventaGravadaLocal;
        decimal ventaNoSujeta;
        decimal ventaExenta;
        string razonSocial;
        string nit;
        string noControlInterno;
        string numero;
        string noSerie;
        string noResolucion;
        string tipoDocumento;
        string claseDocumento;
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

        [Size(200), DbType("varchar(200)"), XafDisplayName("Razón Social")]
        public string RazonSocial
        {
            get => razonSocial;
            set => SetPropertyValue(nameof(RazonSocial), ref razonSocial, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Venta Exenta")]
        public decimal VentaExenta
        {
            get => ventaExenta;
            set => SetPropertyValue(nameof(VentaExenta), ref ventaExenta, value);
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

        [DbType("numeric(14,2)"), XafDisplayName("Débito Fiscal")]
        public decimal DebitoFiscal
        {
            get => debitoFiscal;
            set => SetPropertyValue(nameof(DebitoFiscal), ref debitoFiscal, value);
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
        public decimal TotalVenta
        {
            get => totalVenta;
            set => SetPropertyValue(nameof(TotalVenta), ref totalVenta, value);
        }

        [XafDisplayName("No de Anexo"), PersistentAlias("1")]
        public string NumeroAnexo => Convert.ToString(EvaluateAlias(nameof(NumeroAnexo)));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}