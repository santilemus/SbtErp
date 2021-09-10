using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Linq;
using DevExpress.Persistent.Validation;

namespace SBT.Apps.CxP.Module.BusinessObjects
{
    [DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CxPDocumento : CxPTransaccion
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CxPDocumento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
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

        [PersistentAlias("[Gravada] + [Iva]")]
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
        /// OJO SE DEBE QUITAR Y SUMAR ESTO EN Monto del BO ancestro
        /// </summary>
        [PersistentAlias("[SubTotal] - [IvaRetenido] + [IvaPercibido]  + [NoSujeta] + [Exenta] ")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [XafDisplayName("Total"), Index(16)]
        public decimal Total
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(Total))); }
        }
        #endregion

        #region Collecciones
        [Association("CxPDocumento-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<CxPDocumentoDetalle> Detalles => GetCollection<CxPDocumentoDetalle>(nameof(Detalles));

        #endregion

        #region Metodos
        public void UpdateTotalExenta(bool forceChangeEvents)
        {
            decimal? oldExenta = exenta;
            exenta = Convert.ToDecimal(Evaluate(CriteriaOperator.Parse("[Detalles].Sum([Exenta])")));
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

        protected override void Anular()
        {
            foreach (CxPDocumentoDetalle item in Detalles)
            {
                item.CantidadAnulada = item.Cantidad;
                item.Cantidad = 0.0m;
                item.PrecioUnidad = 0.0m;                
            }
            base.Anular();
        }
        #endregion




        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}