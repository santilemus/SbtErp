using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
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
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Editors;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Producto.Module.BusinessObjects;

namespace SBT.Apps.Facturacion.Module.BusinessObjects
{
    /// <summary>
    /// BO con las formulas o expresiones para calcular los tributos
    /// </summary>
    
    [MapInheritance(MapInheritanceType.ParentTable)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TributoRegla : Tributo
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TributoRegla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        string formula;

        [Browsable(false)]
        public Type CriteriaObjectType
        {
            get { return typeof(Venta); }
        }

        [Size(400), DbType("varchar(400)"), RuleRequiredField("Tributo.Formula_Requerido", "Save"), XafDisplayName("Fórmula")]
        [ElementTypeProperty("CriteriaObjectType")]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [ModelDefault("Width", "50")]
        [Index(5), VisibleInListView(false)]
        public string Formula
        {
            get => formula;
            set => SetPropertyValue(nameof(Formula), ref formula, value);
        }
        #endregion

        #region Colecciones

        [Association("Tributo-VentaResumenTributos"), XafDisplayName("Factura Tributos"), Index(0)]
        public XPCollection<VentaResumenTributo> VentaTributos => GetCollection<VentaResumenTributo>(nameof(VentaTributos));

        #endregion

        #region metodos
        /// <summary>
        /// Evaluar la formula y retornar el valor calculado
        /// </summary>
        /// <param name="theObject"></param>
        /// <returns></returns>
        public decimal EvaluarFormula(object theObject)
        {
            ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Venta)), Formula);
            return Math.Round(Convert.ToDecimal(eval.Evaluate(theObject)), 2);
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}