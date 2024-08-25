using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// BO con los catálogos de apoyo del sistema de transmisión de Dte. 
    /// <br>
    /// En un solo BO y tabla de la base de datos se incorporan la mayoría de los catálogos del sistema de transmisión de Dte.
    /// Se utiliza para identificar cada catálogo la enumeración IC
    /// </br>
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Catálogos Dte"), NavigationItem("Facturación"), 
        CreatableItem(false), DefaultProperty(nameof(Concepto))]
    [Persistent(nameof(DteCatalogo)), FriendlyKeyProperty(nameof(Codigo))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DteCatalogo : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private ETipoDteCatalogo tipo;
        private string codigo;
        private string concepto;
        private string equivalente;
        private bool activo;

        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public DteCatalogo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Tipo = ETipoDteCatalogo.AmbienteDestino;
            Activo = true;
        }

        #region Propiedades

        [DisplayName("Catálogo Dte")]
        [Indexed(nameof(Codigo), Name = "idxDteCatalogo_TipoCodigo")]
        [ToolTip(@"Tipo de catálogo del sistema de transmisión de Dte")]
        public ETipoDteCatalogo Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [RuleRequiredField("DteCatalogo.Codigo_requerido", DefaultContexts.Save)]
        [DisplayName("Código")]
        [Size(4), DbType("varchar(2)")]
        [ToolTip("Código del registro para un Tipo de catálogo")]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue<string>(nameof(Codigo), ref codigo, value);
        }

        [RuleRequiredField("DteCatalogo.Concepto", DefaultContexts.Save)]
        [Size(60), DbType("varchar(60)")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue<string>(nameof(Concepto), ref concepto, value);
        }

        [Size(4), DbType("varchar(4)")]
        [ToolTip("Valor equivalente en el erp. Este es el valor que se utiliza en el erp y sirve para buscar el valor ce código que es el requerido para los Dte")]
        public string Equivalente
        {
            get => equivalente;
            set => SetPropertyValue<string>(nameof(Equivalente), ref equivalente, value);
        }

        public bool Activo
        {
            get => activo;
            set => SetPropertyValue<bool>(nameof(Activo), ref activo, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}