using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a las garantías presentadas por terceros que son clientes para obtener crédito
    /// </summary>
    /// <remarks>
    /// NOTA: Se hereda de XPobjectBaseBO por la propiedad Empresa
    /// </remarks>
    [ModelDefault("Caption", "Tercero Garantía"), NavigationItem(false), CreatableItem(false),
        DefaultProperty(nameof(Descripcion)), Persistent(nameof(TerceroGarantia))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TerceroGarantia : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TerceroGarantia(Session session)
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
        DateTime? fechaVence;
        decimal valor;
        DateTime fechaInicio;
        string descripcion;
        Listas tipo;
        Tercero cliente;

        [Association("Tercero-Garantias"), XafDisplayName("Cliente"), Index(0)]
        public Tercero Cliente
        {
            get => cliente;
            set => SetPropertyValue(nameof(Cliente), ref cliente, value);
        }

        [XafDisplayName("Empresa"), Persistent(nameof(Empresa)), Index(1), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        /// <summary>
        /// Tipo de Credito. Pendiente de agregar en el BO, la clasificacion de tipos de garantia
        /// y completar el datasource aqui
        /// </summary>
        [XafDisplayName("Tipo Garantía"), VisibleInLookupListView(true), Index(2)]
        [DataSourceCriteria("[Categoria] == 3 And [Activo] == True")]
        [ExplicitLoading]
        public Listas Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }


        [Size(100), DbType("varchar(100)"), XafDisplayName("Descripción"), Index(3),
            RuleRequiredField("TerceroGarantia.Descripcion_Requerido", DefaultContexts.Save)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Inicio Garantía"), Index(4)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        [DbType("numeric(14,2)"), XafDisplayName("Valor"), Index(5),
            RuleValueComparison("TerceroGarantia.Valor >= 0", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Vence Garantía"), Index(6)]
        [RuleValueComparison("TerceroGarantia.FechaVence > FechaInicio", DefaultContexts.Save, ValueComparisonType.GreaterThan,
            "[FechaInicio]", ParametersMode.Expression, SkipNullOrEmptyValues = true)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime? FechaVence
        {
            get => fechaVence;
            set => SetPropertyValue(nameof(FechaVence), ref fechaVence, value);
        }

        [PersistentAlias("([FechaVence] Is Null) || (Now() <= [FechaVence])"), XafDisplayName("Vigente"), Index(7)]
        public bool Vigente => Convert.ToBoolean(EvaluateAlias(nameof(Vigente)));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}