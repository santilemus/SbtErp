using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Depreciación"), NavigationItem(false), CreatableItem(false)]
    [Persistent(nameof(ActivoDepreciacion)), DefaultProperty(nameof(Fecha))]
    [ImageName(nameof(ActivoDepreciacion))]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoDepreciacion : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoDepreciacion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            AcumuladoAnterior = 0.0m;
            Valor = 0.0m;
            Tipo = ETipoDepreciacion.Activo;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        ActivoMejora mejora;
        ETipoDepreciacion tipo;
        decimal valor;
        decimal acumuladoAnterior;
        decimal valorMoneda;
        DateTime fecha;
        ActivoCatalogo activo;

        [Association("ActivoCatalogo-Depreciaciones"), XafDisplayName("Depreciaciones")]
        public ActivoCatalogo Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [XafDisplayName("Fecha"), DbType("datetime"), RuleRequiredField("ActivoDepreciacion.Fecha_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        [Indexed(nameof(Activo), Name = "idxActivoFecha_ActivoDepreciacion", Unique = true)]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [XafDisplayName("Valor Moneda"), DbType("numeric(14,2)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ValorMoneda
        {
            get => valorMoneda;
            set => SetPropertyValue(nameof(ValorMoneda), ref valorMoneda, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Acumulado Anterior")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal AcumuladoAnterior
        {
            get => acumuladoAnterior;
            set => SetPropertyValue(nameof(AcumuladoAnterior), ref acumuladoAnterior, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor")]
        [RuleValueComparison("ActivoDepreciacion.Valor_valido", DefaultContexts.Save, ValueComparisonType.NotEquals, 0, 
            "El valor de la depreciación debe ser diferente a cero")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [XafDisplayName("Tipo Depreciación")]
        public ETipoDepreciacion Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }


        [Association("ActivoMejora-Depreciaciones"), XafDisplayName("Mejora")]
        [ToolTip("Mejora o Versión a la cual corresponde la depreciación")]
        public ActivoMejora Mejora
        {
            get => mejora;
            set => SetPropertyValue(nameof(Mejora), ref mejora, value);
        }

        #endregion


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}