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
    /// Cuenta por Cobrar
    /// BO que correspnde al encabezado de los documentos de la cuenta por cobrar (algunos no tienen detalle como los pagos).
    /// En todo caso es el detalle de BO CxcTransaccion
    /// </summary>

    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
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
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Venta venta;
        [Persistent(nameof(Numero)), DbType("int")]
        int numero = 0;
        CxCTransaccion cxCTransaccion;
        [Persistent(nameof(AutorizacionCorrelativo))]
        AutorizacionDocumento autorizacionCorrelativo = null;
        [Persistent(nameof(VentaGravada)), DbType("numeric(14,2)")]
        decimal ventaGravada = 0.0m;
        [Persistent(nameof(IVA)), DbType("numeric(14,2)")]
        decimal iVA = 0.0m;
        [Persistent(nameof(IvaPercibido)), DbType("numeric(14,2)")]
        decimal ivaPercibido = 0.0m;
        [Persistent(nameof(IvaRetenido)), DbType("numeric(14,2)")]
        decimal ivaRetenido = 0.0m;
        [Persistent(nameof(VentaNoSujeta)), DbType("numeric(14,2)")]
        decimal ventaNoSujeta = 0.0m;
        [Persistent(nameof(VentaExenta)), DbType("numeric(14,2)")]
        decimal ventaExenta = 0.0m;

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
        public int Numero => numero;

        
        [Association("Venta-CxCDocumentos"), XafDisplayName("Venta"), Index(4)]
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

        [PersistentAlias(nameof(iVA)), XafDisplayName("IVA"), Index(10)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal IVA
        {
            get { return iVA; }
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
}