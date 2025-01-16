using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;


namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// BO para relacionar los roles del tercero con las cuentas contables, que permitan identificar la cuenta contable a utilizar
    /// en la generacion de asientos contables.
    /// </summary>
    [DefaultClassOptions, NavigationItem(false), CreatableItem(false), ModelDefault("Caption", "Tercero Cuenta Contable")]
    [Persistent(nameof(TerceroCuentaContable))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TerceroCuentaContable : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private TerceroRole terceroRole;
        private Catalogo cuenta;

        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public TerceroCuentaContable(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
       

        [Association("TerceroRole-TerceroCuentaRole")]
        [System.ComponentModel.DisplayName("Role")]
        //[DataSourceCriteria("[Tercero.Oid] == '")]
        public TerceroRole TerceroRole
        {
            get => terceroRole;
            set => SetPropertyValue(nameof(TerceroRole), ref terceroRole, value);
        }

        [System.ComponentModel.DisplayName("Cuenta Contable")]
        [ToolTip(@"Cuenta contable por empresa asociada al tercero con base a su role. Por lo tanto puede ser una cuenta de clientes, de proveedor, etc")]
        [DataSourceCriteria("[Empresa.Oid] == EmpresaActualOid()")]
        public Catalogo Cuenta
        {
            get => cuenta;
            set => SetPropertyValue(nameof(Cuenta), ref cuenta, value);
        }


        #endregion
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}