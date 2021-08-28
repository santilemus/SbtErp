using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Persistent.Validation;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde al Examen. Es la clase para el objeto de negocios que corresponde al mantenimiento de examenes
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Examenes")]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Salud")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    [DevExpress.Persistent.Base.ImageNameAttribute("list-info")]
    [RuleIsReferenced("Examen_Referencia", DefaultContexts.Delete, typeof(Examen), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Examen: XPObject
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
        }
		
		
		private System.String _comentario;
		private System.Boolean _activo;
		private MedicoLista _categoria;
		private System.String _nombre;
		public Examen(DevExpress.Xpo.Session session): base(session)
		{
		}
		[RuleRequiredField("Examen.Nombre_Requerido", "Save")]
		[RuleUniqueValue("Examen.Nombre_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		[DbType("varchar(100)"), Size(100)]
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

		[DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(true)]
		[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
		[RuleRequiredField("Examen.IdCategoria_Requerido", "Save")]
        [DataSourceCriteria("[Categoria] == 8")]
		public MedicoLista Categoria
		{
            get => _categoria;
		    set => SetPropertyValue("Categoria", ref _categoria, value);
		}

		[DevExpress.Xpo.SizeAttribute(250), DbType("varchar(250)")]
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
		[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
		[RuleRequiredField("Examen.Activo_Requerido", "Save")]
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
