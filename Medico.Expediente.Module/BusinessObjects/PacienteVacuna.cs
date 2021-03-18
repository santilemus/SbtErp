using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    [NavigationItem(false)]
    [DevExpress.Persistent.Base.CreatableItemAttribute(false)]
    [DevExpress.Persistent.Base.ImageNameAttribute("vacunas")]
    [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Vacunas")]
    [Persistent(nameof(PacienteVacuna))]
    public class PacienteVacuna : XPObjectBaseBO
    {
        private Medicamento _vacuna;
        private System.String _lote;
        private Paciente _paciente;
        private System.String _comentario;
        private System.String _marca;
        private Tercero.Module.BusinessObjects.Tercero _farmaceutica;
        private System.Int16 _noRefuerzo;
        private System.Int16 _noDosis;
        private TerminologiaAnatomica _parteDeCuerpo;
        private System.Boolean _aplicada;
        private System.DateTime _proximaDosis;
        private System.DateTime _fecha;
        public PacienteVacuna(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [ExplicitLoading]
        public Medicamento Vacuna
        {
            get
            {
                return _vacuna;
            }
            set
            {
                SetPropertyValue("Vacuna", ref _vacuna, value);
            }
        }
        public System.DateTime Fecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                SetPropertyValue("Fecha", ref _fecha, value);
            }
        }
        public System.DateTime ProximaDosis
        {
            get
            {
                return _proximaDosis;
            }
            set
            {
                SetPropertyValue("ProximaDosis", ref _proximaDosis, value);
            }
        }
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.Boolean Aplicada
        {
            get
            {
                return _aplicada;
            }
            set
            {
                SetPropertyValue("Aplicada", ref _aplicada, value);
            }
        }

        [ExplicitLoading]
        public TerminologiaAnatomica ParteDeCuerpo
        {
            get
            {
                return _parteDeCuerpo;
            }
            set
            {
                SetPropertyValue("ParteDeCuerpo", ref _parteDeCuerpo, value);
            }
        }
        public System.Int16 NoDosis
        {
            get
            {
                return _noDosis;
            }
            set
            {
                SetPropertyValue("NoDosis", ref _noDosis, value);
            }
        }
        public System.Int16 NoRefuerzo
        {
            get
            {
                return _noRefuerzo;
            }
            set
            {
                SetPropertyValue("NoRefuerzo", ref _noRefuerzo, value);
            }
        }

        [ExplicitLoading]
        public Tercero.Module.BusinessObjects.Tercero Farmaceutica
        {
            get
            {
                return _farmaceutica;
            }
            set
            {
                SetPropertyValue("Farmaceutica", ref _farmaceutica, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(50)]
        public System.String Marca
        {
            get
            {
                return _marca;
            }
            set
            {
                SetPropertyValue("Marca", ref _marca, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(25)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String Lote
        {
            get
            {
                return _lote;
            }
            set
            {
                SetPropertyValue("Lote", ref _lote, value);
            }
        }
        [DevExpress.Xpo.SizeAttribute(200)]
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
        [DevExpress.Xpo.AssociationAttribute("Vacunas-Paciente")]
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
