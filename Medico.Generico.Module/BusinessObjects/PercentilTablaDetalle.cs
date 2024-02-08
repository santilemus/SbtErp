using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty(nameof(Oid)), ModelDefault("Caption", "Percentil Tabla"), CreatableItem(false),
        Persistent(nameof(PercentilTablaDetalle)), NavigationItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PercentilTablaDetalle : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PercentilTablaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades
        decimal p15th;
        PercentilTabla percentilTabla;
        decimal p99th;
        decimal p85th;
        decimal p1st;
        decimal desviacionStandard;
        decimal p97th;
        decimal p95th;
        decimal? p90th;
        decimal p75th;
        decimal p50th;
        decimal p25th;
        decimal? p10th;
        decimal p5th;
        decimal p3rd;
        decimal sigma;
        decimal mu;
        decimal lambda;
        int edadMes;



        [Association("PercentilTabla-Detalles"), XafDisplayName("Percentil Tabla"), Index(0)]
        public PercentilTabla PercentilTabla
        {
            get => percentilTabla;
            set => SetPropertyValue(nameof(PercentilTabla), ref percentilTabla, value);
        }

        [DbType("smallint"), RuleRange("WhoPercentilPesoLong.Mes_Rango", DefaultContexts.Save, 0, 24)]
        [XafDisplayName("Mes"), Index(1)]
        public int EdadMes
        {
            get => edadMes;
            set => SetPropertyValue(nameof(EdadMes), ref edadMes, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("L"), Index(2)]
        public decimal Lambda
        {
            get => lambda;
            set => SetPropertyValue(nameof(Lambda), ref lambda, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("M"), Index(3)]
        public decimal Mu
        {
            get => mu;
            set => SetPropertyValue(nameof(Mu), ref mu, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("S"), Index(4)]
        public decimal Sigma
        {
            get => sigma;
            set => SetPropertyValue(nameof(Sigma), ref sigma, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("SD"), Index(5)]
        public decimal DesviacionStandard
        {
            get => desviacionStandard;
            set => SetPropertyValue(nameof(DesviacionStandard), ref desviacionStandard, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N6}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("1st"), Persistent(nameof(P1st)), Index(6)]
        public decimal P1st
        {
            get => p1st;
            set => SetPropertyValue(nameof(P1st), ref p1st, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("3rd"), Persistent(nameof(P3rd)), Index(7)]
        public decimal P3rd
        {
            get => p3rd;
            set => SetPropertyValue(nameof(P3rd), ref p3rd, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("5th"), Persistent(nameof(P5th)), Index(8)]
        public decimal P5th
        {
            get => p5th;
            set => SetPropertyValue(nameof(P5th), ref p5th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("10th"), Persistent(nameof(P10th)), Index(9)]
        public decimal? P10th
        {
            get => p10th;
            set => SetPropertyValue(nameof(P10th), ref p10th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("15th"), Persistent(nameof(P15th)), Index(10)]
        public decimal P15th
        {
            get => p15th;
            set => SetPropertyValue(nameof(P15th), ref p15th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("25th"), Persistent(nameof(P25th)), Index(11)]
        public decimal P25th
        {
            get => p25th;
            set => SetPropertyValue(nameof(P25th), ref p25th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("50th"), Persistent(nameof(P50th)), Index(12)]
        public decimal P50th
        {
            get => p50th;
            set => SetPropertyValue(nameof(P50th), ref p50th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("75th"), Persistent(nameof(P75th)), Index(13)]
        public decimal P75th
        {
            get => p75th;
            set => SetPropertyValue(nameof(P75th), ref p75th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("85th"), Persistent(nameof(P85th)), Index(14)]
        public decimal P85th
        {
            get => p85th;
            set => SetPropertyValue(nameof(P85th), ref p85th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("90th"), Persistent(nameof(P90th)), Index(15)]
        public decimal? P90th
        {
            get => p90th;
            set => SetPropertyValue(nameof(P90th), ref p90th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("95th"), Persistent(nameof(P95th)), Index(16)]
        public decimal P95th
        {
            get => p95th;
            set => SetPropertyValue(nameof(P95th), ref p95th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("97th"), Persistent(nameof(P97th)), Index(17)]
        public decimal P97th
        {
            get => p97th;
            set => SetPropertyValue(nameof(P97th), ref p97th, value);
        }

        [DbType("numeric(8,5)"), ModelDefault("DisplayFormat", "{0:N5}"), ModelDefault("EditMask", "n5")]
        [XafDisplayName("99th"), Persistent(nameof(P99th)), Index(18)]
        public decimal P99th
        {
            get => p99th;
            set => SetPropertyValue(nameof(P99th), ref p99th, value);
        }

        #endregion

        #region Metodos
        public decimal ObtenerPercentil(decimal aValor)
        {
            if (aValor < 3.0m)
                return 1.0m;
            else if (aValor >= 3.0m && aValor < 5.0m)
                return 3.0m;
            else if (aValor >= 5.0m && aValor < 15.0m)
                return 5.0m;
            else if (aValor >= 15.0m && aValor < 25.0m)
                return 15.0m;
            else if (aValor >= 25.0m && aValor < 50.0m)
                return 25.0m;
            else if (aValor >= 50.0m && aValor < 75.0m)
                return 50.0m;
            else if (aValor >= 75.0m && aValor < 85.0m)
                return 75.0m;
            else if (aValor >= 85.0m && aValor < 90.0m)
                return 85.0m;
            else if (aValor >= 90.0m && aValor < 95.0m)
                return 90.0m;
            else if (aValor >= 95.0m && aValor < 97.0m)
                return 95.0m;
            else if (aValor >= 97.0m && aValor < 99.0m)
                return 97.0m;
            else
                return 99.0m;
        }

        #endregion
        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}