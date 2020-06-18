using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Xpo;
using System.Linq;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Model;
using SBT.Apps.Empleado.Module.BusinessObjects;



namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los médicos. Es la clase para el objeto de negocios que corresponde al mantenimiento de médicos
    /// </summary>
	[DefaultClassOptions]
    [ModelDefault("Caption", "Medico"), XafDefaultProperty("NombreCompleto"), ImageName("medico")]
    [NavigationItem("Recurso Humano")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [RuleIsReferenced("Medico_Referencia", DefaultContexts.Delete, typeof(Medico), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    public class Medico: SBT.Apps.Empleado.Module.BusinessObjects.Empleado
    {
        public override void AfterConstruction()
        {
            /// <summary>
            /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
            /// </summary>
            base.AfterConstruction();

        }
		
		private System.String _numeroJVPM;
		public Medico(DevExpress.Xpo.Session session)
		  : base(session)
		{
		}

		[DevExpress.Xpo.SizeAttribute(10), VisibleInLookupListView(true)]
		[RuleRequiredField("Medico.NumeroJVPM_Requerido", "Save")]
		[RuleUniqueValue("Medico.NumeroJVPM_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
		public System.String NumeroJVPM
		{
		  get
		  {
			return _numeroJVPM;
		  }
		  set
		  {
			SetPropertyValue("NumeroJVPM", ref _numeroJVPM, value);
		  }
		}
		[Association("Especialidades-Medico"), DevExpress.Xpo.Aggregated]
		public XPCollection<MedicoEspecialidad> Especialidades
		{
		  get
		  {
			return GetCollection<MedicoEspecialidad>("Especialidades");
		  }
		}
		
		[Association("Consultorios-Medico"), DevExpress.Xpo.Aggregated]
		public XPCollection<MedicoConsultorio> Consultorios
		{
		  get
		  {
			return GetCollection<MedicoConsultorio>("Consultorios");
		  }
		}

        [Association("Medico-Citas"), XafDisplayName("Citas"), DevExpress.Xpo.Aggregated]
        public XPCollection<CitaBase> Citas
        {
            get
            {
                return GetCollection<CitaBase>(nameof(Citas));
            }
        }
    }
}
