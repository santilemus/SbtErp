using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Asueto"), NavigationItem("Recurso Humano"), DefaultProperty(nameof(Fecha)),
        Persistent(nameof(Asueto)), CreatableItem(false)]
    [ImageName(nameof(Asueto))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Asueto : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Asueto(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        string descripcion;
        DateTime fecha;

        [DbType("datetime2"), XafDisplayName("Fecha"), RuleRequiredField("Asueto.Fecha_Requerido", "Save"), Index(0)]
        [RuleUniqueValue("Asueto.Fecha_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Indexed(Unique = true, Name = "idxAsuetoFecha")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [Size(60), DbType("varchar(60)"), XafDisplayName("Descripción"), Index(1)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [XafDisplayName("Período"), Index(2)]
        public int Periodo => fecha.Year;

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}