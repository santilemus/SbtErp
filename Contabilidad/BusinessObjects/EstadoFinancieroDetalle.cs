using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SBT.Apps.Contabilidad.BusinessObjects;
using SBT.Apps.Base.Module.BusinessObjects;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Core;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [NavigationItem(false), CreatableItem(false), ModelDefault("Caption", "Estado Financiero Detalle")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EstadoFinancieroDetalle : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EstadoFinancieroDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        string formula2;
        string nombre2;
        Catalogo cuenta2;
        string formula1;
        string nombre1;
        Catalogo cuenta1;
        Type tipoBO;
        EstadoFinanciero estadoFinanciero;

        [Association("EstadoFinanciero-Detalles"), XafDisplayName("Estado Financiero")]
        public EstadoFinanciero EstadoFinanciero
        {
            get => estadoFinanciero;
            set => SetPropertyValue(nameof(EstadoFinanciero), ref estadoFinanciero, value);
        }

        /// <summary>
        /// El BO para el cual se implementa la formula
        /// </summary>
        /// <remarks>
        /// Mas info
        /// 1. EditorAliases.TypePropertyEditor implementa la lista de seleccion de los BO
        ///    Ver: https://docs.devexpress.com/eXpressAppFramework/113579/concepts/business-model-design/data-types-supported-by-built-in-editors/type-properties
        /// 2. ValueConverter, para hacer la propiedad persistente se tiliza la conversion a string y guardar el nombre del BO en la bd, incluyendo el namespace
        /// </remarks>
        [XafDisplayName("Tipo Business Object"), Persistent(nameof(TipoBO))]
        [EditorAlias(EditorAliases.TypePropertyEditor)]
        [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter)), ImmediatePostData]
        [DataSourceCriteria("")]
        public Type TipoBO
        {
            get => tipoBO;
            set => SetPropertyValue(nameof(TipoBO), ref tipoBO, value);
        }

        [XafDisplayName("Cuenta Columna 1")]
        public Catalogo Cuenta1
        {
            get => cuenta1;
            set => SetPropertyValue(nameof(Cuenta1), ref cuenta1, value);
        }
        
        [Size(150), DbType("varchar(150)"), XafDisplayName("Nombre Columna 1")]
        [RuleRequiredField("EstadoFinancieroDetalle.Nombre1_Requerido", "Save")]
        public string Nombre1
        {
            get => nombre1;
            set => SetPropertyValue(nameof(Nombre1), ref nombre1, value);
        }

        [Size(1000), DbType("varchar(1000)"), XafDisplayName("Fórmula 1"), Persistent(nameof(Formula1))]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false)]
        //[ModelDefault("Width", "50")]
        [ModelDefault("RowCount", "3")]
        public string Formula1
        {
            get => formula1;
            set => SetPropertyValue(nameof(Formula1), ref formula1, value);
        }

        [XafDisplayName("Cuenta Columna 2")]
        public Catalogo Cuenta2
        {
            get => cuenta2;
            set => SetPropertyValue(nameof(Cuenta2), ref cuenta2, value);
        }

        [Size(150), DbType("varchar(150)"), XafDisplayName("Nombre Columna 2")]
        public string Nombre2
        {
            get => nombre2;
            set => SetPropertyValue(nameof(Nombre2), ref nombre2, value);
        }

        [Size(1000), DbType("varchar(1000)"), XafDisplayName("Fórmula 2"), Persistent(nameof(Formula2))]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false)]
        //[ModelDefault("Width", "50")]
        [ModelDefault("RowCount", "3")]
        public string Formula2
        {
            get => formula2;
            set => SetPropertyValue(nameof(Formula2), ref formula2, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}