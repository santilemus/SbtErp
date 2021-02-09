using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.ExpressApp;
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
    /// Cuenta por Cobrar
    /// BO que corresponde al encabezado de los documentos de la cuenta por cobrar (algunos no tienen detalle como los pagos).
    /// En todo caso es el detalle de BO CxcTransaccion
    /// </summary>

    [DefaultClassOptions, ModelDefault(@"Caption", @"CxC Documento"), NavigationItem(false), CreatableItem(false), 
        Persistent(nameof(CxCDocumento)), DefaultProperty(nameof(Numero))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxCDocumento : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxCDocumento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            numero = null;
            autorizacionCorrelativo = null;
            ventaGravada = 0.0m;
            cxCTransaccion = null;
            iva = 0.0m;
            ivaPercibido = 0.0m;
            ivaRetenido = 0.0m;
            ventaNoSujeta = 0.0m;
            ventaExenta = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Venta venta;
        [Persistent(nameof(Numero)), DbType("int")]
        int? numero;
        CxCTransaccion cxCTransaccion;
        [Persistent(nameof(AutorizacionCorrelativo))]
        AutorizacionDocumento autorizacionCorrelativo;
        [Persistent(nameof(VentaGravada)), DbType("numeric(14,2)")]
        decimal ventaGravada;
        [Persistent(nameof(Iva)), DbType("numeric(14,2)")]
        decimal iva;
        [Persistent(nameof(IvaPercibido)), DbType("numeric(14,2)")]
        decimal ivaPercibido;
        [Persistent(nameof(IvaRetenido)), DbType("numeric(14,2)")]
        decimal ivaRetenido;
        [Persistent(nameof(VentaNoSujeta)), DbType("numeric(14,2)")]
        decimal ventaNoSujeta;
        [Persistent(nameof(VentaExenta)), DbType("numeric(14,2)")]
        decimal ventaExenta;

        [Association("CxCTransaccion-Documentos"), XafDisplayName("Transacción"), Index(0)]
        public CxCTransaccion CxCTransaccion
        {
            get => cxCTransaccion;
            set => SetPropertyValue(nameof(CxCTransaccion), ref cxCTransaccion, value);
        }

        /// <summary>
        /// Algunos tipos de concepto van a requerir una autorizacion de correlativos, habra que identificar en el BO
        /// CxCTransaccion el tipo de documento o vincularlo al concepto, para que el sistema pueda obener la autorizacion y validar
        /// que sea la correcta para el tipo de documento, ademas este vigente y el correlativo generado este dentro del 
        /// rango
        /// </summary>
        [XafDisplayName("Autorización Correlativo"), PersistentAlias(nameof(autorizacionCorrelativo)), Index(2)]
        [VisibleInListView(false)]
        public AutorizacionDocumento AutorizacionCorrelativo => autorizacionCorrelativo;

        /// <summary>
        /// No de documento o correlativo. Si requiere autorizacion debe estar en el rango de AutorizacionCorrelativo
        /// </summary>
        [PersistentAlias(nameof(numero)), XafDisplayName("Número Documento"), Index(3)]
        public int? Numero => numero;
     
        [Association("Venta-CxCDocumentos"), XafDisplayName("Venta"), Index(4), VisibleInLookupListView(true)]
        public Venta Venta
        {
            get => venta;
            set => SetPropertyValue(nameof(Venta), ref venta, value);
        }

        [PersistentAlias(nameof(ventaGravada)), XafDisplayName("Gravado"), Index(9)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal VentaGravada
        {
            get { return ventaGravada; }
        }

        [PersistentAlias(nameof(iva)), XafDisplayName("IVA"), Index(10)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Iva
        {
            get { return iva; }
        }

        [PersistentAlias("[VentaGravada] + [IVA]")]
        [XafDisplayName("SubTotal"), Index(11)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SubTotal
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(SubTotal))); }
        }

        [PersistentAlias(nameof(ivaRetenido)), XafDisplayName("(-) Iva Retenido"), VisibleInListView(false), Index(12)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal IvaRetenido
        {
            get { return ivaRetenido; }
        }

        [PersistentAlias(nameof(ivaPercibido)), XafDisplayName("(+) Iva Percibido"), VisibleInListView(false), Index(13)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal IvaPercibido
        {
            get { return ivaPercibido; }
        }

        [PersistentAlias(nameof(ventaNoSujeta)), XafDisplayName("No Sujeta"), VisibleInListView(false), Index(14)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal VentaNoSujeta
        {
            get { return ventaNoSujeta; }
        }

        [PersistentAlias(nameof(ventaExenta)), XafDisplayName("Exento"), VisibleInListView(true), Index(15)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal VentaExenta => ventaExenta;

        [PersistentAlias("[SubTotal] - [IvaRetenido] + [IvaPercibido]  + [VentaNoSujeta] + [VentaExenta] ")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [XafDisplayName("Total"), Index(16)]
        public decimal Total
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(Total))); }
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}