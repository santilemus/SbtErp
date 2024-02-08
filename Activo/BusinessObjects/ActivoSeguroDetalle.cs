using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Seguro Detalle"), NavigationItem(false), CreatableItem(false)]
    [DefaultProperty(nameof(Activo)), Persistent(nameof(ActivoSeguroDetalle))]
    [ImageName(nameof(ActivoSeguroDetalle))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoSeguroDetalle : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoSeguroDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Valor = 0.0m;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal valor;
        DateTime fechaAdicion;
        ActivoCatalogo activo;
        ActivoSeguro seguro;

        [Association("ActivoSeguro-Detalles"), XafDisplayName("Seguro")]
        public ActivoSeguro Seguro
        {
            get => seguro;
            set => SetPropertyValue(nameof(Seguro), ref seguro, value);
        }

        [Association("ActivoCatalogo-SeguroDetalles"), XafDisplayName("Activo")]
        public ActivoCatalogo Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Adición")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime FechaAdicion
        {
            get => fechaAdicion;
            set => SetPropertyValue(nameof(FechaAdicion), ref fechaAdicion, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor Asegurado")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
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