﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions, NavigationItem(false), ModelDefault("Caption", "Rango de Fechas")]
    [CreatableItem(false)]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class RangoFechaParams : IXafEntityObject/*, IObjectSpaceLink*/, INotifyPropertyChanged
    {
        //private IObjectSpace objectSpace;
        private DateTime fechaInicio;
        private DateTime fechaFin;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RangoFechaParams()
        {
            Oid = Guid.NewGuid();
        }

        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]  // Hide the entity identifier from UI.
        public Guid Oid { get; set; }

        [XafDisplayName("Fecha Inicio"), Index(0)]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy"), ModelDefault("EditMask", "dd/MM/yyyy")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set
            {
                fechaInicio = value;
                var tmpDate = fechaInicio.AddMonths(1);
                FechaFin = new DateTime(tmpDate.Year, tmpDate.Month, 01).AddDays(-1);
                OnPropertyChanged(nameof(FechaFin));
            }
        }

        [XafDisplayName("Fecha Fin"), Index(1)]
        [ImmediatePostData(true)]
        [RuleValueComparison("RangoFechaParams.FechaFin >= FechaInicio", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, nameof(FechaInicio),
            ParametersMode.Expression, SkipNullOrEmptyValues = false, CustomMessageTemplate = "Fecha Fin debe ser mayor o igual a Fecha Inicio")]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy"), ModelDefault("EditMask", "dd/MM/yyyy")]
        public DateTime FechaFin 
        { 
            get => fechaFin; 
            set => fechaFin = value; 
        }

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