using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Plan Medico Regla"), Persistent("PlanMedicoDetalle"),
        XafDefaultProperty("Regla"), NavigationItem(false), CreatableItem(false)]
    [ImageName("list-info")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PlanMedicoDetalle : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PlanMedicoDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        PlanMedico plan;
        Regla regla;


        [Association("Plan-Detalles"), XafDisplayName("Plan"), Persistent("PlanMedico"),
            RuleRequiredField("PlanMedicoRegla.Plan_Requerido", "Save")]
        public PlanMedico Plan
        {
            get => plan;
            set => SetPropertyValue(nameof(Plan), ref plan, value);
        }

        [Association("Regla-PlanDetalles"), Persistent("Regla"), XafDisplayName("Regla"),
            RuleRequiredField("PlanRegla.Regla_Requerido", DefaultContexts.Save)]
        public Regla Regla
        {
            get => regla;
            set => SetPropertyValue(nameof(Regla), ref regla, value);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}