using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Compra.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde al detalle de los pagos via movimiento del modulo de bancos. Normalmente son a proveedores
    /// </summary>
    /// <remarks>
    /// De manera dinamica en ErpModule se debe agregar metodo para crear dinamicamente la propiedad y los atributos 
    /// que relacionan con la factura de compras a cancelar.
    /// Ademas se deben agregar de forma dinamica el atributo DefaultProperty de este BO 
    /// </remarks>

    [DefaultClassOptions, ModelDefault("Caption", "Pagos"), CreatableItem(false), NavigationItem(false)]
    [Persistent(nameof(BancoTransaccionPago))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoTransaccionPago : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTransaccionPago(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            monto = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        CompraFactura factura;
        decimal monto;
        BancoTransaccion bancoTransaccion;

        //[Association("BancoTransaccion-Pagos")]
        [XafDisplayName("Transacción")]
        public BancoTransaccion BancoTransaccion
        {
            get => bancoTransaccion;
            set => SetPropertyValue(nameof(BancoTransaccion), ref bancoTransaccion, value);
        }

        //[Association("CompraFactura-BancoTransaccionPagos")]
        [XafDisplayName("Factura")]
        [RuleRequiredField("BancoTransaccionPago.Factura_Requerido", DefaultContexts.Save)]
        [DataSourceCriteria("[Proveedor] == '@This.BancoTransaccion.Proveedor' && [Saldo] > 0")]
        public CompraFactura Factura
        {
            get => factura;
            set => SetPropertyValue(nameof(Factura), ref factura, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Monto")]
        [RuleValueComparison("BancoTransaccionPago.Monto > 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}