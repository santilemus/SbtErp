using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("EdadMes"), ModelDefault("Caption", "Who Percentil Peso Longitud"),
        RuleCombinationOfPropertiesIsUnique("WhoPercentilPesoLong.TipoMes_Unico", DefaultContexts.Save, "TipoTabla,EdadMes", SkipNullOrEmptyValues = false),
        Indices("TipoTabla;EdadMes"), Persistent("WhoPercentilPesoLong"), NavigationItem(false)] //NavigationItem("Salud")]  
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class WhoPercentilPesoLong : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public WhoPercentilPesoLong(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        ETablaPercentilWho tipoTabla = ETablaPercentilWho.LengthBoy_0_2Y;
        decimal p98 = 0.0m;
        decimal p95 = 0.0m;
        decimal p90 = 0.0m;
        decimal p75 = 0.0m;
        decimal p50 = 0.0m;
        decimal p25 = 0.0m;
        decimal p10 = 0.0m;
        decimal p5 = 0.0m;
        decimal p2_3 = 0.0m;
        decimal s = 0.0m;
        decimal m = 0.0m;
        decimal l = 0.0m;
        int edadMes = 0;

        [XafDisplayName("Tipo Tabla"), DbType("smallint")]
        public ETablaPercentilWho TipoTabla
        {
            get => tipoTabla;
            set => SetPropertyValue(nameof(TipoTabla), ref tipoTabla, value);
        }

        [DbType("numeric(3,2)"), RuleRequiredField("WhoPercentilPesoLong.Mes_Requerido", "Save"),
            RuleRange("WhoPercentilPesoLong.Mes_Rango", DefaultContexts.Save, 0, 24)]
        public int EdadMes
        {
            get => edadMes;
            set => SetPropertyValue(nameof(EdadMes), ref edadMes, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal L
        {
            get => l;
            set => SetPropertyValue(nameof(L), ref l, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal M
        {
            get => m;
            set => SetPropertyValue(nameof(M), ref m, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal S
        {
            get => s;
            set => SetPropertyValue(nameof(S), ref s, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P2_3
        {
            get => p2_3;
            set => SetPropertyValue(nameof(P2_3), ref p2_3, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P5
        {
            get => p5;
            set => SetPropertyValue(nameof(P5), ref p5, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P10
        {
            get => p10;
            set => SetPropertyValue(nameof(P10), ref p10, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P25
        {
            get => p25;
            set => SetPropertyValue(nameof(P25), ref p25, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P50
        {
            get => p50;
            set => SetPropertyValue(nameof(P50), ref p50, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P75
        {
            get => p75;
            set => SetPropertyValue(nameof(P75), ref p75, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P90
        {
            get => p90;
            set => SetPropertyValue(nameof(P90), ref p90, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P95
        {
            get => p95;
            set => SetPropertyValue(nameof(P95), ref p95, value);
        }

        [DbType("numeric(8,6)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n6")]
        public decimal P98
        {
            get => p98;
            set => SetPropertyValue(nameof(P98), ref p98, value);
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}