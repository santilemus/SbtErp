using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using System;


namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false)]
    [ModelDefault("Caption", "Empleado"), XafDisplayName("Empleado"), NavigationItem("Recurso Humano"), XafDefaultProperty("NombreCompleto"),
        Persistent("Empleado"), ImageName("employees")]
    [RuleIsReferenced("Empleado_Referencia", DefaultContexts.Delete, typeof(Empleado), nameof(Oid),
        MessageTemplateMustBeReferenced = "Para borrar el objeto '{TargetObject}', debe estar seguro que no es utilizado (referenciado) en ningún lugar.",
        InvertResult = true, FoundObjectMessageFormat = "'{0}'", FoundObjectMessagesSeparator = ";")]
    [ListViewFilter("Todos", "")]
    [ListViewFilter("Activos", "!([Estado.Codigo] In ('EMPL02', 'EMPL08', 'EMPL09'))")]
    [ListViewFilter("Retirados y Despedidos", "(Estado.Codigo In ('EMPL02', 'EMPL08')")]
    [ListViewFilter("Contrato Suspendido", "Estado.Codigo == 'EMPL09'")]
    [ListViewFilter("SubContrato", "[Estado.Codigo] == 'EMPL07'")]
    public class Empleado : Persona
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Salario = 0.0m;
            TipoSalario = TipoSalario.Mensual;
            Pensionado = false;
            TipoContrato = TipoContrato.Indefinido;
            TipoCuenta = ETipoCuentaBanco.Ahorros;
            cargo = null;
        }

        //private AFP aFP;
        private EDiaDescanso diaDescanso;
        //private Banco banco;
        private Cargo cargo;
        private EmpresaUnidad unidad;
        private Empresa empresa;
        private Listas estado;
        private System.Decimal salario;
        private TipoSalario tipoSalario;
        private System.String titulo;
        private System.String numeroCarne;
        private System.Boolean pensionado;
        private System.String numeroCuenta;
        private ETipoCuentaBanco tipoCuenta;
        //private ZonaGeografica nacionalidad;
        private TipoContrato tipoContrato;

        public Empleado(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        #region Propiedades

        [XafDisplayName("Empresa"), VisibleInReports(true), VisibleInListView(false), VisibleInDetailView(false)]
        [RuleRequiredField("Empleado.CodEmpresa_requerido", DefaultContexts.Save, "Empresa es requerida")]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [ModelDefault("Caption", "Unidad"), VisibleInLookupListView(false), VisibleInDetailView(true),
            VisibleInReports(true)]
        [RuleRequiredField("Empleado.CodUnidad_requerido", DefaultContexts.Save, "Unidad asignada es requerida")]
        [DataSourceCriteria("[Empresa.Oid] == '@This.Empresa'")]
        [ExplicitLoading]
        public EmpresaUnidad Unidad
        {
            get => unidad;
            set => SetPropertyValue(nameof(Unidad), ref unidad, value);
        }

        /// <summary>
        /// Cargo del Empleado
        /// </summary>
        /// <remarks>En casos como este donde hay codigo en el setter que depende del BO referenciado (Cargo), 
        /// debe incluir el atributo [ImmediatePostData] para que ejecute correctamente las asignaciones del setter
        /// y NO DEBE usar el atributo [ExplicitLoading]
        /// </remarks>

        [XafDisplayName("Cargo"), RuleRequiredField("Empleado.Cargo_Requerido", DefaultContexts.Save), ImmediatePostData(true)]
        [ExplicitLoading]
        public Cargo Cargo
        {
            get => cargo;
            set
            {
                bool changed = SetPropertyValue(nameof(Cargo), ref cargo, value);
                if (!IsLoading && !IsSaving && changed)
                {
                    TipoSalario = value.TipoSalario;
                    Salario = value.Salario;
                    TipoContrato = value.TipoContrato;
                }
            }
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        public TipoSalario TipoSalario
        {
            get => tipoSalario;
            set => SetPropertyValue(nameof(TipoSalario), ref tipoSalario, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleValueComparison("Empleado.Salario_Mayor o Igual a Cero", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public System.Decimal Salario
        {
            get => salario;
            set => SetPropertyValue(nameof(Salario), ref salario, value);

        }

        [ModelDefault("Caption", "Tipo Contrato"), VisibleInDetailView(true), VisibleInLookupListView(false), VisibleInReports(true)]
        public TipoContrato TipoContrato
        {
            get => tipoContrato;
            set => SetPropertyValue(nameof(TipoContrato), ref tipoContrato, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Estado Empleado")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        //[DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Empleado.Estado_requerido", DefaultContexts.Save, "Estado del empleado es Requerido")]
        [DataSourceCriteria("Categoria = 9")]  // categoria corresponde a los estados del empleado
        public Listas Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DataSourceCriteria("[ZonaPadre] is null and [Activa] = true")]
        //[ExplicitLoading]
        [Delayed(true)]
        public ZonaGeografica Nacionalidad
        {
            get => GetDelayedPropertyValue<ZonaGeografica>(nameof(Nacionalidad));
            set => SetDelayedPropertyValue<ZonaGeografica>(nameof(Nacionalidad), value);
        }

        [XafDisplayName("Banco"), VisibleInListView(false)]
        //[ExplicitLoading]
        [Delayed(true)]
        public Banco Banco
        {
            get => GetDelayedPropertyValue<Banco>(nameof(Banco));
            set => SetDelayedPropertyValue<Banco>(nameof(Banco), value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), VisibleInListView(false)]
        public ETipoCuentaBanco TipoCuenta
        {
            get => tipoCuenta;
            set => SetPropertyValue(nameof(TipoCuenta), ref tipoCuenta, value);
        }

        [DevExpress.Xpo.SizeAttribute(14), DbType("varchar(14)")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Número Cuenta")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false), VisibleInListView(false)]
        public System.String NumeroCuenta
        {
            get => numeroCuenta;
            set => SetPropertyValue(nameof(NumeroCuenta), ref numeroCuenta, value);
        }
        [VisibleInListView(false)]
        public System.Boolean Pensionado
        {
            get => pensionado;
            set => SetPropertyValue(nameof(Pensionado), ref pensionado, value);
        }

        [DevExpress.Xpo.SizeAttribute(10), DbType("varchar(10)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Número Carné"), VisibleInListView(false)]
        public System.String NumeroCarne
        {
            get => numeroCarne;
            set => SetPropertyValue(nameof(NumeroCarne), ref numeroCarne, value);
        }

        [DevExpress.Xpo.SizeAttribute(12), DbType("varchar(12)")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute, VisibleInListView(false)]
        public System.String Titulo
        {
            get => titulo;
            set => SetPropertyValue(nameof(Titulo), ref titulo, value);
        }

        [XafDisplayName("Día Descanso"), DbType("smallint"), VisibleInListView(false)]
        public EDiaDescanso DiaDescanso
        {
            get => diaDescanso;
            set => SetPropertyValue(nameof(DiaDescanso), ref diaDescanso, value);
        }


        [XafDisplayName("AFP"), VisibleInListView(false)]
        [Delayed(true)]
        public AFP AFP
        {
            get => GetDelayedPropertyValue<AFP>(nameof(AFP));
            set => SetDelayedPropertyValue<AFP>(nameof(AFP), value);
        }

        [PersistentAlias("Iif([TipoSalario] != 1, [Salario], [Salario] * DateDiffDay(LocalDateTimeThisYear(), LocalDateTimeNextYear()) / 12.00)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2"), VisibleInListView(false)]
        public decimal SalarioMes
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(SalarioMes))); }
        }

        //[Browsable(false)]
        //public DateTime? FechaCumpleAnioContrato => FechaIngreso.Year < DateTime.Now.Year ? (DateTime?)new DateTime(DateTime.Now.Year, FechaIngreso.Month, FechaIngreso.Day) : null;

        //[Persistent("Usuario"), XafDisplayName("Usuario Sistema")]
        //public Usuario Usuario
        //{
        //    get => usuario;
        //    set => SetPropertyValue(nameof(Usuario), ref usuario, value);
        //}

        #endregion

        #region Colecciones

        [DevExpress.Xpo.AssociationAttribute("Profesiones-Empleado")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Profesiones"), DevExpress.Xpo.Aggregated]
        public XPCollection<EmpleadoProfesion> Profesiones
        {
            get
            {
                return GetCollection<EmpleadoProfesion>("Profesiones");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Capacitaciones-Empleado")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Capacitaciones"), DevExpress.Xpo.Aggregated]
        public XPCollection<EmpleadoCapacitacion> Capacitaciones
        {
            get
            {
                return GetCollection<EmpleadoCapacitacion>("Capacitaciones");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Membresias-Empleado")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Membresías"), DevExpress.Xpo.Aggregated]
        public XPCollection<EmpleadoMembresia> Membresias
        {
            get
            {
                return GetCollection<EmpleadoMembresia>("Membresias");
            }
        }
        [DevExpress.Xpo.AssociationAttribute("Parientes-Empleado"), DevExpress.Xpo.Aggregated]
        public XPCollection<EmpleadoPariente> Parientes
        {
            get
            {
                return GetCollection<EmpleadoPariente>("Parientes");
            }
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Probar en el expression editor
        /// </summary>
        /// <param name="AMeses"></param>
        /// <returns></returns>
        public decimal IngresoPromedioN(int AMeses)
        {
            return Salario * AMeses;

        }

        #endregion

    }


}
