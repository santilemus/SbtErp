using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// BO para listas genericas de objetos que se utilizan en cualquier módulo del sistema
    /// </summary>
    
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
        private string codigoAlterno;

        public Listas(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        /// <summary>
        /// Código del objeto. Es el Oid o llave primaria en la base de datos
        /// </summary>
        [RuleUniqueValue("Listas.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [DevExpress.Xpo.Size(12), DbType("varchar(12)"), Index(0)]
        [DevExpress.Xpo.KeyAttribute, VisibleInLookupListView(true)]
        [RuleRequiredField("Listas.Codigo_Requerido", "Save")]
        public System.String Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }
        /// <summary>
        /// Concepto o nombre comprensivo para el usuario
        /// </summary>
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Nombre"), DbType("varchar(100)")]
        [Index(1)]
        [RuleRequiredField("Lista.NombreRequerido", DefaultContexts.Save, "Nombre es requerido")]
        public System.String Nombre
        {
            get => _nombre;
            set => SetPropertyValue(nameof(Nombre), ref _nombre, value);
        }

        /// <summary>
        /// Categoria de la lista. Clasifica los elementos para agruparlos de acuerdo a una misma categoría y 
        /// filtrar cuando se va utilizar la lista
        /// </summary>
        [Index(2)]
        [RuleRequiredField("Lista.CategoriaRequerida", DefaultContexts.Save, "Categoría es requerida")]
        public CategoriaLista Categoria
        {
            get => _categoria;
            set => SetPropertyValue(nameof(Categoria), ref _categoria, value);
        }

        /// <summary>
        /// Codigo alterno, cuando es requerido para otros propósitos por ejemplo estándarización con reguladores
        /// </summary>
        [Index(3), DisplayName("Código Alterno")]
        [Indexed(Name = "idxCodigoAlterno_Listas")]
        public string CodigoAlterno
        {
            get => codigoAlterno;
            set => SetPropertyValue(nameof(CodigoAlterno), ref codigoAlterno, value);
        }

        /// <summary>
        /// Comentario del registro
        /// </summary>
        [DevExpress.Xpo.Size(250), DbType("varchar(250)"), Index(4), VisibleInListView(false)]
        public System.String Comentario
        {
            get => _comentario;
            set => SetPropertyValue(nameof(Comentario), ref _comentario, value);
        }

        //Indica si el registro o elemento de la lista está activo
        [Index(5)]
        [RuleRequiredField("Lista.Activa", DefaultContexts.Save, "Activa es requerida")]
        public System.Boolean Activo
        {
            get => _activo;
            set => SetPropertyValue(nameof(Activo), ref _activo, value);
        }

    }
}
