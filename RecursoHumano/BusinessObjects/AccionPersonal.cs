using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Empleado.Module.BusinessObjects;
using System;
using System.ComponentModel;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano. BO para implementar el mantenimiento de las acciones de personal
    /// **** LEEME ==> Faltan dos persistent alias: dias y horas de accion
    /// </summary>
    [DefaultClassOptions, ModelDefault("Caption", "Acción de Personal"), NavigationItem("Recurso Humano"), DefaultProperty("Empleado")]
    [Persistent(nameof(AccionPersonal))]
    [ImageName(nameof(AccionPersonal))]
    [CreatableItem(false)]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AccionPersonal : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AccionPersonal(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Tipo = ETipoAccionPersonal.Incapacidad;
            Estado = EEstadoAccionPersonal.Digitada;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado;
        ETipoAccionPersonal tipo = ETipoAccionPersonal.Otro;
        DateTime fechaAccion = DateTime.Now;
        [Persistent("CargoActual"), FetchOnly]
        Cargo cargoActual;
        Cargo cargoPropuesto;
        DateTime fechaInicio;
        DateTime fechaFin;
        decimal monto;
        [DbType("numeric(12, 2)"), Persistent("SalarioActual"), FetchOnly]
        decimal salarioActual;
        decimal salarioPropuesto;
        EmpresaUnidad unidadDestino;
        string observaciones;
        EEstadoAccionPersonal estado = EEstadoAccionPersonal.Digitada;
        [Size(25), DbType("varchar(25)"), Persistent("UsuarioAprobo")]
        string usuarioAprobo;
        [Size(150), DbType("varchar(150)"), Persistent("ComentarioAprobo")]
        string comentarioAprobo;

        [XafDisplayName("Empleado"), Persistent("Empleado"), RuleRequiredField("AccionPersonal.Empleado_Requerido", "Save")]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
            set
            {
                var oldValue = Empleado;
                bool changed = SetPropertyValue(nameof(Empleado), ref empleado, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    salarioActual = oldValue.Salario;
                    cargoActual = oldValue.Cargo;
                }
            }
        }


        [DbType("smallint"), XafDisplayName("Tipo Acción")]
        public ETipoAccionPersonal Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [DbType("datetime"), XafDisplayName("Fecha Acción"), RuleRequiredField("AccionPersonal.FechaAccion_Requerido", "Save")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaAccion
        {
            get => fechaAccion;
            set => SetPropertyValue(nameof(FechaAccion), ref fechaAccion, value);
        }

        [DbType("datetime2"), XafDisplayName("Fecha Inicio")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }

        /// <summary>
        /// Fecha Fin de la accion de personal. Solo es valida cuando el tipo de accion requiere que exista una fecha de
        /// finalizacion. Los casos son: 1 => Permisos sin goce de sueldo, 3 => Suspension de Contrato, 9 => Vacacion, 10 => Maternidad, 
        /// 11 => Inasistencia, 14 => Licencia con Goce de Sueldo, 15 => Amonestacion
        /// </summary>
        [DbType("datetime2"), XafDisplayName("Fecha Fin")]
        [RuleValueComparison("AccionPersonal.FechaFin >= FechaInicio", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "[FechaInicio]", ParametersMode.Expression, TargetCriteria = "[Tipo] In (1, 3, 9, 10, 11, 14, 15)")]
        [ModelDefault("DisplayFormat", "{0:G}"), ModelDefault("EditMask", "G")]
        public DateTime FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }

        [XafDisplayName("Cargo Actual"), PersistentAlias("cargoActual")]
        public Cargo CargoActual
        {
            get => cargoActual;
        }

        [XafDisplayName("Cargo Propuesto")]
        public Cargo CargoPropuesto
        {
            get => cargoPropuesto;
            set => SetPropertyValue(nameof(CargoPropuesto), ref cargoPropuesto, value);
        }

        [DbType("numeric(12,2)"), XafDisplayName("Monto")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        [XafDisplayName("Salario Actual"), PersistentAlias("salarioActual")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SalarioActual
        {
            get => salarioActual;
        }

        /// <summary>
        /// SalarioPropuesto cuando el Tipo de la Accion es 6 => Aumento de Salario 
        /// </summary>
        [DbType("numeric(12, 2)"), XafDisplayName("Salario Propuesto")]
        [RuleValueComparison("AccionPersonal.SalarioPropuesto > SalarioActual", DefaultContexts.Save, ValueComparisonType.GreaterThan,
            "[SalarioActual]", ParametersMode.Expression, TargetCriteria = "[Tipo] = 6)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SalarioPropuesto
        {
            get => salarioPropuesto;
            set => SetPropertyValue(nameof(SalarioPropuesto), ref salarioPropuesto, value);
        }

        [XafDisplayName("Unidad Destino")]
        public EmpresaUnidad UnidadDestino
        {
            get => unidadDestino;
            set => SetPropertyValue(nameof(UnidadDestino), ref unidadDestino, value);
        }

        [Size(250), DbType("varchar(250)"), XafDisplayName("Observaciones")]
        public string Observaciones
        {
            get => observaciones;
            set => SetPropertyValue(nameof(Observaciones), ref observaciones, value);
        }

        [DbType("smallint"), XafDisplayName("Estado")]
        public EEstadoAccionPersonal Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }
        [XafDisplayName("Usuario Aprobó"), PersistentAlias(nameof(usuarioAprobo))]
        public string UsuarioAprobo
        {
            get => usuarioAprobo;
        }
        [XafDisplayName("Comentario Aprobación"), PersistentAlias(nameof(comentarioAprobo))]
        public string ComentarioAprobo
        {
            get => comentarioAprobo;
        }

        #endregion 

        /// <summary>
        /// Aprobar la Accion de Personal. Este metodo ejecuta las actualizaciones de acuerdo al tipo de
        /// accion de personal; incluyendo la actualizacion del estado de la accion
        /// </summary>
        /// [Action(Caption = "Aprobar", ImageName = "Aceptar")]
        public void Aprobar(string AComentario)
        {
            Session.BeginTransaction();
            try
            {
                comentarioAprobo = AComentario;
                usuarioAprobo = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
                estado = EEstadoAccionPersonal.Aprobada;
                switch (Tipo)
                {
                    case ETipoAccionPersonal.LicenciaSSueldo:
                        CambiarEstadoEmpleado("EMPL03");
                        break;
                    case ETipoAccionPersonal.SuspensionContrato:
                        CambiarEstadoEmpleado("EMPL09");
                        break;
                    case ETipoAccionPersonal.Traslado:
                        Empleado.Cargo = CargoPropuesto;
                        break;
                    case ETipoAccionPersonal.Promocion:
                        Empleado.Cargo = CargoPropuesto;
                        break;
                    case ETipoAccionPersonal.AumentoSalario:
                        Empleado.Salario = SalarioPropuesto;
                        break;
                    case ETipoAccionPersonal.Despido:
                        CambiarEstadoEmpleado("EMPL08");
                        Empleado.FechaRetiro = FechaFin;
                        break;
                    case ETipoAccionPersonal.Renuncia:
                        CambiarEstadoEmpleado("EMPL02");
                        Empleado.FechaRetiro = FechaFin;
                        break;
                    case ETipoAccionPersonal.Maternidad:
                        CambiarEstadoEmpleado("EMPL06");
                        break;
                }
                Session.CommitTransaction();
                Session.Reload(this);
            }
            catch
            {
                Session.RollbackTransaction();
                throw;
            }
        }

        private void CambiarEstadoEmpleado(string Codigo)
        {
            var obj = Session.GetObjectByKey<Listas>(Codigo);
            if (obj != null)
                Empleado.Estado = obj;
        }

        /// <summary>
        /// Rechazar la Accion de Personal.Cambia el estado de la accion de Personal a rechazada
        /// </summary>
        //[Action(Caption = "Rechazar", ConfirmationMessage = "Esta seguro?", ImageName = "Cancelar", AutoCommit = true)]
        public void Rechazar(string AComentario)
        {
            Session.BeginTransaction();
            try
            {
                comentarioAprobo = AComentario;
                usuarioAprobo = DevExpress.ExpressApp.SecuritySystem.CurrentUserName;
                estado = EEstadoAccionPersonal.Rechazada;
                Session.Save(this);
                if (Session.InTransaction)
                    Session.CommitTransaction();
            }
            catch
            {
                if (Session.InTransaction)
                    Session.RollbackTransaction();
                throw;
            }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}