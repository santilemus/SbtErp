using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Atributo"), NavigationItem(false), CreatableItem(false)]
    [DefaultProperty(nameof(Descripcion)), Persistent(nameof(ActivoAtributo))]
    [ImageName(nameof(ActivoAtributo))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoAtributo : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoAtributo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Atributo = null;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string descripcion;
        Listas atributo;
        ActivoCatalogo activo;

        [Association("ActivoCatalogo-Atributos"), XafDisplayName("Activo")]
        public ActivoCatalogo Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [XafDisplayName("Atributo")]
        [VisibleInLookupListView(true)]
        [RuleRequiredField("ActivoAtributo.Atributo_Requerido", DefaultContexts.Save, "Atributo es requerido")]
        [DataSourceCriteria("[Categoria] = 13")]
        [ExplicitLoading]
        public Listas Atributo
        {
            get => atributo;
            set => SetPropertyValue(nameof(Atributo), ref atributo, value);
        }


        [Size(100), DbType("varchar(100)"), XafDisplayName("Descripción")]
        [RuleRequiredField("ActivoAtributo.Descripcion_Requerido", "Save")]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}