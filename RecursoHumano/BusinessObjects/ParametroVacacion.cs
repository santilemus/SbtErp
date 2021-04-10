using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// BO para la parametrizacion de las vacaciones. La parametrización es por empresa y el proposito es que puedan existir
    /// multiples tramos, segun la antiguedad del empleado. Si se aplica segun la ley solo habra un tramo desde 1 año hasta 999
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Parametro Vacación"), NavigationItem("Recurso Humano"), Persistent(nameof(ParametroVacacion)),
        DefaultProperty("Desde")]
    [ImageName("ParametroVacacion")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ParametroVacacion : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ParametroVacacion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Empresa empresa;
        int desde;
        int hasta;
        decimal valor;
        int dias;
        bool activo = true;

        [Persistent("Empresa"), DbType("int"), XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("smallint"), Persistent("Desde"), XafDisplayName("Años Desde"),
            RuleValueComparison("ParametroVacacion.Desde >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public int Desde
        {
            get => desde;
            set => SetPropertyValue(nameof(Desde), ref desde, value);
        }

        [DbType("smallint"), Persistent("Hasta"), XafDisplayName("Años Hasta"),
    RuleValueComparison("ParametroVacacion.Hasta >= Desde", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "Desde", ParametersMode.Expression,
    SkipNullOrEmptyValues = false)]
        public int Hasta
        {
            get => hasta;
            set => SetPropertyValue(nameof(Hasta), ref hasta, value);
        }


        [DbType("numeric(6,2)"), Persistent("Valor"), XafDisplayName("Valor"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"),
    RuleValueComparison("ParametroVacacion.Valor >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ToolTip("Porcentaje del salario para calcular el monto que corresponde al beneficio de la vacacion")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [DbType("smallint"), Persistent("Dias"), XafDisplayName("Dias de Vacación"),
            RuleValueComparison("ParametroVacacion.Dias >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public int Dias
        {
            get => dias;
            set => SetPropertyValue(nameof(Dias), ref dias, value);
        }

        [DbType("bit"), Persistent("Activo"), XafDisplayName("Activo"), RuleRequiredField("ParametroVacacion.Activo_Requerido", DefaultContexts.Save)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}