using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

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
            condicionPago = ECondicionPago.Contado;
            estado = EEstadoFactura.Pagado;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        [Persistent(nameof(Oid)), DbType("bigint"), Key(true)]
        long oid = -1;
        [Persistent(nameof(Saldo))]
        protected decimal saldo;
        [Persistent(nameof(Exenta)), DbType("numeric(14,2)")]
        protected decimal? exenta;
        [Persistent(nameof(NoSujeta)), DbType("numeric(14,2)")]
        protected decimal? noSujeta;
        [Persistent(nameof(IvaRetenido)), DbType("numeric(14,2)")]
        protected decimal? ivaRetenido;
        [Persistent(nameof(IvaPercibido)), DbType("numeric(14,2)")]
        protected decimal? ivaPercibido;
        [Persistent(nameof(Iva)), DbType("numeric(14,2)")]
        protected decimal? iva;
        [Persistent(nameof(Gravada)), DbType("numeric(14,2)")]
        protected decimal? gravada;
        [Persistent(nameof(Estado)), DbType("smallint")]
        EEstadoFactura estado;
        int? diasCredito;
        ECondicionPago condicionPago;
        Listas tipoFactura;

        /// <summary>
        /// Oid del objeto. Es la llave primaria
        /// </summary>
        [PersistentAlias(nameof(oid)), XafDisplayName("Oid"), Index(0)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        public long Oid => oid;

        /// <summary>
        /// Tipo de Factura emitida
        /// </summary>
        [XafDisplayName("Tipo Factura")]
        [DataSourceProperty(nameof(TiposDeFacturas)), Index(9)]
        [RuleRequiredField("", DefaultContexts.Save)]
        [DetailViewLayout("Datos Generales", LayoutGroupType.SimpleEditorsGroup, 0)]
        [VisibleInListView(true), VisibleInLookupListView(true)]
        [ExplicitLoading]
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
        [DbType("smallint")]
        [RuleRequiredField("", "Save", TargetCriteria = "[TipoFactura.Codigo] In ('COVE01', 'COVE02')")]
        [VisibleInListView(false)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
        public ECondicionPago CondicionPago
        {
            get => condicionPago;
            set
            {
                ECondicionPago oldCondicionPago = condicionPago;
                bool changed = SetPropertyValue(nameof(CondicionPago), ref condicionPago, value);
                if (!IsLoading && !IsSaving && changed)
                    DoCondicionPagoChanged(true, oldCondicionPago);
            }
        }

        /// <summary>
        /// Dias de crédito, cuando la condicion de pago es al crédito
        /// </summary>
        [DbType("smallint"), XafDisplayName("Días Crédito"), VisibleInLookupListView(false)]
        [DetailViewLayout("Datos de Pago", LayoutGroupType.SimpleEditorsGroup, 2)]
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
        [VisibleInListView(true)]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
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
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        [VisibleInListView(true)]
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
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        public decimal SubTotal => Convert.ToDecimal(EvaluateAlias(nameof(SubTotal)));

        /// <summary>
        /// Monto del IVA percibido
        /// </summary>
        [PersistentAlias(nameof(ivaPercibido))]
        [XafDisplayName("Iva Percibido"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        public decimal? IvaPercibido => ivaPercibido;

        /// <summary>
        /// Monto del IVA retenido
        /// </summary>
        [PersistentAlias(nameof(ivaRetenido))]
        [XafDisplayName("Iva Retenido"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        public decimal? IvaRetenido => ivaRetenido;

        /// <summary>
        /// Monto no sujeto del documento
        /// </summary>
        [PersistentAlias(nameof(noSujeta))]
        [XafDisplayName("No Sujeta"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
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
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
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
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        [VisibleInListView(true)]
        public decimal Total => Convert.ToDecimal(EvaluateAlias(nameof(Total)));

        /// <summary>
        /// Estado del documento
        /// </summary>
        [PersistentAlias(nameof(estado)), XafDisplayName("Estado"), VisibleInListView(true)]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        [DbType("smallint")]
        public EEstadoFactura Estado => estado;

        /// <summary>
        /// Saldo del documento
        /// </summary>
        [PersistentAlias(nameof(saldo))]
        [XafDisplayName("Saldo Pendiente"), VisibleInListView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 10)]
        public decimal Saldo => saldo;


        protected XPCollection<Listas> fTiposDeFacturas;
        [Browsable(false)]  // evitar que se muestre la colleccion separada
        public XPCollection<Listas> TiposDeFacturas
        {
            get
            {
                if (fTiposDeFacturas == null)
                {
                    fTiposDeFacturas = new XPCollection<Listas>(Session);
                    RefreshTiposDeFacturas();
                }
                return fTiposDeFacturas;
            }

        }
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
        /// Metodo virtual cuando cambia la condicion de pago
        /// </summary>
        /// <param name="forceChangeEvents"></param>
        /// <param name="oldValue"></param>

        protected virtual void DoCondicionPagoChanged(bool forceChangeEvents, ECondicionPago oldValue)
        {
            if (CondicionPago == ECondicionPago.Credito)
            {
                estado = EEstadoFactura.Debe;
                saldo = Total;
            }
            else
            {
                estado = EEstadoFactura.Pagado;
                saldo = 0.0m;
            }
            if (forceChangeEvents)
                OnChanged(nameof(Estado));
        }

        /// <summary>
        /// Metodo virtual cuando cambia el valor gravado del documento
        /// </summary>
        /// <param name="forceChangeEvents"></param>
        /// <param name="oldValue"></param>
        protected virtual void DoGravadaChanged(bool forceChangeEvents, decimal oldValue)
        {

        }

        protected virtual void DoAnular()
        {
            estado = EEstadoFactura.Anulado;
            OnChanged(nameof(Estado));
            Save();
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

        /// <summary>
        /// Metodo para actualizar el saldo de la factura
        /// </summary>
        /// <param name="valor">Valor del nuevo saldo</param>
        /// <param name="forceChangeEvents">Indica si dee invocar eventos para las propiedades afectadas</param>
        public virtual void ActualizarSaldo(decimal valor, EEstadoFactura status, bool forceChangeEvents)
        {
            decimal oldSaldo = Saldo;
            saldo = valor;
            estado = status;
            if (forceChangeEvents)
                OnChanged(nameof(Saldo), oldSaldo, saldo);
        }

        protected virtual void RefreshTiposDeFacturas()
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