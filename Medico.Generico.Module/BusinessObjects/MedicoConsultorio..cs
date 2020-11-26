using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Model;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a los consultorios donde labora el médico Es la clase para el objeto de negocios que corresponde al mantenimiento consultorios del medico
    /// </summary>
    [NavigationItem(false)]
    [CreatableItem(false), ImageName("ccmedico"), ModelDefault("Caption", "Consultorio")]
    public class MedicoConsultorio: XPObjectBaseBO
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Activo = true;
        }
		
        private EmpresaUnidad _consultorio;
		private Medico _medico;
		private System.Boolean _activo;
		public MedicoConsultorio(DevExpress.Xpo.Session session)
		  : base(session)
		{
		}

		[RuleRequiredField("MedicoConsultorio.Consultorio_Requerido", "Save")]
		public EmpresaUnidad Consultorio
		{
		  get
		  {
			return _consultorio;
		  }
		  set
		  {
			SetPropertyValue("Consultorio", ref _consultorio, value);
		  }
		}

		[RuleRequiredField("MedicoConsultorio.Activo_Requerido", "Save")]
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
		[DevExpress.Xpo.AssociationAttribute("Consultorios-Medico")]
		public Medico Medico
		{
		  get
		  {
			return _medico;
		  }
		  set
		  {
			SetPropertyValue("Medico", ref _medico, value);
		  }
		}

    }
}
