using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;


namespace SBT.Apps.Banco.Module.BusinessObjects
{
    /// <summary>
    /// Bancos. BO para las chequeras correspondientes a las cuentas bancarias de las cuales se emiten cheques
    /// </summary>
    /// 
    [ModelDefault("Caption", "Chequera"), NavigationItem(false), Persistent(nameof(BancoChequera)), CreatableItem(false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class BancoChequera : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BancoChequera(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string serie;
        int numeroActual;
        int numeroFin;
        int numeroInicio;
        DateTime fechaFin;
        DateTime fechaInicio;
        BancoCuenta numeroCuenta;

        [Association("BancoCuenta-Chequeras"), Persistent("NumeroCuenta")]
        public BancoCuenta NumeroCuenta
        {
            get => numeroCuenta;
            set => SetPropertyValue(nameof(NumeroCuenta), ref numeroCuenta, value);
        }

        [DbType("datetime"), Persistent("FechaInicio"), XafDisplayName("Fecha Inicio"), RuleRequiredField("Chequera.FechaInicio_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        [DbType("datetime"), Persistent("FechaFin"), XafDisplayName("Fecha Fin")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        [RuleValueComparison("Chequera.FechaFin >= FechaInicio", DefaultContexts.Save, ValueComparisonType.GreaterThan, "FechaInicio",
            ParametersMode.Expression, SkipNullOrEmptyValues = true)]
        public DateTime FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        [DbType("int"), Persistent("NumeroInicio"), XafDisplayName("No Primer Cheque"), RuleRequiredField("Chequera.NumeroInicio_Requerido", "Save")]
        public int NumeroInicio
        {
            get => numeroInicio;
            set => SetPropertyValue(nameof(NumeroInicio), ref numeroInicio, value);
        }

        [DbType("int"), Persistent("NumeroFin"), XafDisplayName("No Ultimo Cheque"), RuleRequiredField("Chequera.NumeroFin_Requerido", "Save")]
        public int NumeroFin
        {
            get => numeroFin;
            set => SetPropertyValue(nameof(NumeroFin), ref numeroFin, value);
        }

        [DbType("int"), Persistent("NumeroActual"), XafDisplayName("No Cheque Actual")]
        [RuleRange("Chequera.NumeroActual >= NumeroInicio y <= NumeroFin", DefaultContexts.Save, "NumeroInicio", "NumeroFin", ParametersMode.Expression,
            SkipNullOrEmptyValues = false)]
        public int NumeroActual
        {
            get => numeroActual;
            set => SetPropertyValue(nameof(NumeroActual), ref numeroActual, value);
        }


        [Size(8), DbType("varchar(8)"), Persistent("Serie"), XafDisplayName("No Serie")]
        public string Serie
        {
            get => serie;
            set => SetPropertyValue(nameof(Serie), ref serie, value);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}