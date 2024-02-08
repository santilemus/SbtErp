using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano
    /// BO para las operaciones a realizar cuando se calcula o generan las planillas
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Operaciones"), NavigationItem("Recurso Humano"), DefaultProperty("Nombre")]
    [Persistent("PlaOperacion")]
    [ImageName(nameof(Operacion))]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Operacion : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Operacion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            Tipo = ETipoOperacion.Descuento;
            CalcularPara = ETipoCalculoOperacion.Siempre;
            Activo = true;
        }

        #region Propiedades

        Type tipoBO;
        string nombre;
        string formula;
        ETipoOperacion tipo = ETipoOperacion.Null;
        decimal valor;
        string descripcion;
        bool visible = true;
        ETipoCalculoOperacion calcularPara = ETipoCalculoOperacion.Siempre;
        bool activo = true;
        string comentario;

        [Size(40), DbType("varchar(40)"), XafDisplayName("Nombre"), RuleRequiredField("Operacion.Nombre_Requerido", "Save"), NonCloneable()]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
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

        //[Browsable(false)]
        //private Type CriteriaObjectType { get { return typeof(Empleado.Module.BusinessObjects.Empleado); } }

        [Size(1000), DbType("varchar(1000)"), XafDisplayName("Fórmula"), Persistent(nameof(Formula))]
        [ElementTypeProperty(nameof(TipoBO))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [VisibleInListView(false)]
        //[ModelDefault("Width", "50")]
        [ModelDefault("RowCount", "3")]
        public string Formula
        {
            get => formula;
            set => SetPropertyValue(nameof(Formula), ref formula, value);
        }

        [DbType("smallint"), XafDisplayName("Tipo")]
        public ETipoOperacion Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("numeric(12,2)"), XafDisplayName("Valor")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), ImmediatePostData(true)]
        public decimal Valor
        {
            get => valor;
            set => SetPropertyValue(nameof(Valor), ref valor, value);
        }

        [Size(60), DbType("varchar(60)"), XafDisplayName("Descripción"), NonCloneable()]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [DbType("bit"), XafDisplayName("Visible")]
        public bool Visible
        {
            get => visible;
            set => SetPropertyValue(nameof(Visible), ref visible, value);
        }

        [DbType("smallint"), XafDisplayName("Calcular Para")]
        public ETipoCalculoOperacion CalcularPara
        {
            get => calcularPara;
            set => SetPropertyValue(nameof(CalcularPara), ref calcularPara, value);
        }

        [DbType("bit"), XafDisplayName("Activo")]
        [DefaultValue(true)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [Size(400), DbType("varchar(400)"), XafDisplayName("Comentario")]
        [ModelDefault("RowCount", "4")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }
        #endregion


        #region Colecciones
        [Association("Operacion-TipoPlanillas"), DevExpress.Xpo.Aggregated, XafDisplayName("Tipo Planillas"), Index(0)]
        public XPCollection<OperacionTipoPlanilla> TipoPlanillas
        {
            get
            {
                return GetCollection<OperacionTipoPlanilla>(nameof(TipoPlanillas));
            }
        }

        [Association("Operacion-PlanillaOperaciones"), XafDisplayName("Planillas Operaciones")]
        public XPCollection<PlanillaDetalleOperacion> PlanillaDetalleOperaciones
        {
            get
            {
                return GetCollection<PlanillaDetalleOperacion>(nameof(PlanillaDetalleOperaciones));
            }
        }
        #endregion


        /// <summary>
        /// solo para probar la evaluacion de expresiones, porque se utilizara para calcular las operaciones de la planilla
        /// </summary>
        [Action(Caption = "Evaluar Expression", ImageName = "Attention", SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        public void Execute()
        {
            ExpressionEvaluator eval = new (TypeDescriptor.GetProperties(TipoBO), Formula);
            var bo = Session.GetObjectByKey(TipoBO, 1);
            if (bo != null)
                Valor = Convert.ToDecimal(eval.Evaluate(bo));
            else
                Valor = 0.0m;
        }


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}