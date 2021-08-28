using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Historia de padecimientos y/o enfermedades comunes en la familia
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Historia Familiar"), NavigationItem(false), XafDefaultProperty(nameof(QuePadecen)),
        Persistent("HistoriaFamiliar"), CreatableItem(false)]
    [ImageName(nameof(HistoriaFamiliar))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class HistoriaFamiliar : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public HistoriaFamiliar(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Paciente paciente;
        string quienes;
        string quePadecen;


        [Association("Paciente-HistoriaFamiliares"), XafDisplayName("Paciente"), Persistent("Paciente")]
        public Paciente Paciente
        {
            get => paciente;
            set => SetPropertyValue(nameof(Paciente), ref paciente, value);
        }

        [Size(200), DbType("varchar(200)"), Persistent("QuePadecen"), XafDisplayName("Descripción Diagnostico"),
            RuleRequiredField("HistoriaFamiliar.DescripcionDiagnostico_Requerido", DefaultContexts.Save)]
        public string QuePadecen
        {
            get => quePadecen;
            set => SetPropertyValue(nameof(QuePadecen), ref quePadecen, value);
        }

        [Size(200), DbType("varchar(200)"), Persistent("Quienes"), XafDisplayName("Quienes")]
        public string Quienes
        {
            get => quienes;
            set => SetPropertyValue(nameof(Quienes), ref quienes, value);
        }

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}