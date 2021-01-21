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

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano. BO para la parametrización de las reglas de calculo del aguinaldo. La parametrización es por empresa y el objetivo 
    /// es que se puedan parametrizar los tramos segun la antiguedad del empleado. Además que sea factible calcularla con
    /// diferentes metodos. Por defecto Dias de Salario como la legislacion local lo exije, pero puede ser por un porcentaje
    /// del salario e incluso un valor fijo
    /// </summary>
    [DefaultClassOptions, NavigationItem("Recurso Humano"), ModelDefault("Caption", "Parámetro Aguinaldo"),
        Persistent(nameof(ParametroAguinaldo)), DefaultProperty("Desde")]
    [ImageName("ParametroAguinaldo")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ParametroAguinaldo : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ParametroAguinaldo(Session session)
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
        EMetodoAguinaldo metodo = EMetodoAguinaldo.DiasSalario;
        decimal valor = 0;
        bool activo = true;

        [Persistent("Empresa"), DbType("int"), XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [DbType("smallint"), Persistent("Desde"), XafDisplayName("Años Desde"),
            RuleValueComparison("ParametroAguinaldo.Desde >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        public int Desde
        {
            get => desde;
            set => SetPropertyValue(nameof(Desde), ref desde, value);
        }

        [DbType("smallint"), Persistent("Hasta"), XafDisplayName("Años Hasta"),
            RuleValueComparison("ParametroAguinaldo.Hasta >= Desde", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "Desde", ParametersMode.Expression,
            SkipNullOrEmptyValues = false)]
        public int Hasta
        {
            get => hasta;
            set => SetPropertyValue(nameof(Hasta), ref hasta, value);
        }

        [DbType("smallint"), Persistent("Metodo"), XafDisplayName("Método")]
        public EMetodoAguinaldo Metodo
        {
            get => metodo;
            set => SetPropertyValue(nameof(Metodo), ref metodo, value);
        }

        [DbType("numeric(6,2)"), Persistent("Valor"), XafDisplayName("Valor"), ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"),
            RuleValueComparison("ParametroAguinaldo.Valor >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false)]
        [ToolTip("Valor para calcular el aguinaldo. Corresponde al método seleccionado, Ejemplo: sí el método es días salario, Valor serán días")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [DbType("bit"), Persistent("Activo"), XafDisplayName("Activo"), RuleRequiredField("ParametroAguinaldo.Activo_Requerido", DefaultContexts.Save)]
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