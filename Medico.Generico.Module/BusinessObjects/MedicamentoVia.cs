using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a las vías de administración de los medicamentos
    /// </summary>

    [DefaultClassOptions, NavigationItem(false), ModelDefault("Caption", "Vía Administración"), CreatableItem(false),
        DefaultProperty(nameof(Via))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class MedicamentoVia : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MedicamentoVia(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Medicamento medicamento;
        MedicoLista via;

        [Association("Medicamento-Vias"), XafDisplayName("Medicamento"), Index(0)]
        public Medicamento Medicamento
        {
            get => medicamento;
            set => SetPropertyValue(nameof(Medicamento), ref medicamento, value);
        }

        [DataSourceCriteria("[Categoria] == 6"), XafDisplayName("Vía Administración"), Index(1),
            RuleRequiredField("MedicamentoVia.Via_Requerido", "Save")]
        [ExplicitLoading]
        public MedicoLista Via
        {
            get => via;
            set => SetPropertyValue(nameof(Via), ref via, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}