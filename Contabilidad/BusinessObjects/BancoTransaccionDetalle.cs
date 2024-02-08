using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Contabilidad.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para el vaucher o detalle de una transacción de bancos 
    /// </summary>

    [ModelDefault("Caption", "Detalle Transacción Bancaria"), Persistent(nameof(BancoTransaccionDetalle)), NavigationItem(false), CreatableItem(false),
        DefaultProperty("CodCuenta")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoTransaccionDetalle : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTransaccionDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        BancoTransaccion transaccion;
        Catalogo cuentaContable;
        decimal debe;
        decimal haber;

        [Association("BancoTransaccion-Detalles"), Persistent("BancoTransaccion"), XafDisplayName("Transacción")]
        public BancoTransaccion Transaccion
        {
            get => transaccion;
            set => SetPropertyValue(nameof(Transaccion), ref transaccion, value);
        }

        [Persistent("CuentaContable"), XafDisplayName("Cuenta Contable"),
            RuleRequiredField("BancoTransacionDetalle.CuentaContable_Requerido", "Save")]
        public Catalogo CuentaContable
        {
            get => cuentaContable;
            set => SetPropertyValue(nameof(CuentaContable), ref cuentaContable, value);
        }

        [DbType("money"), Persistent("Debe"), XafDisplayName("Debe")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Debe
        {
            get => debe;
            set => SetPropertyValue(nameof(Debe), ref debe, value);
        }

        [DbType("money"), Persistent("Haber"), XafDisplayName("Haber")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Haber
        {
            get => haber;
            set => SetPropertyValue(nameof(Haber), ref haber, value);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}