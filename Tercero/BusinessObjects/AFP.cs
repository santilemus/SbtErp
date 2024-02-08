using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem("Catalogos"), ModelDefault("Caption", "AFP"), DefaultProperty("Proveedor")]
    [ImageName("afp"), CreatableItem(false)]
    [DevExpress.Xpo.MapInheritance(DevExpress.Xpo.MapInheritanceType.ParentTable)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AFP : Tercero
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AFP(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades
        decimal aporteAfiliado;
        decimal aporteEmpresa;
        decimal comision;


        [DbType("numeric(10,4)"), XafDisplayName("Porc. Aporte Afiliado"), ToolTip("Porcentaje del aporte del afiliado")]
        [ModelDefault("DisplayFormat", "{0:P2}"), ModelDefault("EditMask", "n2")]
        [RuleRange("AFP.Aporte_Afiliado > 0", DefaultContexts.Save, 0.01, 100.00, SkipNullOrEmptyValues = false)]
        [VisibleInLookupListView(false)]
        public decimal AporteAfiliado
        {
            get => aporteAfiliado;
            set => SetPropertyValue(nameof(AporteAfiliado), ref aporteAfiliado, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Aporte Empresa"), ToolTip("Porcentaje del aporte de la empresa")]
        [ModelDefault("DisplayFormat", "{0:P2}"), ModelDefault("EditMask", "n2")]
        [VisibleInLookupListView(false)]
        public decimal AporteEmpresa
        {
            get => aporteEmpresa;
            set => SetPropertyValue(nameof(AporteEmpresa), ref aporteEmpresa, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Comisión"), ToolTip("Porcentaje de la comisión por administración")]
        [ModelDefault("DisplayFormat", "{0:P2}"), ModelDefault("EditMask", "n2")]
        [VisibleInLookupListView(false)]
        public decimal Comision
        {
            get => comision;
            set => SetPropertyValue(nameof(Comision), ref comision, value);
        }


        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}