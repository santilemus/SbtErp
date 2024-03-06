using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem(false), CreatableItem(false), ModelDefault("Caption", "Recuento de 24H")]
    [DefaultProperty(nameof(TiempoComida))]
    [Persistent(nameof(NutricionRecuento24H))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class NutricionRecuento24H : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private ConsultaNutricion consulta;
        private ETiempoComida clasificacion;
        private DateTime hora;
        private string alimento;
        private decimal porciones;

        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public NutricionRecuento24H(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        [Association("ConsultaNutricion_Recuento24H")]
        public ConsultaNutricion Consulta
        {
            get => consulta;
            set => SetPropertyValue(nameof(Consulta), ref consulta, value);
        }

        [System.ComponentModel.DisplayName("Tiempo Comida")]
        public ETiempoComida TiempoComida
        {
            get => clasificacion;
            set => SetPropertyValue(nameof(TiempoComida), ref clasificacion, value);
        }

        [DbType("datetime")]
        [System.ComponentModel.DisplayName("Fecha y Hora")]
        [ModelDefault("EditMask", "dd/MM/yyyy hh:mm"), ModelDefault("DisplayFormat", "{0:dd/MM/yyy hh:mm}")]
        public DateTime Hora
        {
            get => hora;
            set => SetPropertyValue(nameof(Hora), ref hora, value);
        }

        [Size(100), DbType("varchar(100)")]
        [System.ComponentModel.DisplayName("Alimentos")]
        [ToolTip("Alimentos Ingeridos")]
        public string Alimento
        {
            get => alimento;
            set => SetPropertyValue(nameof(Alimento), ref alimento, value);
        }

        [ModelDefault("EditMask", "n2"), ModelDefault("DisplayFormat", "{0:N2}")]
        [DbType("numeric(6,2)")]
        public decimal Porciones
        {
            get => porciones;
            set => SetPropertyValue(nameof(Porciones), ref porciones, value);
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

    public enum ETiempoComida
    {
        Desayuno = 0,
        Refrigerio1 = 1,
        Amuerzo = 2,
        Refrigerio2 = 3,
        Cena = 4
    }
 
}