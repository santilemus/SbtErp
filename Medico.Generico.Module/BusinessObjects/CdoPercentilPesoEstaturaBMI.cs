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

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("EdadMes"), ModelDefault("Caption", "CDO Percentil Peso Estatura BMI"), 
        RuleCombinationOfPropertiesIsUnique("CdoPercentilPesoEstaturaBMI.TipoEdad_Unico", DefaultContexts.Save, "TipoTabla,EdadMes", SkipNullOrEmptyValues = false),
        Indices("TipoTabla;EdadMes"), Persistent("CdoPercentilPesoEstaturaBMI"), NavigationItem("Salud")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CdoPercentilPesoEstaturaBMI : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CdoPercentilPesoEstaturaBMI(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        decimal p97 = 0.0m;
        decimal p95 = 0.0m;
        decimal p90 = 0.0m;
        decimal p85 = 0.0m;
        decimal p75 = 0.0m;
        decimal p50 = 0.0m;
        decimal p25 = 0.0m;
        decimal p10 = 0.0m;
        decimal p5 = 0.0m;
        decimal p3 = 0.0m;
        decimal edadMes = 0.0m;
        ETablaPercentilCdo tipoTabla = ETablaPercentilCdo.BMI_Female;

        public ETablaPercentilCdo TipoTabla
        {
            get => tipoTabla;
            set => SetPropertyValue(nameof(TipoTabla), ref tipoTabla, value);
        }

        [DbType("numeric(5,2)"), XafDisplayName("Edad (Meses)"), RuleRequiredField("CdoPercentilPesoEstaturaBMI.EdadMes_Requerido", "Save"),
            RuleRange("CdoPercentilPesoEstaturaBMI.EdadMes_Rango", DefaultContexts.Save, 0, 240)]
        public decimal EdadMes
        {
            get => edadMes;
            set => SetPropertyValue(nameof(EdadMes), ref edadMes, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P3
        {
            get => p3;
            set => SetPropertyValue(nameof(P3), ref p3, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P5
        {
            get => p5;
            set => SetPropertyValue(nameof(P5), ref p5, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P10
        {
            get => p10;
            set => SetPropertyValue(nameof(P10), ref p10, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P25
        {
            get => p25;
            set => SetPropertyValue(nameof(P25), ref p25, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P50
        {
            get => p50;
            set => SetPropertyValue(nameof(P50), ref p50, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P75
        {
            get => p75;
            set => SetPropertyValue(nameof(P75), ref p75, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P85
        {
            get => p85;
            set => SetPropertyValue(nameof(P85), ref p85, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P90
        {
            get => p90;
            set => SetPropertyValue(nameof(P90), ref p90, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P95
        {
            get => p95;
            set => SetPropertyValue(nameof(P95), ref p95, value);
        }

        [DbType("numeric(9,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P97
        {
            get => p97;
            set => SetPropertyValue(nameof(P97), ref p97, value);
        }


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}