using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    [NavigationItem(false)]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Medicos del Paciente")]
    [DevExpress.Persistent.Base.ImageNameAttribute("medico")]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    public class PacienteMedico : XPObjectBaseBO
    {

        private Medico.Generico.Module.BusinessObjects.Medico _medico;
        private Paciente _paciente;
        private System.DateTime _inicioDeRelacion;
        private System.Boolean _activo;
        public PacienteMedico(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Medico")]
        [ExplicitLoading]
        public Medico.Generico.Module.BusinessObjects.Medico Medico
        {
            get => _medico;
            set => SetPropertyValue("Medico", ref _medico, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Inicio de Relación")]
        public System.DateTime InicioDeRelacion
        {
            get
            {
                return _inicioDeRelacion;
            }
            set
            {
                SetPropertyValue("InicioDeRelacion", ref _inicioDeRelacion, value);
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
        [DevExpress.Xpo.AssociationAttribute("Medicos-Paciente")]
        public Paciente Paciente
        {
            get
            {
                return _paciente;
            }
            set
            {
                SetPropertyValue("Paciente", ref _paciente, value);
            }
        }

    }
}
