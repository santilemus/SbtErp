using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a Listas de Valores del sistema Médico.
    /// </summary>
	[DefaultClassOptions]
    [DevExpress.ExpressApp.DC.XafDisplayName("Listas de Valores")]
    [DevExpress.ExpressApp.DC.XafDefaultProperty("Nombre")]
    [DevExpress.Persistent.Base.NavigationItem("Salud")]
    [DevExpress.Persistent.Base.ImageName("list-key")]
    [Persistent("MedLista")]
    [RuleIsReferenced("MedicoListas_Referencia", DefaultContexts.Delete, typeof(MedicoListas), nameof(Codigo),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class MedicoListas: XPCustomBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;

        }
		
		private CategoriaListaMedico _categoria = CategoriaListaMedico.CategoriaExamen;
		private System.Boolean _activo;
		private System.String _comentario;
		private System.String _nombre;
		private System.String _codigo;
		public MedicoListas(DevExpress.Xpo.Session session)
		  : base(session)
		{
		}
		[DevExpress.Xpo.SizeAttribute(10)]
		[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
		[DevExpress.Xpo.KeyAttribute]
		[RuleRequiredField("MedicoListas.Codigo_Requerido", "Save")]
		[RuleUniqueValue("MedicoListas.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public System.String Codigo
		{
		  get
		  {
			return _codigo;
		  }
		  set
		  {
			SetPropertyValue("CodigoCie", ref _codigo, value);
		  }
		}
		[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
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

		[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
		[DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Categoría")]
		[RuleRequiredField("MedicoListas.IdCategoria_Requerido", "Save")]
		public CategoriaListaMedico Categoria
		{
            get => _categoria;
		    set => SetPropertyValue("Categoria", ref _categoria, value);
		}

		[DevExpress.Xpo.SizeAttribute(250)]
		[DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
		[DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
		[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
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
		[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
		[RuleRequiredField("MedicoListas.Activo_Requerido", "Save")]
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
