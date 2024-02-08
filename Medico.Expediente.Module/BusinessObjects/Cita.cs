using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;


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
    [RuleCombinationOfPropertiesIsUnique("Cita_FechaNombreUnico", DefaultContexts.Save, "StartOn,Subject", SkipNullOrEmptyValues = false)]
    public class Cita : CitaBase
    {
        /// <summary>
        /// Metodo para la inicialización de propiedades y/o objetos del BO. Se ejecuta una sola vez después de la creación del objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SePresento = false;
        }

        private Paciente _paciente;
        private System.Boolean _sePresento;
        public Cita(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        [DevExpress.Xpo.AssociationAttribute("Paciente-Citas"), ImmediatePostData(true)]
        [ExplicitLoading]
        public Paciente Paciente
        {
            get => _paciente;
            set
            {
                bool changed = SetPropertyValue("Paciente", ref _paciente, value);
                if (!IsLoading && !IsSaving && changed)
                    Subject = value.NombreCompleto;
            }
        }

        public System.Boolean SePresento
        {
            get => _sePresento;
            set => SetPropertyValue("SePresento", ref _sePresento, value);
        }

    }

}
