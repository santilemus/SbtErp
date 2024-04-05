using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System.ComponentModel;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    /// <summary>
    /// TerceroDocumento BO
    /// BO que corresponde a los documentos de los terceros. Es para permitir vincular a un tercero, que puede ser persona
    /// natural o jurídica, más de un documento. Por ejemplo: NIT, DUI, PASAPORTE. En el caso de personas jurídicas, aplican
    /// NIT y cualquier otro documento vinculado a su personería
    /// </summary>
    [ModelDefault("Caption", "Tercero Documentos"), NavigationItem(false), DefaultProperty(nameof(Numero))]
    [Persistent(nameof(TerceroDocumento)), CreatableItem(false)]
    [ImageName("user_id-info")]
    [RuleCombinationOfPropertiesIsUnique("TerceroDocumento_TipoDocumento", DefaultContexts.Save, "Tercero,Numero,Tipo", IncludeCurrentObject = false)]
    // nuevas agregadas el 29/03/2024
    [RuleCriteria("TerceroDocumento.Nit", DefaultContexts.Save, "len(trim(Numero)) = 14", TargetCriteria = "Tipo.Codigo == 'NIT'",
        CustomMessageTemplate = "Longitud de NIT no válido", SkipNullOrEmptyValues = true)]
    [RuleCriteria("TerceroDocumento.Dui", DefaultContexts.Save, "len(trim(Numero)) = 9", TargetCriteria = "Tipo.Codigo == 'DUI'",
        CustomMessageTemplate = "Longitud de DUI no válido", SkipNullOrEmptyValues = true)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TerceroDocumento : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TerceroDocumento(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Vigente = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        Tercero tercero;
        private System.DateTime fechaEmision;
        private System.Boolean vigente;
        private System.String lugarEmision;
        private System.String nombre;
        private System.String numero;
        private Listas tipo;

        [ImmediatePostData(true), Index(0), VisibleInLookupListView(true)]
        [RuleRequiredField("TerceroDocumento.Tipo_Requerido", DefaultContexts.Save, "Tipo Documento es requerido")]
        [DataSourceCriteria("[Categoria] = 10"), XafDisplayName("Tipo")]
        [ExplicitLoading]
        public Listas Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [Size(14), DbType("varchar(14)"), XafDisplayName("Número"), Index(2), VisibleInReports(true)]
        [RuleRequiredField("TerceroDocumento.Numero_Requerido", "Save"), Indexed("Tipo", Name = "idxTerceroNoDocumento")]
        [RuleRegularExpression("TerceroDocumento.Nit_Valido", DefaultContexts.Save, 
            "^(0?[0-1]{1}|9)(0?[0-9]{1})(\\d{2})(0?[1-9]|[12]\\d|3[01])(0?[1-9]|1[0-2])\\d{2}\\d{3}\\d{1}$", 
            TargetCriteria = "[Tipo.Codigo] == 'NIT'", SkipNullOrEmptyValues = true, CustomMessageTemplate = "Número de NIT no válido")]
        [RuleRegularExpression("TerceroDocumento.Dui_Valido", DefaultContexts.Save, "^(0?[0-9]{8})(0?[0-9]{1})$", TargetCriteria = "[Tipo.Codigo] == 'DUI'",
            SkipNullOrEmptyValues = true, CustomMessageTemplate = "Número de DUI no válido")]
        public System.String Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        /// <summary>
        /// Nombre del tercero en el documento. Cuando es diferente del nombre registrado en el sistema
        /// </summary>
        [Size(80), DbType("varchar(80)"), ToolTip("Nombre del tercero según el documento"), Index(3)]
        public System.String Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [XafDisplayName("Lugar Emisión"), ToolTip("Lugar de emisión del documento"), VisibleInListView(false),
            VisibleInLookupListView(false), DbType("varchar(100)"), Index(4)]
        [RuleRequiredField("TerceroDocumento.LugarEmision_Requerido", "Save", TargetCriteria = "[Tipo.Codigo] In ('DUI', 'PAS', 'RES')",
            ResultType = ValidationResultType.Information)]
        public System.String LugarEmision
        {
            get => lugarEmision;
            set => SetPropertyValue(nameof(LugarEmision), ref lugarEmision, value);
        }

        [XafDisplayName("Fecha Emisión"), VisibleInLookupListView(false)]
        [RuleRequiredField("TerceroDocumento.FechaEmision_Requerido", "Save", TargetCriteria = "[Tipo.Codigo] In ('DUI', 'PAS', 'RES')")]
        [RuleRequiredField("TerceroDocumento.LugarEmision_Information", "Save",
            TargetCriteria = "!([Tipo.Codigo] In ('DUI', 'PAS', 'RES'))", ResultType = ValidationResultType.Information)]
        [Index(5), VisibleInListView(false)]
        public System.DateTime FechaEmision
        {
            get => fechaEmision;
            set => SetPropertyValue(nameof(FechaEmision), ref fechaEmision, value);
        }

        [VisibleInLookupListView(false), Index(6)]
        public System.Boolean Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }


        [Association("Tercero-Documentos"), XafDisplayName("Tercero"), VisibleInListView(false), VisibleInLookupListView(false), Index(7)]
        public Tercero Tercero
        {
            get => tercero;
            set => SetPropertyValue(nameof(Tercero), ref tercero, value);
        }

        #endregion

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

}