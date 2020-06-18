using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using System;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.SystemModule;


namespace SBT.Apps.Empleado.Module.BusinessObjects
{
    [DefaultClassOptions]
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
            Empresa = EmpresaDeSesion();
        }

        AFP aFP;
        EDiaDescanso diaDescanso;
        Banco banco;
        Cargo cargo;
        private EmpresaUnidad unidad;
        private Empresa empresa;
        private Listas estado;
        private System.Decimal salario;
        private TipoSalario tipoSalario;
        private System.String titulo;
        private System.String numeroCarne;
        private System.Boolean pensionado;
        private System.String numeroCuenta;
        private ETipoCuentaBanco tipoCuenta = ETipoCuentaBanco.Ahorros;
        private ZonaGeografica nacionalidad;
        private TipoContrato tipoContrato = TipoContrato.Indefinido;

        public Empleado(DevExpress.Xpo.Session session)
          : base(session)
        {
        }

        #region Propiedades

        [XafDisplayName("Empresa"), ImmediatePostData(true), VisibleInLookupListView(false), VisibleInListView(false), VisibleInDetailView(true),
            VisibleInReports(true)]
        [RuleRequiredField("Empleado.CodEmpresa_requerido", DefaultContexts.Save, "Empresa es requerida")]
        public Empresa Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
        }

        [ModelDefault("Caption", "Unidad"), ImmediatePostData(true), VisibleInLookupListView(false), VisibleInDetailView(true),
            VisibleInReports(true)]
        [RuleRequiredField("Empleado.CodUnidad_requerido", DefaultContexts.Save, "Unidad asignada es requerida")]
        public EmpresaUnidad Unidad
        {
            get => unidad;
            set => SetPropertyValue(nameof(Unidad), ref unidad, value);
        }


        [XafDisplayName("Cargo"), RuleRequiredField("Empleado.Cargo_Requerido", DefaultContexts.Save), ImmediatePostData(true)]
        [DevExpress.Xpo.NoForeignKey]
        public Cargo Cargo
        {
            get => cargo;
            set
            {
                var oldValue = Cargo;
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
        [RuleRequiredField("Empleado.TipoSalario_requerido", DefaultContexts.Save, "Tipo de Salario es requerido")]
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
        [RuleRequiredField("Empleado.TipoContrato_Requerido", DefaultContexts.Save, "TipoContrato es Requerido")]
        public TipoContrato TipoContrato
        {
            get => tipoContrato;
            set => SetPropertyValue(nameof(TipoContrato), ref tipoContrato, value);
        }

        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Estado Empleado")]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [RuleRequiredField("Empleado.Estado_requerido", DefaultContexts.Save, "Estado del empleado es Requerido")]
        [DataSourceCriteria("Categoria = 9")]  // categoria corresponde a los estados del empleado
        public Listas Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DataSourceCriteria("ZonaPadre is null and Activa = true")]
        public ZonaGeografica Nacionalidad
        {
            get => nacionalidad;
            set => SetPropertyValue(nameof(Nacionalidad), ref nacionalidad, value);
        }


        [XafDisplayName("Banco")]
        [DataSourceCriteria("[Roles][[IdRole] = 2]")]
        public Banco Banco
        {
            get => banco;
            set => SetPropertyValue(nameof(Banco), ref banco, value);
        }

        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public ETipoCuentaBanco TipoCuenta
        {
            get => tipoCuenta;
            set => SetPropertyValue(nameof(TipoCuenta), ref tipoCuenta, value);
        }

        [DevExpress.Xpo.SizeAttribute(14)]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Número Cuenta")]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        public System.String NumeroCuenta
        {
            get => numeroCuenta;
            set => SetPropertyValue(nameof(NumeroCuenta), ref numeroCuenta, value);
        }
        [RuleRequiredField("Empleado.Pensionado_requerido", DefaultContexts.Save, "Estado Pensionado es requerido")]
        public System.Boolean Pensionado
        {
            get => pensionado;
            set => SetPropertyValue(nameof(Pensionado), ref pensionado, value);
        }

        [DevExpress.Xpo.SizeAttribute(10)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Número Carné")]
        public System.String NumeroCarne
        {
            get => numeroCarne;
            set => SetPropertyValue(nameof(NumeroCarne), ref numeroCarne, value);
        }

        [DevExpress.Xpo.SizeAttribute(12)]
        [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
        [DevExpress.Persistent.Base.ImmediatePostDataAttribute]
        public System.String Titulo
        {
            get => titulo;
            set => SetPropertyValue(nameof(Titulo), ref titulo, value);
        }

        [XafDisplayName("Día Descanso"), DbType("smallint")]
        public EDiaDescanso DiaDescanso
        {
            get => diaDescanso;
            set => SetPropertyValue(nameof(DiaDescanso), ref diaDescanso, value);
        }

        
        [XafDisplayName("AFP")]
        public AFP AFP
        {
            get => aFP;
            set => SetPropertyValue(nameof(AFP), ref aFP, value);
        }

        [PersistentAlias("Iif([TipoSalario] != 1, [Salario], [Salario] * DateDiffDay(LocalDateTimeThisYear(), LocalDateTimeNextYear()) / 12.00)")]      
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal SalarioMes
        {
            get { return Convert.ToDecimal(EvaluateAlias(nameof(SalarioMes))); }
        }
        


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

    }
}
