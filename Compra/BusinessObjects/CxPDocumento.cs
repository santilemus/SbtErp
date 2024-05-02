using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;

namespace SBT.Apps.CxP.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "CxP Documento")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [CreatableItem(false)]
    //[ImageName("BO_Contact")]
    [Appearance("CxPDocumento_enable_valores", AppearanceItemType = "ViewItem",
        Criteria = "[Factura] Is Not Null And [Factura.Detalles][].Count() = 0", Enabled = true, 
        TargetItems = "Gravada;Iva;IvaRetenido;IvaPercibido;NoSujeta;Exenta")]
    [Appearance("CxPDocumento.Hide_Detalle", AppearanceItemType = "ViewItem",
        Criteria = "[Factura] Is Not Null And [Factura.Detalles][].Count() = 0", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide,
        TargetItems = "Detalles")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
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
            ivaPercibido = 0.0m;
            ivaRetenido = 0.0m;
            iva = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        //[Persistent(nameof(Gravada)), DbType("numeric(14,2)")]
        decimal gravada;
        //[Persistent(nameof(Iva)), DbType("numeric(14,2)")]
        decimal iva;
        //[Persistent(nameof(IvaPercibido)), DbType("numeric(14,2)")]
        decimal ivaPercibido;
        //[Persistent(nameof(IvaRetenido)), DbType("numeric(14,2)")]
        decimal ivaRetenido;
        //[Persistent(nameof(NoSujeta)), DbType("numeric(14,2)")]
        decimal noSujeta;
        //[Persistent(nameof(Exenta)), DbType("numeric(14,2)")]
        decimal exenta;

        //[PersistentAlias(nameof(gravada)), XafDisplayName("Gravado"), Index(9)]
        [Persistent(nameof(Gravada)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Gravada
        {
            get { return gravada; }
            set => SetPropertyValue(nameof(Gravada), ref gravada, value);
        }

        //[PersistentAlias(nameof(iva)), XafDisplayName("IVA"), Index(10)]
        [Persistent(nameof(Iva)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Iva
        {
            get { return iva; }
            set => SetPropertyValue(nameof(Iva), ref iva, value);
        }

        [PersistentAlias("[Gravada] + [Iva]")]
        [XafDisplayName("SubTotal"), Index(11)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SubTotal
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(SubTotal))); }
        }

        //[PersistentAlias(nameof(ivaRetenido)), XafDisplayName("(-) Iva Retenido"), VisibleInListView(false), Index(12)]
        [Persistent(nameof(IvaRetenido)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal IvaRetenido
        {
            get { return ivaRetenido; }
            set => SetPropertyValue(nameof(IvaRetenido), ref ivaRetenido, value);
        }

        //[PersistentAlias(nameof(ivaPercibido)), XafDisplayName("(+) Iva Percibido"), VisibleInListView(false), Index(13)]
        [Persistent(nameof(IvaPercibido)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal IvaPercibido
        {
            get { return ivaPercibido; }
            set => SetPropertyValue(nameof(IvaPercibido), ref  ivaPercibido, value);
        }

        //[PersistentAlias(nameof(noSujeta)), XafDisplayName("No Sujeta"), VisibleInListView(false), Index(14)]
        [Persistent(nameof(NoSujeta)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal NoSujeta
        {
            get { return noSujeta; }
            set => SetPropertyValue(nameof(NoSujeta), ref noSujeta, value); 
        }

        //[PersistentAlias(nameof(exenta)), XafDisplayName("Exento"), VisibleInListView(true), Index(15)]
        [Persistent(nameof(Exenta)), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Exenta
        {
            get => exenta;
            set => SetPropertyValue(nameof(Exenta), ref exenta, value);
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

        protected override void DoFromCompraFactura()
        {
            base.DoFromCompraFactura();
            Exenta = Factura.Exenta ?? 0.0m;
            Gravada = Factura.Gravada ?? 0.0m;
            Iva = Factura.Iva ?? 0.0m;
            NoSujeta = Factura.NoSujeta ?? 0.0m;
            IvaPercibido = Factura.IvaPercibido ?? 0.0m;
            IvaRetenido = Factura.IvaRetenido ?? 0.0m;
        }
        #endregion




        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}