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
using DevExpress.Persistent.Base.General;
using System.ComponentModel;
using System.Drawing;

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
    public class Medico: SBT.Apps.Empleado.Module.BusinessObjects.Empleado, IResource
    {
        public override void AfterConstruction()
        {
            /// <summary>
            /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
            /// </summary>
            base.AfterConstruction();
            color = Color.White.ToArgb();
        }

        string caption;
        private System.String numeroJVPM;
        [Persistent(nameof(Color))]
        private int color;

        public Medico(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Xpo.SizeAttribute(10), VisibleInLookupListView(true)]
        [RuleRequiredField("Medico.NumeroJVPM_Requerido", "Save")]
        [RuleUniqueValue("Medico.NumeroJVPM_Unico", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public System.String NumeroJVPM
        {
            get => numeroJVPM;
            set => SetPropertyValue(nameof(NumeroJVPM), ref numeroJVPM, value);
        }


        [PersistentAlias("[Oid]"), Browsable(false)]
        public object Id => EvaluateAlias(nameof(Id));
        
        [Size(100), DbType("varchar(100)"), XafDisplayName("SubTitulo")]
        public string Caption
        {
            get => caption;
            set => SetPropertyValue(nameof(Caption), ref caption, value);
        }

        [Browsable(false)]
        public int OleColor
        {
            get { return ColorTranslator.ToOle(Color.FromArgb(color)); }
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

        [Association("Medico-Citas", UseAssociationNameAsIntermediateTableName = true), XafDisplayName("Citas")]
        public XPCollection<CitaBase> Citas
        {
            get
            {
                return GetCollection<CitaBase>(nameof(Citas));
            }
        }
    }
}
