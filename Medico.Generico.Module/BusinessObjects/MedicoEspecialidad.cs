using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a MedicoEspecialidad. Es la clase para el objeto de negocios que corresponde a las especialidades del médico
    /// </summary>
	[NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Especialidades del Médico")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("med_especialista")]
    public class MedicoEspecialidad : XPObject
    {
        private Medico _medico;
        private MedicoLista especialidad;
        public MedicoEspecialidad(DevExpress.Xpo.Session session)
          : base(session)
        {
        }
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Especialidad")]
        [RuleRequiredField("MedicoEspecialidad.Especialidad_Requerido", "Save")]
        [DataSourceCriteria("[Categoria] == 3")]
        [ExplicitLoading]
        public MedicoLista Especialidad
        {
            get => especialidad;
            set => SetPropertyValue("Especialidad", ref especialidad, value);
        }

        [DevExpress.Xpo.AssociationAttribute("Especialidades-Medico")]
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
