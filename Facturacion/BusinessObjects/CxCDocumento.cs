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
    [ImageName(nameof(CxCDocumento))]
    [RuleCriteria("CxCDocumento.SaldoPendiente > 0 y Estado = Debe", DefaultContexts.Save, "[Venta.Estado] == 0 && [Venta.Saldo] <> 0.0", 
        CustomMessageTemplate = "El estado de la factura debe ser Debe y el Saldo <> 0.0 para aplicarle una transacción de Cuenta por Cobrar")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxCDocumento : XPOBaseDoc
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxCDocumento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            gravada = 0.0m;
            cxCTransaccion = null;
            iva = 0.0m;
            ivaPercibido = 0.0m;
            ivaRetenido = 0.0m;
            noSujeta = 0.0m;
            exenta = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal? valor;
        Venta venta;
        CxCTransaccion cxCTransaccion;
        [Persistent(nameof(Gravada)), DbType("numeric(14,2)")]
        decimal gravada;
        [Persistent(nameof(Iva)), DbType("numeric(14,2)")]
        decimal iva;
        [Persistent(nameof(IvaPercibido)), DbType("numeric(14,2)")]
        decimal ivaPercibido;
        [Persistent(nameof(IvaRetenido)), DbType("numeric(14,2)")]
        decimal ivaRetenido;
        [Persistent(nameof(NoSujeta)), DbType("numeric(14,2)")]
        decimal noSujeta;
        [Persistent(nameof(Exenta)), DbType("numeric(14,2)")]
        decimal exenta;

        [Association("CxCTransaccion-Documentos"), XafDisplayName("Transacción"), Index(0)]
        public CxCTransaccion CxCTransaccion
        {
            get => cxCTransaccion;
            set => SetPropertyValue(nameof(CxCTransaccion), ref cxCTransaccion, value);
        }

        [Association("Venta-CxCDocumentos"), XafDisplayName("Venta"), Index(4), VisibleInLookupListView(true)]
        public Venta Venta
        {
            get => venta;
            set => SetPropertyValue(nameof(Venta), ref venta, value);
        }

        [PersistentAlias(nameof(gravada)), XafDisplayName("Gravado"), Index(9)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Gravada
        {
            get { return gravada; }
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

        [PersistentAlias(nameof(noSujeta)), XafDisplayName("No Sujeta"), VisibleInListView(false), Index(14)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal NoSujeta
        {
            get { return noSujeta; }
        }

        [PersistentAlias(nameof(exenta)), XafDisplayName("Exento"), VisibleInListView(true), Index(15)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Exenta => exenta;

        /// <summary>
        /// Valor de las transacciones que no se separan en exento, gravado, etc. Por ejemplo: Pagos, cheques rechazados, etc
        /// </summary>
        [DbType("numeric(14,2)"), XafDisplayName("Valor")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [PersistentAlias("[SubTotal] - [IvaRetenido] + [IvaPercibido]  + [VentaNoSujeta] + [VentaExenta] ")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [XafDisplayName("Total"), Index(16)]
        public decimal Total
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(Total))); }
        }


        #endregion

        #region Colecciones
        [Association("CxCDocumento-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalle"), Index(0)]
        public XPCollection<CxCDocumentoDetalle> Detalles => GetCollection<CxCDocumentoDetalle>(nameof(Detalles));
        #endregion

        #region Metodos
        public void UpdateTotalExenta(bool forceChangeEvents)
        {
            decimal? oldExenta = exenta;
            exenta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Exenta])"))); ;
            if (forceChangeEvents)
                OnChanged(nameof(Exenta), oldExenta, exenta);
        }

        public void UpdateTotalGravada(bool forceChangeEvents)
        {
            decimal? oldGravada = gravada;
            gravada = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Gravada])")));
            if (forceChangeEvents)
                OnChanged(nameof(Gravada), oldGravada, gravada);
        }

        public void UpdateTotalIva(bool forceChangeEvents)
        {
            decimal? oldIva = iva;
            iva = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Iva])")));
            if (forceChangeEvents)
                OnChanged(nameof(Iva), oldIva, iva);
        }

        public void UpdateTotalNoSujeta(bool forceChangeEvents)
        {
            decimal? oldNoSujeta = noSujeta;
            noSujeta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([NoSujeta])")));
            if (forceChangeEvents)
                OnChanged(nameof(NoSujeta), oldNoSujeta, noSujeta);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}