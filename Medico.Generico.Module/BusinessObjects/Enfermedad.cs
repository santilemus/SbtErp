using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Es la clase para el objeto de negocios que corresponde al mantenimiento de enfermedades
    /// </summary>
	[DefaultClassOptions, CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Enfermedades")]
    [DevExpress.ExpressApp.DC.XafDefaultPropertyAttribute("Nombre")]
    [DevExpress.Persistent.Base.ImageNameAttribute(nameof(Enfermedad))]
    [DevExpress.Persistent.Base.NavigationItemAttribute("Salud")]
    [RuleIsReferenced("Enfermedad_Referencia", DefaultContexts.Delete, typeof(Enfermedad), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Enfermedad : XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
            EsGrupo = false;
        }
		
		private System.Boolean _esGrupo;
		private System.Boolean _activo;
		private System.String _comentario;
		private Enfermedad _categoria;
		private System.String _nombre;
		private System.String _codigoCie;
		public Enfermedad(DevExpress.Xpo.Session session)
		  : base(session)
		{
		}

        #region Propiedades
		[DevExpress.Xpo.Indexed(Unique = true)]
		[DevExpress.Xpo.SizeAttribute(6), VisibleInLookupListView(true), DbType("varchar(6)")]
		[RuleRequiredField("Diagnostico.Codigo_Requerido", "Save")]
		[RuleUniqueValue("Diagnostico.Codigo_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public System.String CodigoCie
		{
		  get
		  {
			return _codigoCie;
		  }
		  set
		  {
			SetPropertyValue("CodigoCie", ref _codigoCie, value);
		  }
		}
		[DevExpress.Xpo.SizeAttribute(300), DbType("varchar(300)")]
		[RuleRequiredField("Diagnostico.Nombre_Requerido", "Save")]
		[RuleUniqueValue("Diagnostico.Nombre_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
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

		[DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
		public Enfermedad Categoria
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
		[DevExpress.Persistent.Base.ToolTipAttribute("Indica sí el registro esta activo")]
		[RuleRequiredField("Diagnostico.Activo_Requerido", "Save")]
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

		[RuleRequiredField("Diagnostico.EsGrupo_Requerido", "Save")]
		public System.Boolean EsGrupo
		{
		  get
		  {
			return _esGrupo;
		  }
		  set
		  {
			SetPropertyValue("EsGrupo", ref _esGrupo, value);
		  }
		}

        #endregion

        #region Colecciones
        [Association("Enfermedad-FactorRiesgos"), XafDisplayName("Factores de Riesgo")]
        public XPCollection<FactorRiesgo> FactorRiesgos
        {
            get
            {
                return GetCollection<FactorRiesgo>(nameof(FactorRiesgos));
            }
        }
        #endregion
    }
}
