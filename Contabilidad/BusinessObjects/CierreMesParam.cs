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
using DevExpress.Persistent.Validation;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// BO para los parametros del cierre de mes
    /// </summary>
    [DomainComponent]
    [DefaultClassOptions, ModelDefault("Caption", "Cierre de Mes"), NavigationItem("Contabilidad")]
    [ImageName("CierreMes")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CierreMesParam 
    {
        public CierreMesParam()
        {
        }

        [Browsable(false)]
        [DevExpress.ExpressApp.Data.Key]
        public int Oid { get; set; }

        [XafDisplayName("Fecha de Cierre"), ModelDefault("DisplayFormat", "{0:D}"), ModelDefault("EditMask", "d")]
        public DateTime FechaCierre { get; set; }

        [FieldSize(2000), XafDisplayName("Bitácora"), ModelDefault("RowCount", "10")]
        public string Bitacora { get; set; }

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

    }
}