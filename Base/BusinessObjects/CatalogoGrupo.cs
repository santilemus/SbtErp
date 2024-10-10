using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace SBT.Apps.Contabilidad.BusinessObjects
{
    /// <summary>
    /// Catálogo para agrupar cuentas contables para informes, en particular los estados financieros
    /// </summary>
    [DefaultClassOptions, CreatableItem(false), NavigationItem("Contabilidad"), DefaultProperty(nameof(Nombre))]
    [Persistent("ConCatalogoGrupo")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CatalogoGrupo : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string nombre;
        private int orden;
        private bool activo;
        private ETipoCuentaCatalogo tipo;

        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public CatalogoGrupo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Tipo = ETipoCuentaCatalogo.Activo;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        [Size(80), DbType("varchar(80)"), Index(0)]
        [RuleRequiredField("CatalogoGrupo.Nombre_requerido", DefaultContexts.Save)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [DbType("smallint"), Index(1)]
        public int Orden
        {
            get => orden;
            set => SetPropertyValue(nameof(Orden), ref orden, value);
        }

        [DbType("smallint"), Index(2)]
        public ETipoCuentaCatalogo Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("bit"), Index(3)]
        [RuleRequiredField("CatalogoGrupo.Activo_requerido", DefaultContexts.Save)]
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