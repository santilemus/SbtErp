using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Activo.Module.BusinessObjects
{
    [DefaultClassOptions, ModelDefault("Caption", "Seguro de Activo"), NavigationItem("Activo Fijo")]
    [DefaultProperty(nameof(NumeroPoliza)), Persistent(nameof(ActivoSeguro))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActivoSeguro : XPObjectCustom
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActivoSeguro(Session session)
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
        DateTime finCobertura;
        DateTime inicioCobertura;
        string numeroPoliza;
        Tercero.Module.BusinessObjects.Tercero aseguradora;
        DateTime fechaPoliza;
        Empresa empresa;

        [XafDisplayName("Empresa"), Browsable(false)]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [XafDisplayName("Aseguradora")]
        public Tercero.Module.BusinessObjects.Tercero Aseguradora
        {
            get => aseguradora;
            set => SetPropertyValue(nameof(Aseguradora), ref aseguradora, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Póliza")]
        [RuleRequiredField("ActivoSeguro.FechaPoliza_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:d}"), ModelDefault("EditMask", "d")]
        public DateTime FechaPoliza
        {
            get => fechaPoliza;
            set => SetPropertyValue(nameof(FechaPoliza), ref fechaPoliza, value);
        }

        [Size(20), DbType("varchar(20)"), XafDisplayName("Número Póliza")]
        public string NumeroPoliza
        {
            get => numeroPoliza;
            set => SetPropertyValue(nameof(NumeroPoliza), ref numeroPoliza, value);
        }

        [DbType("datetime"), XafDisplayName("Inicio de Cobertura")]
        [RuleValueComparison("ActivoSeguro.InicioCobertura >= FechaPoliza", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "[FechaPoliza]", ParametersMode.Expression)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "g")]
        public DateTime InicioCobertura
        {
            get => inicioCobertura;
            set => SetPropertyValue(nameof(InicioCobertura), ref inicioCobertura, value);
        }

        [DbType("datetime"), XafDisplayName("Fin de Cobertura")]
        [RuleValueComparison("ActivoSeguro.FinCobertura >= InicioCobertura", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
               "[InicioCobertura]", ParametersMode.Expression)]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "g")]
        public DateTime FinCobertura
        {
            get => finCobertura;
            set => SetPropertyValue(nameof(FinCobertura), ref finCobertura, value);
        }

        [Size(250), DbType("varchar(250)"), XafDisplayName("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }
        #endregion

        #region Colecciones
        [Association("ActivoSeguro-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<ActivoSeguroDetalle> Detalles => GetCollection<ActivoSeguroDetalle>(nameof(Detalles));

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}