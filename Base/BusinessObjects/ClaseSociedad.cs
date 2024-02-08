using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem("Catalogos"), ModelDefault("Caption", "Clase Sociedad"), CreatableItem(false)]
    [DefaultProperty(nameof(Nombre)), Persistent(nameof(ClaseSociedad))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ClaseSociedad : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ClaseSociedad(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region propiedades

        decimal porcentajeRenta;
        bool activo;
        decimal porcentajeCapital;
        decimal porcentajeAnual;
        string nombre;

        [Size(60), XafDisplayName("Nombre"), RuleRequiredField("ClaseSociedad.Nombre_Requerido", DefaultContexts.Save)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("numeric(14,6)"), XafDisplayName("Porcentaje Anual Reserva"), RuleRequiredField("ClaseSociedad.PorcenteAnual_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:P6}"), ModelDefault("EditMask", "p6")]
        [ToolTip("Porcentaje anual de las reservas")]
        public decimal PorcentajeAnual
        {
            get => porcentajeAnual;
            set => SetPropertyValue(nameof(PorcentajeAnual), ref porcentajeAnual, value);
        }

        [DbType("numeric(14,6)"), XafDisplayName("Porcentaje Capital"), RuleRequiredField("ClaseSociedad.PorcenteCapital_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:P6}"), ModelDefault("EditMask", "p6")]
        public decimal PorcentajeCapital
        {
            get => porcentajeCapital;
            set => SetPropertyValue(nameof(PorcentajeCapital), ref porcentajeCapital, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Porcentaje Impuesto Renta")]
        [ModelDefault("DisplayFormat", "{0:P2}"), ModelDefault("EditMask", "p2")]
        public decimal PorcentajeRenta
        {
            get => porcentajeRenta;
            set => SetPropertyValue(nameof(PorcentajeRenta), ref porcentajeRenta, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), RuleRequiredField("ClaseSociedad.Activo_Requerido", DefaultContexts.Save)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
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