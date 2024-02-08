using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Ajuste"), NavigationItem(false), CreatableItem(false)]
    [DefaultProperty(nameof(Fecha)), Persistent(nameof(ActivoAjuste))]
    [ImageName(nameof(ActivoAjuste))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoAjuste : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoAjuste(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string comentario;
        decimal valor;
        decimal valorMoneda;
        DateTime fecha;
        ActivoCatalogo activo;

        [Association("ActivoCatalogo-Ajustes"), XafDisplayName("Activo")]
        public ActivoCatalogo Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha"), RuleRequiredField("ActivoAjuste.Fecha_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor Moneda")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor Ajuste")]
        [RuleValueComparison("ActivoAjuste.Valor_Valido", DefaultContexts.Save, ValueComparisonType.NotEquals, 0, "El valor del ajuste debe ser diferente de cero")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [Size(250), DbType("varchar(250)"), XafDisplayName("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}