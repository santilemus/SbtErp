using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.BusinessObjects;
using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Core;
using System.Collections.Generic;
using DevExpress.Data.Filtering;
using System.ComponentModel;

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
            Tipo = ETipoOperacion.Cargo;

            //parameters = new List<XafFilterParameter>();
            //parameters.Add(new XafFilterParameter("Oid", typeof(int)));
            //parameters.Add(new XafFilterParameter("FechaDesde", typeof(DateTime)));
            //parameters.Add(new XafFilterParameter("FechaHasta", typeof(DateTime)));
        }

        #region Propiedades
        PartidaModelo partidaModelo;
        Catalogo catalogo;
        string concepto;
        //decimal valorHaber;
        //decimal valorDebe;
        //string expresionItem;
        //string valorItem;
        private Type tipoBO;
        private string formula;
        private string criteria;
        private ETipoOperacion tipoOperacion;
        //private string codigoCuenta;

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

        /*
         * QUITAR EL COMENTARIO CUANDO SE ESTE LISTO PARA PROBAR ESTA FUNCIONALIDAD 
         * La idea es recuperar el código de cuenta a utilizar vía el BO que se está contabilizando (TipoBO)
         * Ejemplos: Cuentas de Activo (gasto, depreciación, activo), cuenta asociada a productos, cuenta contable de cuenta de bancos
        [Size(500), DbType("varchar(500)"), System.ComponentModel.DisplayName("Código Cuenta")]
        [ToolTip("Expresión para obtener la cuenta cuando es dinámica, es decir; se obtiene desde el TipoBO, por ejemplo las cuentas de un activo fijo")]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false)]
        [ModelDefault("RowCount", "3")]
        public string CodigoCuenta
        {
            get => codigoCuenta;
            set => SetPropertyValue<string>(nameof(CodigoCuenta), ref codigoCuenta, value));
        }
        */

        [PersistentAlias("[Cuenta.Nombre]")]
        public string NombreCuenta
        {
            get { return Convert.ToString(EvaluateAlias(nameof(NombreCuenta))); }
        }


        [Size(500), DbType("varchar(500)"), Persistent("Concepto"), XafDisplayName("Concepto"),
            RuleRequiredField("PartidaModeloDetalle.Concepto_Requerido", "Save")]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false)]
        [ModelDefault("RowCount", "3")]
        public string Concepto
        {
            get => concepto;
            set => SetPropertyValue(nameof(Concepto), ref concepto, value);
        }

        //[DbType("money"), Persistent("Debe"), XafDisplayName("Debe")]
        //public decimal ValorDebe
        //{
        //    get => valorDebe;
        //    set => SetPropertyValue(nameof(ValorDebe), ref valorDebe, value);
        //}

        //[DbType("money"), Persistent("Haber"), XafDisplayName("Haber")]
        //public decimal ValorHaber
        //{
        //    get => valorHaber;
        //    set => SetPropertyValue(nameof(ValorHaber), ref valorHaber, value);
        //}


        //[Size(100), Persistent("ExpresionItem"), XafDisplayName("Expresión Item")]
        //[RuleRequiredField("PartidaModeloDetalle.ExpresionItem_Requerido", DefaultContexts.Save, TargetCriteria = "[PartidaModelo.TipoModelo] = 1",
        //    SkipNullOrEmptyValues = true)]
        //public string ExpresionItem
        //{
        //    get => expresionItem;
        //    set => SetPropertyValue(nameof(ExpresionItem), ref expresionItem, value);
        //}


        //[Size(50), Persistent(nameof(ValorItem)), XafDisplayName("Valor Item")]
        //[RuleRequiredField("PartidaModeloDetalle.ValorItem_Requerido", DefaultContexts.Save, TargetCriteria = "[PartidaModelo.TipoModelo] = 1",
        //    SkipNullOrEmptyValues = true)]
        //public string ValorItem
        //{
        //    get => valorItem;
        //    set => SetPropertyValue(nameof(ValorItem), ref valorItem, value);
        //}

        #region Propiedades en prueba
        /// <summary>
        /// El BO para el cual se implementa la formula
        /// </summary>
        /// <remarks>
        /// Mas info
        /// 1. EditorAliases.TypePropertyEditor implementa la lista de seleccion de los BO
        ///    Ver: https://docs.devexpress.com/eXpressAppFramework/113579/concepts/business-model-design/data-types-supported-by-built-in-editors/type-properties
        /// 2. ValueConverter, para hacer la propiedad persistente se utiliza la conversion a string y guardar el nombre del BO en la bd, incluyendo el namespace
        /// </remarks>
        [XafDisplayName("Tipo BO"), Persistent(nameof(TipoBO))]
        [EditorAlias(EditorAliases.TypePropertyEditor)]
        [ValueConverter(typeof(DevExpress.ExpressApp.Utils.TypeToStringConverter)), ImmediatePostData]
        //[DataSourceCriteriaProperty(nameof(BOPermitidos))]
        [Size(150), DbType("varchar(150)")]
        public Type TipoBO
        {
            get => tipoBO;
            set => SetPropertyValue(nameof(TipoBO), ref tipoBO, value);
        }

        [Size(1000), DbType("varchar(1000)"), XafDisplayName("Fórmula"), Persistent(nameof(Formula))]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false)]
        [ModelDefault("RowCount", "3")]
        public string Formula
        {
            get => formula;
            set => SetPropertyValue(nameof(Formula), ref formula, value);
        }

        [Size(1000), DbType("varchar(1000)"), XafDisplayName("Condición"), Persistent(nameof(Criteria))]
        [ElementTypeProperty(nameof(TipoBO))]
        //[EditorAlias("PopupFilterPropertyEditor")]
        //[EditorAlias(EditorAliases.CriteriaPropertyEditor)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        [VisibleInListView(false)]
        [ModelDefault("RowCount", "3")]
        [CriteriaOptions(nameof(TipoBO))] //, ParametersMemberName = nameof("Nombre Clas IEnumerable con los parametros personalizados para el filtro))]
        [ImmediatePostData(true)]
        public string Criteria
        {
            get => criteria;
            set => SetPropertyValue(nameof(Criteria), ref criteria, value);
        }

        public ETipoOperacion Tipo
        {
            get => tipoOperacion;
            set => SetPropertyValue(nameof(Tipo), ref tipoOperacion, value);
        }

        [Browsable(false)]
        public CriteriaOperator BOPermitidos
        {
             get => CriteriaOperator.FromLambda<Type>(x => x.Name.Contains("Venta"));
        }

        #endregion

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        //IList<IFilterParameter> parameters;
        //List<XafFilterParameter> parameters;
    }
}