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

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Tabla IMC"), NavigationItem("Salud"), XafDefaultProperty("Descripcion"),
        Persistent("TablaIMC")]
    [RuleCombinationOfPropertiesIsUnique("TablaIMC.Rango_Unico", DefaultContexts.Save, "Desde,Hasta",
        CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, IncludeCurrentObject = true)]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TablaIMC : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TablaIMC(Session session)
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
        decimal hasta;
        decimal desde;

        [DbType("numeric(5,2)"), Persistent("Desde"), XafDisplayName("Desde"), ModelDefault("DisplayFormat", "N4"), ModelDefault("EditMask", "N4"),
            RuleRequiredField("TablaIMC.Desde_Requerido", "Save"),
            RuleRange("TablaIMC.Desde_Rango", DefaultContexts.Save, 0, 40, ResultType = ValidationResultType.Warning)]
        public decimal Desde
        {
            get => desde;
            set => SetPropertyValue(nameof(Desde), ref desde, value);
        }

        [DbType("numeric(5,2)"), Persistent("Hasta"), XafDisplayName("Hasta"), ModelDefault("DisplayFormat", "N4"), ModelDefault("EditMask", "N4"),
            RuleRequiredField("TablaIMC.Hasta_Requerido", "Save"),
            RuleRange("TablaIMC.Hasta_Rango", DefaultContexts.Save, 15.01, 999.00, ResultType = ValidationResultType.Warning)]
        public decimal Hasta
        {
            get => hasta;
            set => SetPropertyValue(nameof(Hasta), ref hasta, value);
        }

        [Size(60), DbType("varchar(60)"), Persistent("Descripcion"), XafDisplayName("Descripción"),
            RuleRequiredField("TablaIMC.Descripcion_Requerido", DefaultContexts.Save)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        
        [Browsable(false)]
        [RuleFromBoolProperty("TablaIMC.RangosValidos", DefaultContexts.Save, "El Rango ingresado no es valido" )]
        public bool HastaEsValido
        {
            get
            {
                return ((Desde == 0.0m && Hasta == 14.9999m) || (Desde == 15.00m && Hasta == 15.9999m) || (Desde == 16.00m && Hasta == 18.4999m) ||
                    (Desde == 18.5m && Hasta == 24.9999m) || (Desde == 25.00m && Hasta == 29.9999m) || (Desde == 30.0m && Hasta == 34.9999m) ||
                    (Desde == 35.0m && Hasta == 39.9999m) || (Desde >= 40.0m));
            }
        }
        
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}