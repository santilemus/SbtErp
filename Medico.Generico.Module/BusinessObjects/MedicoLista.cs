using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a Listas de Valores del sistema Médico.
    /// </summary>
	[DefaultClassOptions, CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayName("Listas Valores - Medico")]
    [DevExpress.ExpressApp.DC.XafDefaultProperty("Nombre")]
    [DevExpress.Persistent.Base.NavigationItem("Salud")]
    [DevExpress.Persistent.Base.ImageName("list-key")]
    [Persistent("MedLista")]
    [RuleIsReferenced("MedicoListas_Referencia", DefaultContexts.Delete, typeof(MedicoLista), nameof(Codigo),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class MedicoLista : XPCustomObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
            Categoria = CategoriaListaMedico.TipoExamen;
        }

        private CategoriaListaMedico _categoria = CategoriaListaMedico.TipoExamen;
        private System.Boolean _activo;
        private System.String _comentario;
        private System.String _nombre;
        private System.String _codigo;
        public MedicoLista(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.Xpo.SizeAttribute(10)]
        [DevExpress.Xpo.KeyAttribute]
        [RuleRequiredField("MedicoListas.Codigo_Requerido", "Save")]
        [RuleUniqueValue("MedicoListas.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [DbType("varchar(100)"), Size(100)]
        [RuleRequiredField("MedicoListas.Nombre_Requerido", "Save")]
        [RuleUniqueValue("MedicoListas.Nombre_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Categoría")]
        public CategoriaListaMedico Categoria
        {
            get => _categoria;
            set => SetPropertyValue("Categoria", ref _categoria, value);
        }

        [DevExpress.Xpo.SizeAttribute(250), DbType("varchar(250)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
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

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
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


        #region Colecciones
        [Association("MedicoListas-TerminologiaAnatomicas")]
        public XPCollection<TerminologiaAnatomica> TerminologiaAnatomicas
        {
            get
            {
                return GetCollection<TerminologiaAnatomica>(nameof(TerminologiaAnatomicas));
            }
        }
        #endregion

    }
}
