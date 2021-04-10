using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a las presentaciones que pueden tener los productos
    /// 
    /// </summary>
    
    [DefaultClassOptions, ModelDefault("Caption", "Presentacion"), DefaultProperty("Nombre"), NavigationItem("Inventario"), 
        CreatableItem(false), Persistent("ProPresentacion")]
    [ImageName("Presentacion")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Presentacion : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Presentacion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
            Unidades = 0;
            Defecto = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        string codigo;
        [DbType("varchar(12)"), Persistent("Codigo")]
        [Size(12), Index(0), XafDisplayName("Código"), RuleUniqueValue("Presentacion.Codigo_Unico", DefaultContexts.Save,
           CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue("Codigo", ref codigo, value);
        }

        string nombre;
        [DbType("varchar(50)"), Persistent("Nombre")]
        [Size(25), XafDisplayName("Nombre"), Index(1), RuleRequiredField("Presentacion.Nombre_Requerido", DefaultContexts.Save)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue("Nombre", ref nombre, value);
        }

        decimal unidades;
        [DbType("numeric(12,2)"), Persistent(nameof(Unidades))]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        [XafDisplayName("Unidades Inventario"), Index(2), RuleRequiredField("Presentacion.UnidInventa_Requerido", DefaultContexts.Save), VisibleInLookupListView(false),
           ToolTip("Cantidad de Unidades de Inventario para el presentacion")]
        public decimal Unidades
        {
            get => unidades;
            set => SetPropertyValue(nameof(Unidades), ref unidades, value);
        }

        bool activo;
        [DbType("bit"), Persistent("Activo")]
        [XafDisplayName("Activo"), Index(3), RuleRequiredField("Presentacion.Activo_Requerido", "Save")]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue("Activo", ref activo, value);
        }

        bool defecto;
        [DbType("bit"), Persistent("Defecto")]
        [XafDisplayName("Defecto"), Index(4), RuleRequiredField("Presentacion.Defecto_Requerido", "Save"), ToolTip("Presentacion o presentacion por defecto")]
        public bool Defecto
        {
            get => defecto;
            set => SetPropertyValue(nameof(Defecto), ref defecto, value);
        }

        #endregion

        #region Colecciones
        [Association("Presentacion-Productos")]
        public XPCollection<Producto> Productos
        {
            get
            {
                return GetCollection<Producto>("Productos");
            }
        }
        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}