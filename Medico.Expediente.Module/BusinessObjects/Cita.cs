using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using System;
using System.Linq;


/// <Notas>
/// Pendiente de filtrar la lista de medicos para las citas. El BO del cual hereda medico es empleado, entonces es necesario 
/// filtar solo a los empleados (médicos) con estado activo. Los estados se implementarán vía la lista de valores, entonces
/// es necesario primero tener definidos los valores de los estados de los empleados para hacer el filtro. 
/// Validar además que se asocien citas solo a medicos activos
/// </Notas>
namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects
{
    /// <summary>
    /// Objeto Persistente que corresponde a las citas. Es la clase para el objeto de negocios de citas
    /// </summary>
    [DefaultClassOptions]
    [DevExpress.ExpressApp.DC.XafDisplayName("Citas"), ImageName("proforma"), NavigationItem("Salud"), MapInheritance(MapInheritanceType.ParentTable)]
    [RuleIsReferenced("Cita_Referencia", DefaultContexts.Delete, typeof(Cita), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    //[RuleCombinationOfPropertiesIsUnique("Cita_PacienteFechaUnico", DefaultContexts.Save, "IdPaciente,Fecha", SkipNullOrEmptyValues = true)]
    [RuleCombinationOfPropertiesIsUnique("Cita_FechaNombreUnico", DefaultContexts.Save, "StartOn,Nombre", SkipNullOrEmptyValues = false)]
    public class Cita : CitaBase
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //Fecha = DateTime.Now;
            SePresento = false;
        }

        private Paciente _paciente;
        //private SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico _medico;
        private System.Boolean _sePresento;
        private System.String _motivo;
        private System.String _nombre;
        public Cita(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        //[DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        //[RuleRequiredField("Cita.Medico_Requerido", "Save")]
        //public SBT.Apps.Medico.Generico.Module.BusinessObjects.Medico Medico
        //{
        //    get => _medico;
        //    set => SetPropertyValue("Medico", ref _medico, value);
        //}

        [DevExpress.Xpo.AssociationAttribute("Paciente-Citas")]
        public Paciente Paciente
        {
            get
            {
                return _paciente;
            }
            set
            {
                var oldValue = _paciente;
                bool changed = SetPropertyValue("Paciente", ref _paciente, value);
                if (!IsLoading && !IsSaving && changed)
                    Nombre = value.NombreCompleto;
            }
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Cita.Nombre_Requerido", "Save"), Size(100), DbType("varchar(100)")]
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

        [DevExpress.Xpo.SizeAttribute(250)]
        [RuleRequiredField("Cita.Motivo_Requerido", "Save")]
        public System.String Motivo
        {
            get
            {
                return _motivo;
            }
            set
            {
                SetPropertyValue("Motivo", ref _motivo, value);
            }
        }

        [RuleRequiredField("Cita.SePresento_Requerido", "Save")]
        public System.Boolean SePresento
        {
            get
            {
                return _sePresento;
            }
            set
            {
                SetPropertyValue("SePresento", ref _sePresento, value);
            }
        }

    }
}
