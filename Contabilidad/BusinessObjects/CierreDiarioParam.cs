using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// BO No Persistente para los parámetros del cierre contable diario
    /// </summary>
    /// <remarks>
    /// PENDIENTE: Validar que los dias a cerrar correspondan a un periodo abierto y de la sesion actual.
    /// En este momento no se esta guardadndo en la sesion el periodo, cuando se haga quitar este comentario
    /// </remarks>
    [DomainComponent]
    [ModelDefault("Caption", "Cierre Diario")]
    [Appearance("CierreDiarioParams_Error", AppearanceItemType = "ViewItem", TargetItems = "Bitacora",
        Criteria = "[FechaDesde] > [FechaHasta] || GetMonth([FechaDesde]) != GetMonth([FechaHasta])", FontColor = "Red", Context = "DetailView", Priority = 1)]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CierreDiarioParam : IXafEntityObject, IObjectSpaceLink //, INotifyPropertyChanged
    {
        private IObjectSpace objectSpace;

        public CierreDiarioParam()
        {
            
        }

        [Browsable(false),  DevExpress.ExpressApp.Data.Key()]
        public int Oid { get; set; }

        [RuleRequiredField("CierreDiarioParams.FechaDesde_Requerido", "Save;DialogOK", "Fecha Desde debe tener un valor")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), ImmediatePostData(true)]
        public DateTime FechaDesde { get; set; }

        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G"), ImmediatePostData(true)]
        [RuleRequiredField("CierreDiarioParams.FechaHasta_Requerido", "Save;DialogOK", "Fecha Hasta debe tener un valor")]
        [RuleValueComparison("CierreDiarioParams.FechaHasta >= FechaDesde", "Save;DialogOK", 
            ValueComparisonType.GreaterThanOrEqual, "[FechaDesde]", "Fecha Hasta debe ser mayor que Fecha Desde",
            ParametersMode.Expression, SkipNullOrEmptyValues = false, ResultType = ValidationResultType.Error)]
        public DateTime FechaHasta { get; set; }

        [FieldSize(2000), XafDisplayName("Bitácora"), ModelDefault("RowCount", "6"), DevExpress.Persistent.Base.ImmediatePostData(true), Browsable(false)]
        public string   Bitacora { get; set; }

        [Browsable(false)]
        //[RuleFromBoolProperty("CierreDiarioParams_ValidarPeriodo", "Save;DialogOK", SkipNullOrEmptyValues = false, UsedProperties = "FechaHasta",
        //    CustomMessageTemplate = "Fecha Hasta debe ser mayor o igual a Fecha Desde ")]
        public bool ValidarPeriodo
        {          
            get {  return ((FechaDesde == null || FechaHasta == null) || (FechaDesde <= FechaHasta && FechaDesde.Month == FechaHasta.Month)); }
        }


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
        // Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        [Browsable(false)]
        IObjectSpace IObjectSpaceLink.ObjectSpace
        {
            get { return objectSpace; }
            set { objectSpace = value; }
        }
        #endregion

        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        //public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.SampleProperty = "Paid";
        //}

    }
}