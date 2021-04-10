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
using SBT.Apps.Compra.Module.BusinessObjects;

namespace SBT.Apps.Iva.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al Libro de Compras (Iva)
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Libro Compras"), NavigationItem(false)]
    [DefaultProperty(nameof(Numero))]
    [Persistent(nameof(LibroCompra))]
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
            claseDocumento = "1";    
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal totalCompra;
        decimal creditoFiscal;
        decimal importacionGravadaServicio;
        decimal importacionGravadaBien;
        decimal internacionGravadaBien;
        decimal internaGravada;
        decimal importacionExenta;
        decimal internacionExenta;
        decimal internaExenta;
        string nombreProveedor;
        string nit;
        string numero;
        string tipoDocumento;
        string claseDocumento;
        DateTime fechaEmision;
        [Persistent(nameof(Importación)), DbType("bigint"), Key(true)]
        long oid;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public long Importación => oid;

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

        [Size(8), DbType("varchar(8)"), XafDisplayName("No Documento")]
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

        [Size(200), DbType("varchar(200)"), XafDisplayName("NombreProveedor")]
        public string NombreProveedor
        {
            get => nombreProveedor;
            set => SetPropertyValue(nameof(NombreProveedor), ref nombreProveedor, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Interna Exenta")]
        public decimal InternaExenta
        {
            get => internaExenta;
            set => SetPropertyValue(nameof(InternaExenta), ref internaExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Internación Exenta")]
        public decimal InternacionExenta
        {
            get => internacionExenta;
            set => SetPropertyValue(nameof(InternacionExenta), ref internacionExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Importación Exenta")]
        public decimal ImportacionExenta
        {
            get => importacionExenta;
            set => SetPropertyValue(nameof(ImportacionExenta), ref importacionExenta, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Interna Gravada")]
        public decimal InternaGravada
        {
            get => internaGravada;
            set => SetPropertyValue(nameof(InternaGravada), ref internaGravada, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Internación Gravada Bien")]
        public decimal InternacionGravadaBien
        {
            get => internacionGravadaBien;
            set => SetPropertyValue(nameof(InternacionGravadaBien), ref internacionGravadaBien, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Importación Gravada Bien")]
        public decimal ImportacionGravadaBien
        {
            get => importacionGravadaBien;
            set => SetPropertyValue(nameof(ImportacionGravadaBien), ref importacionGravadaBien, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Importación Gravada Servicio")]
        public decimal ImportacionGravadaServicio
        {
            get => importacionGravadaServicio;
            set => SetPropertyValue(nameof(ImportacionGravadaServicio), ref importacionGravadaServicio, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Crédito Fiscal")]
        public decimal CreditoFiscal
        {
            get => creditoFiscal;
            set => SetPropertyValue(nameof(CreditoFiscal), ref creditoFiscal, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Total Compra")]
        public decimal TotalCompra
        {
            get => totalCompra;
            set => SetPropertyValue(nameof(TotalCompra), ref totalCompra, value);
        }

        [XafDisplayName("No de Anexo"), PersistentAlias("3")]
        public string NumeroAnexo => Convert.ToString(EvaluateAlias(nameof(NumeroAnexo)));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}