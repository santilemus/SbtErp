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

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO no persistente, es el ancestro para los BO que corresponden a documentos de compra y venta. Aplica además para las ordenes de compra
    /// </summary>
    /// <remarks>
    /// Mas informacion para calcular las propiedades a partir del detalle en
    /// https://docs.devexpress.com/eXpressAppFramework/113179/task-based-help/business-model-design/express-persistent-objects-xpo/how-to-calculate-a-property-value-based-on-values-from-a-detail-collection
    /// </remarks>
    [NonPersistent]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class XPCustomFacturaBO : XPCustomBaseDoc
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public XPCustomFacturaBO(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            saldo = 0.0m;
            ivaRetenido = 0.0m;
            ivaPercibido = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid = -1;
        [Persistent(nameof(Saldo))]
        decimal saldo;
        [Persistent(nameof(Exenta)), DbType("numeric(14,2)")]
        protected decimal? exenta;
        [Persistent(nameof(NoSujeta)), DbType("numeric(14,2)")]
        protected decimal? noSujeta;
        [Persistent(nameof(IvaRetenido)), DbType("numeric(14,2)")]
        decimal? ivaRetenido;
        [Persistent(nameof(IvaPercibido)), DbType("numeric(14,2)")]
        decimal? ivaPercibido;
        [Persistent(nameof(Iva)), DbType("numeric(14,2)")]
        protected decimal? iva;
        [Persistent(nameof(Gravada)), DbType("numeric(14,2)")]
        protected decimal? gravada;
        [Persistent(nameof(Estado)), DbType("smallint")]
        EEstadoFactura estado = EEstadoFactura.Debe;
        int? diasCredito;
        Listas condicionPago;
        Listas tipoFactura;

        /// <summary>
        /// Oid del objeto. Es la llave primaria
        /// </summary>
        [PersistentAlias(nameof(oid)), XafDisplayName("Oid"), Index(0)]
        public long Oid => oid;

        /// <summary>
        /// Tipo de Factura emitida
        /// </summary>
        [XafDisplayName("Tipo Factura")]
        [DataSourceCriteria("[Categoria] == 15 And [Activo] == True"), VisibleInLookupListView(true), Index(9)]
        [RuleRequiredField("", DefaultContexts.Save)]
        public Listas TipoFactura
        {
            get => tipoFactura;
            set
            {
                Listas oldTipoFactura = tipoFactura;
                bool changed = SetPropertyValue(nameof(TipoFactura), ref tipoFactura, value);
                if (!IsLoading && !IsSaving && changed)
                    DoTipoFacturaChanged(true, oldTipoFactura);
            }
        }

        /// <summary>
        /// Condicion de Pago. Es obligatorio cuando se trata de Credito Fiscal o Factura Consumidor Final
        /// </summary>
        [XafDisplayName("Condición Pago")]
        [DataSourceCriteria("[Categoria] == 17 And [Activo] == True")]   // Categoria = 17 es condicion de pago
        [RuleRequiredField("", "Save", TargetCriteria = "@This.Tipo.Codigo In ('COVE01', 'COVE02')")]
        [VisibleInListView(false)]
        public Listas CondicionPago
        {
            get => condicionPago;
            set => SetPropertyValue(nameof(CondicionPago), ref condicionPago, value);
        }

        /// <summary>
        /// Dias de crédito, cuando la condicion de pago es al crédito
        /// </summary>
        [DbType("smallint"), XafDisplayName("Días Crédito"), VisibleInLookupListView(false)]
        public int? DiasCredito
        {
            get => diasCredito;
            set => SetPropertyValue(nameof(DiasCredito), ref diasCredito, value);
        }

        /// <summary>
        /// Monto gravado del documento
        /// </summary>
        [PersistentAlias(nameof(gravada))]
        [XafDisplayName("Gravada")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? Gravada
        {
            get
            {

                if (!IsLoading && !IsSaving && gravada == null)
                    UpdateTotalGravada(false);
                return gravada;
            }
        }

        /// <summary>
        /// Monto del iva del documento
        /// </summary>
        [PersistentAlias(nameof(iva))]
        [XafDisplayName("IVA"), Index(22)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? Iva
        {
            get
            {
                if (!IsLoading && !IsSaving && iva == null)
                    UpdateTotalIva(false);
                return iva;
            }
        }

        /// <summary>
        /// SubTotal gravado + iva del documento
        /// </summary>
        [PersistentAlias("[Gravada] + [Iva]")]
        [XafDisplayName("SubTotal"), Index(23)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SubTotal => Convert.ToDecimal(EvaluateAlias(nameof(SubTotal)));

        /// <summary>
        /// Monto del IVA percibido
        /// </summary>
        [PersistentAlias(nameof(ivaPercibido))]
        [XafDisplayName("Iva Percibido"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? IvaPercibido => ivaPercibido;

        /// <summary>
        /// Monto del IVA retenido
        /// </summary>
        [PersistentAlias(nameof(ivaRetenido))]
        [XafDisplayName("Iva Retenido"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? IvaRetenido => ivaRetenido;

        /// <summary>
        /// Monto no sujeto del documento
        /// </summary>
        [PersistentAlias(nameof(noSujeta))]
        [XafDisplayName("No Sujeta"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? NoSujeta
        {
            get
            {
                if (!IsLoading && !IsSaving && noSujeta == null)
                    UpdateTotalNoSujeta(false);
                return noSujeta;
            }
        }

        /// <summary>
        /// Monto exento del documento
        /// </summary>
        [PersistentAlias(nameof(exenta))]
        [XafDisplayName("Exenta"), VisibleInListView(true)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal? Exenta
        {
            get
            {
                if (!IsLoading && !IsSaving && exenta == null)
                    UpdateTotalExenta(false);
                return exenta;
            }
        }

        /// <summary>
        /// Total del documento
        /// </summary>
        [PersistentAlias("[SubTotal] + [IvaPercibido] - [IvaRetenido] + [NoSujeta] + [Exenta] ")]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [XafDisplayName("Total"), Index(28)]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));

        /// <summary>
        /// Estado del documento
        /// </summary>
        [PersistentAlias(nameof(estado)), XafDisplayName("Estado"), VisibleInListView(false)]
        public EEstadoFactura Estado => estado;

        /// <summary>
        /// Saldo del documento
        /// </summary>
        [PersistentAlias(nameof(saldo))]
        [XafDisplayName("Saldo Pendiente"), VisibleInListView(false), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        public decimal Saldo => saldo;

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo virtual que se ejecuta cuando cambia el tipo de factura
        /// </summary>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        /// <param name="oldValue">El valor de la propiedad TipoFactura antes del setter</param>
        protected virtual void DoTipoFacturaChanged(bool forceChangeEvents, Listas oldValue)
        {

        }


        /// <summary>
        /// Metodo virtual para actualizar el valor propiedad Exenta a partir de la suma de los valores de la columna equivalente en el detalle
        /// </summary>
        /// <remarks>Reescribir en las clases heredadas</remarks>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        public virtual void UpdateTotalExenta(bool forceChangeEvents)
        {

        }

        /// <summary>
        /// Metodo virtual para actualizar el valor propiedad Gravada a partir de la suma de los valores de la columna equivalente en el detalle
        /// </summary>
        /// <remarks>Reescribir en las clases heredadas</remarks>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        public virtual void UpdateTotalGravada(bool forceChangeEvents)
        {

        }

        /// <summary>
        /// Metodo virtual para actualizar el valor propiedad Iva a partir de la suma de los valores de la columna equivalente en el detalle
        /// </summary>
        /// <remarks>Reescribir en las clases heredadas</remarks>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        public virtual void UpdateTotalIva(bool forceChangeEvents)
        {

        }

        /// <summary>
        /// Metodo virtual para actualizar el valor propiedad NoSujeta a partir de la suma de los valores de la columna equivalente en el detalle
        /// </summary>
        /// <remarks>Reescribir en las clases heredadas</remarks>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        public virtual void UpdateTotalNoSujeta(bool forceChangeEvents)
        {

        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}