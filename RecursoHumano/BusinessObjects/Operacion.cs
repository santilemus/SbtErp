using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano
    /// BO para las operaciones a realizar cuando se calcula o generan las planillas
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Operaciones"), NavigationItem("Recurso Humano"), DefaultProperty("Nombre")]
    [Persistent("PlaOperacion")]
    //[ImageName("BO_Contact")]
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
        }

        #region Propiedades

        string nombre;
        string formula;
        ETipoOperacion tipo = ETipoOperacion.Null;
        decimal valor = 0.0m;
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

        [Browsable(false)]
        public Type CriteriaObjectType { get { return typeof(Empleado.Module.BusinessObjects.Empleado); } }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Nombre Función"), Persistent(nameof(Formula))]
        [ElementTypeProperty("CriteriaObjectType")]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [ModelDefault("Width", "50"), VisibleInListView(false)]
        public string Formula
        {
            get => formula;
            set => SetPropertyValue(nameof(Formula), ref formula, value);
        }

        [DbType("smallint"), XafDisplayName("Tipo"), RuleRequiredField("Operacion.Tipo_Requerido", "Save")]
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
        [EditorAlias(EditorAliases.TypePropertyEditor)]
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

        [DbType("smallint"), XafDisplayName("Calcular Para"), RuleRequiredField("Operacion.CalcularPara_Requerido", "Save")]
        public ETipoCalculoOperacion CalcularPara
        {
            get => calcularPara;
            set => SetPropertyValue(nameof(CalcularPara), ref calcularPara, value);
        }

        [DbType("bit"), XafDisplayName("Activo"), RuleRequiredField("Operacion.Activo_Requerido", DefaultContexts.Save)]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [Size(200), DbType("varchar(200)"), XafDisplayName("Comentario")]
        public string Comentario
        {
            get => comentario;
            set => SetPropertyValue(nameof(Comentario), ref comentario, value);
        }
        #endregion
        

        #region Colecciones
        [Association("Operacion-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Tipo Planillas"), Index(0)]
        public XPCollection<TipoPlanillaOperacion> Detalles
        {
            get
            {
                return GetCollection<TipoPlanillaOperacion>(nameof(Detalles));
            }
        }
        #endregion


        /// <summary>
        /// solo para probar la evaluacion de expresiones, porque se utilizara para calcular las operaciones de la planilla
        /// </summary>
        [Action(Caption = "Evaluar Expression", ImageName = "Attention", SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        public void Execute()
        {
            ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(typeof(Empleado.Module.BusinessObjects.Empleado)), Comentario);
            Empleado.Module.BusinessObjects.Empleado emple = new Empleado.Module.BusinessObjects.Empleado(Session);
            emple.Salario = 2500.00m;
            Valor = Convert.ToDecimal(eval.Evaluate(emple));
        }


        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}