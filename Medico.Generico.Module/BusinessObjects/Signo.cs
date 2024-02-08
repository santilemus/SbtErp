using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Signo"), NavigationItem("Salud"), CreatableItem(false), DefaultProperty(nameof(Nombre)),
        Persistent(nameof(Signo))]
    [ImageName("SignoLista")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Signo : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Signo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        private string nombre;
        private string unidad;
        private ETipoSigno tipo = ETipoSigno.Vital;
        private bool activo = true;

        [Size(50), DbType("varchar(50)")]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Size(25), DbType("varchar(25)")]
        public string Unidad
        {
            get => unidad;
            set => SetPropertyValue(nameof(Unidad), ref unidad, value);
        }

        [DbType("smallint"), XafDisplayName("Tipo Signo"), VisibleInLookupListView(true)]
        public ETipoSigno Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }
        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}