using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Catalogos")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Listas Valores")]
    [DevExpress.Persistent.Base.ImageNameAttribute("list")]
    [Persistent(nameof(Listas))]
    [RuleIsReferenced("Listas_Referencia", DefaultContexts.Delete, typeof(Listas), nameof(Codigo),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Listas : XPCustomObject
    {
        private System.Boolean _activo = true;
        private CategoriaLista _categoria = CategoriaLista.EstadoEmpleado;
        private System.String _comentario;
        private System.String _nombre;
        private System.String _codigo;
        public Listas(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [RuleUniqueValue("Listas.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [DevExpress.Xpo.Size(12), DbType("varchar(12)"), Index(0)]
        [DevExpress.Xpo.KeyAttribute, VisibleInLookupListView(true)]
        [RuleRequiredField("Listas.Codigo_Requerido", "Save")]
        public System.String Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                SetPropertyValue("Codigo", ref _codigo, value);
            }
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Nombre"), DbType("varchar(100)")]
        [Index(1)]
        [RuleRequiredField("Lista.NombreRequerido", DefaultContexts.Save, "Nombre es requerido")]
        public System.String Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                SetPropertyValue("Nombre", ref _nombre, value);
            }
        }
        [Index(2)]
        [RuleRequiredField("Lista.CategoriaRequerida", DefaultContexts.Save, "Categoría es requerida")]
        public CategoriaLista Categoria
        {
            get
            {
                return _categoria;
            }
            set
            {
                SetPropertyValue("Categoria", ref _categoria, value);
            }
        }

        [DevExpress.Xpo.Size(250), DbType("varchar(250)"), Index(3), VisibleInListView(false)]
        public System.String Comentario
        {
            get
            {
                return _comentario;
            }
            set
            {
                SetPropertyValue("Comentario", ref _comentario, value);
            }
        }
        [Index(4)]
        [RuleRequiredField("Lista.Activa", DefaultContexts.Save, "Activa es requerida")]
        public System.Boolean Activo
        {
            get
            {
                return _activo;
            }
            set
            {
                SetPropertyValue("Activo", ref _activo, value);
            }
        }

    }
}
