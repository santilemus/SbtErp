﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using System;
using System.ComponentModel;


namespace SBT.Apps.CxC.Module.BusinessObjects
{
    /// <summary>
    /// Cuenta por Cobrar
    /// BO que corresponde al encabezado de los documentos de la cuenta por cobrar (algunos no tienen detalle como los pagos).
    /// Hereda del BO CxCTransaccion
    /// </summary>

    [DefaultClassOptions, ModelDefault(@"Caption", @"CxC Documento"), NavigationItem(false), CreatableItem(false),
        DefaultProperty(nameof(Numero)), VisibleInReports(true), VisibleInDashboards(true)]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [ImageName(nameof(CxCDocumento))]
    [RuleCriteria("CxCDocumento.SaldoPendiente > 0 y Estado = Debe", DefaultContexts.Save, "[Venta.Estado] == 0 && [Venta.Saldo] <> 0.0",
        CustomMessageTemplate = "El estado de la factura debe ser Debe y el Saldo <> 0.0 para aplicarle una transacción de Cuenta por Cobrar")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxCDocumento : CxCTransaccion
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxCDocumento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            gravada = 0.0m;
            iva = 0.0m;
            ivaPercibido = 0.0m;
            ivaRetenido = 0.0m;
            noSujeta = 0.0m;
            exenta = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

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

        [PersistentAlias(nameof(gravada)), XafDisplayName("Gravada"), Index(9)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 2)]
        public decimal Gravada
        {
            get { return gravada; }
        }

        [PersistentAlias(nameof(iva)), XafDisplayName("IVA"), Index(10)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 2)]
        public decimal Iva
        {
            get { return iva; }
        }

        [PersistentAlias("[Gravada] + [Iva]")]
        [XafDisplayName("SubTotal"), Index(11)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 2)]
        public decimal SubTotal
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(SubTotal))); }
        }

        [PersistentAlias(nameof(ivaRetenido)), XafDisplayName("(-) Iva Retenido"), VisibleInListView(false), Index(12)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 2)]
        public decimal IvaRetenido
        {
            get { return ivaRetenido; }
        }

        [PersistentAlias(nameof(ivaPercibido)), XafDisplayName("(+) Iva Percibido"), VisibleInListView(false), Index(13)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 2)]
        public decimal IvaPercibido
        {
            get { return ivaPercibido; }
        }

        [PersistentAlias(nameof(exenta)), XafDisplayName("Exenta"), VisibleInListView(true), Index(14)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 2)]
        public decimal Exenta => exenta;

        [PersistentAlias(nameof(noSujeta)), XafDisplayName("No Sujeta"), VisibleInListView(false), Index(15)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [DetailViewLayout("Totales", LayoutGroupType.SimpleEditorsGroup, 2)]
        public decimal NoSujeta
        {
            get { return noSujeta; }
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
            exenta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Exenta])")));
            monto = SubTotal - IvaRetenido + IvaPercibido + NoSujeta + Exenta;
            if (forceChangeEvents)
            {
                OnChanged(nameof(Exenta), oldExenta, exenta);
                OnChanged(nameof(Monto));
            }
        }

        public void UpdateTotalGravada(bool forceChangeEvents)
        {
            decimal? oldGravada = gravada;
            gravada = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Gravada])")));
            monto = SubTotal - IvaRetenido + IvaPercibido + NoSujeta + Exenta;
            if (forceChangeEvents)
            {
                OnChanged(nameof(Gravada), oldGravada, gravada);
                OnChanged(nameof(Monto));
            }
        }

        public void UpdateTotalIva(bool forceChangeEvents)
        {
            decimal? oldIva = iva;
            iva = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Iva])")));
            monto = SubTotal - IvaRetenido + IvaPercibido + NoSujeta + Exenta;
            if (forceChangeEvents)
            {
                OnChanged(nameof(Iva), oldIva, iva);
                OnChanged(nameof(Monto));
            }
        }

        public void UpdateTotalNoSujeta(bool forceChangeEvents)
        {
            decimal? oldNoSujeta = noSujeta;
            noSujeta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([NoSujeta])")));
            monto = SubTotal - IvaRetenido + IvaPercibido + NoSujeta + Exenta;
            if (forceChangeEvents)
            {
                OnChanged(nameof(NoSujeta), oldNoSujeta, noSujeta);
                OnChanged(nameof(Monto));
            }
        }

        /// <summary>
        /// Se reescribe para obtener la resolución de autorización para emitir los documentos y sugerir el 
        /// próximo correlativo del documento a emitir.
        /// </summary>
        /// <param name="forceChangeEvents">Indica si se deben invocar eventos para propiedades afectadas</param>
        /// <param name="oldValue">El valor de la propiedad Tipo antes del setter</param>
        protected override void DoTipoChanged(bool forceChangeEvents, CxCTipoTransaccion oldValue)
        {
            base.DoTipoChanged(forceChangeEvents, oldValue);
            if ((oldValue.TipoDocumento.Codigo == "DACV01" || oldValue.TipoDocumento.Codigo == "DACV02") && Venta != null && Venta.Agencia != null)
            {
                AutorizacionDocumento newAutorizacion;
                var criteria = CriteriaOperator.FromLambda<AutorizacionDocumento>(x => x.Agencia.Oid == Venta.Agencia.Oid && x.Tipo.Codigo == oldValue.TipoDocumento.Codigo && x.Activo == true);
                newAutorizacion = Session.FindObject<AutorizacionDocumento>(criteria);
                var oldAutorizacion = AutorizacionDocumento;
                AutorizacionDocumento = newAutorizacion;
                OnChanged(nameof(AutorizacionDocumento), oldAutorizacion, newAutorizacion);
                // el número de documento tendrá que ingresarse manualmente, porque cuando se trate de un dte ya no es un correlativo vinculado a la resolución
                // se debe ingresar en la columna referencia
            }
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}