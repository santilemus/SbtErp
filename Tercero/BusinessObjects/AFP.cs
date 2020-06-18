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

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem("Catalogos"), ModelDefault("Caption", "AFP"), DefaultProperty("Proveedor")]
    [ImageName("afp")]
    [Persistent("Afp")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AFP : XPCustomBaseBO
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
        Tercero proveedor;
        string siglas;
        decimal aporteAfiliado;
        decimal aporteEmpresa;
        decimal comision;

        [XafDisplayName("Proveedor"), DbType("int"), Key(false), RuleRequiredField("AFP.Proveedor_Requrido", "Save")]
        [DataSourceCriteria("[Roles][[IdRole] = 10 And [Activo] = True]")]
        public Tercero Proveedor
        {
            get => proveedor;
            set => SetPropertyValue(nameof(Proveedor), ref proveedor, value);
        }

        [Size(10), DbType("Varchar(10)"), XafDisplayName("Siglas"), RuleRequiredField("AFP.Siglas_Requerido", DefaultContexts.Save)]
        public string Siglas
        {
            get => siglas;
            set => SetPropertyValue(nameof(Siglas), ref siglas, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName("Porc. Aporte Afiliado"), ToolTip("Porcentaje del aporte del afiliado")]
        [ModelDefault("DisplayFormat", "{0:P2}"), ModelDefault("EditMask", "n2")]
        [RuleRange("AFP.Aporte_Afiliado > 0", DefaultContexts.Save, 0.01, 100.00, SkipNullOrEmptyValues = false)]
        [VisibleInLookupListView(false)]
        public decimal AporteAfiliado
        {
            get => aporteAfiliado;
            set => SetPropertyValue(nameof(AporteAfiliado), ref aporteAfiliado, value);
        }

        [DbType("numeric(10,4)"), XafDisplayName(" Aporte Empresa"), ToolTip("Porcentaje del aporte de la empresa")]
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