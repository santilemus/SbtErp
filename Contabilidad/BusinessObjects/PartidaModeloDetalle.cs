using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    /// <summary>
    /// Contabilidad. BO con el detalle de las partidas modelo
    /// </summary>
    [ModelDefault("Caption", "Partida Modelo - Detalle"), NavigationItem(false), CreatableItem(false),
        Persistent("ConPartidaModeloDetalle")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PartidaModeloDetalle : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PartidaModeloDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        PartidaModelo partidaModelo;
        Catalogo catalogo;
        string concepto;
        decimal valorHaber;
        decimal valorDebe;
        string expresionItem;
        string valorItem;

        [Association("PartidaModelo-Detalles")]
        public PartidaModelo PartidaModelo
        {
            get => partidaModelo;
            set => SetPropertyValue(nameof(PartidaModelo), ref partidaModelo, value);
        }


        [DbType("int"), Persistent("Cuenta"), XafDisplayName("Cuenta")]
        public Catalogo Cuenta
        {
            get => catalogo;
            set => SetPropertyValue(nameof(Cuenta), ref catalogo, value);
        }


        [PersistentAlias("[Cuenta.Nombre]")]
        public string NombreCuenta
        {
            get { return Convert.ToString(EvaluateAlias(nameof(NombreCuenta))); }
        }


        [Size(150), DbType("varchar(150)"), Persistent("Concepto"), XafDisplayName("Concepto"),
            RuleRequiredField("PartidaModeloDetalle.Concepto_Requerido", "Save")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        [DbType("money"), Persistent("Debe"), XafDisplayName("Debe")]
        public decimal ValorDebe
        {
            get => valorDebe;
            set => SetPropertyValue(nameof(ValorDebe), ref valorDebe, value);
        }

        [DbType("money"), Persistent("Haber"), XafDisplayName("Haber")]
        public decimal ValorHaber
        {
            get => valorHaber;
            set => SetPropertyValue(nameof(ValorHaber), ref valorHaber, value);
        }


        [Size(100), Persistent("ExpresionItem"), XafDisplayName("Expresión Item")]
        [RuleRequiredField("PartidaModeloDetalle.ExpresionItem_Requerido", DefaultContexts.Save, TargetCriteria = "[PartidaModelo.TipoModelo] = 1",
            SkipNullOrEmptyValues = true)]
        public string ExpresionItem
        {
            get => expresionItem;
            set => SetPropertyValue(nameof(ExpresionItem), ref expresionItem, value);
        }

        
        [Size(50), Persistent(nameof(ValorItem)), XafDisplayName("Valor Item")]
        [RuleRequiredField("PartidaModeloDetalle.ValorItem_Requerido", DefaultContexts.Save, TargetCriteria = "[PartidaModelo.TipoModelo] = 1",
            SkipNullOrEmptyValues = true)]
        public string ValorItem
        {
            get => valorItem;
            set => SetPropertyValue(nameof(ValorItem), ref valorItem, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}