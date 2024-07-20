using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions, CreatableItem(false), NavigationItem(false)]
    [ModelDefault("Caption", "Parámetros - Calcular Depreciación")]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DepreciacionParams : IXafEntityObject/*, IObjectSpaceLink*/, INotifyPropertyChanged
    {
        //private IObjectSpace objectSpace;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public DepreciacionParams()
        {
            Oid = Guid.NewGuid();
        }

        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]  // Hide the entity identifier from UI.
        public Guid Oid { get; set; }

        [DisplayName("Mes Depreciación")]
        [ModelDefault("DisplayFormat", "{0:MM/yyyy}")]
        [ModelDefault("EditMask", "MM/yyyy")]
        [ToolTip("Mes al cual corresponde la depreciación a calcular. No debe existir una depreciación calculada, sino tiene que revertirla antes")]
        [RuleRequiredField("DepreciacionParams.MesDepreciacion", "Accept")]
        public DateTime MesDepreciacion { get; set; }

        [DisplayName("Valor Mínimo")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [ModelDefault("EditMask", "n2")]
        [ToolTip("Valor mínimo de adquisición del bien para calcularle depreciación")]
        [RuleValueComparison("DepreciacionParams.ValorMinimo > 0", "Accept", ValueComparisonType.GreaterThan, 0.0)]
        public decimal ValorMinimo { get; set; }

        [ModelDefault("DisplayFormat", "{0:n0}")]
        [DisplayName("Vida Útil Mínima")]
        [ModelDefault("EditMask", "n0")]
        [ToolTip("Vida últil mínima del bien para calcularle depreciación")]
        [RuleValueComparison("DepreciacionParams.VidaUtilMinima", "Accept", ValueComparisonType.GreaterThan, 0.0)]
        public int VidaUtilMinima { get; set; }

        //private string sampleProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        //public string SampleProperty
        //{
        //    get { return sampleProperty; }
        //    set
        //    {
        //        if (sampleProperty != value)
        //        {
        //            sampleProperty = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.SampleProperty = "Paid";
        //}

        #region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        void IXafEntityObject.OnCreated()
        {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded()
        {
            // Place the code that is executed each time the entity is loaded here.
        }
        void IXafEntityObject.OnSaving()
        {
            // Place the code that is executed each time the entity is saved here.
        }
        #endregion

        #region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        // If you implement this interface, handle the NonPersistentObjectSpace.ObjectGetting event and find or create a copy of the source object in the current Object Space.
        // Use the Object Space to access other entities (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        //IObjectSpace IObjectSpaceLink.ObjectSpace {
        //    get { return objectSpace; }
        //    set { objectSpace = value; }
        //}
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}