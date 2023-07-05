using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Banco.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects 
{
    [DefaultClassOptions, ModelDefault("Caption", "BancoTransaccion - Partida"), NavigationItem(false), CreatableItem(false),
        ]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoTransaccionPartida : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoTransaccionPartida(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Partida partida;
        BancoTransaccion bancoTransaccion;

        [Association("BancoTransaccion-Partidas"), XafDisplayName("Transacción"), DevExpress.Xpo.Aggregated]
        public BancoTransaccion BancoTransaccion
        {
            get => bancoTransaccion;
            set => SetPropertyValue(nameof(BancoTransaccion), ref bancoTransaccion, value);
        }

        [Association("Partida-BancoTransaccionPartidas")]
        [XafDisplayName("Partida")]
        public Partida Partida
        {
            get => partida;
            set => SetPropertyValue(nameof(Partida), ref partida, value);
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