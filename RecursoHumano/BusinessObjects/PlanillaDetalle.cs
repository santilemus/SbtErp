using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Empleado.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    /// <summary>
    /// Recurso Humano
    /// BO para el detalle de los empleados incluidos en una planilla calculada por el sistema
    /// </summary>

    [DefaultClassOptions, ModelDefault("Caption", "Planilla Detalle"), NavigationItem(false), DefaultProperty("Empleado")]
    [Persistent(nameof(PlanillaDetalle))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PlanillaDetalle : XPCustomBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PlanillaDetalle(Session session)
            : base(session)
        {
            AddToMetodos();
        }

        public PlanillaDetalle(Session session, Planilla APlani, Empleado.Module.BusinessObjects.Empleado emple) : base(session)
        {
            planilla = APlani;
            empleado = emple;
            unidad = emple.Unidad;
            cargo = emple.Cargo;
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        #region Propiedades

        [DbType("bigint"), Persistent(nameof(Oid)), Key(true)]
        int oid = -1;
        [Persistent(nameof(Planilla)), Association("Planilla-Detalles")]
        Planilla planilla = null;
        [Persistent(nameof(Empleado))]
        SBT.Apps.Empleado.Module.BusinessObjects.Empleado empleado = null;
        [Persistent(nameof(Unidad))]
        EmpresaUnidad unidad = null;
        [Persistent(nameof(Cargo))]
        Cargo cargo = null;

        [PersistentAlias(nameof(oid)), XafDisplayName("Oid")]
        public Int64 Oid
        {
            get => oid;
        }
        [PersistentAlias(nameof(planilla)), XafDisplayName("Planilla")]
        public Planilla Planilla
        {
            get => planilla;
        }
        [PersistentAlias(nameof(empleado)), XafDisplayName("Empleado")]
        public SBT.Apps.Empleado.Module.BusinessObjects.Empleado Empleado
        {
            get => empleado;
        }
        [PersistentAlias(nameof(unidad)), XafDisplayName("Unidad")]
        [ToolTip("Unidad a la cual pertenece el empleado, cuando se calculo la planilla")]
        public EmpresaUnidad Unidad
        {
            get => unidad;
        }
        [PersistentAlias(nameof(cargo)), XafDisplayName("Cargo")]
        [ToolTip("Cargo del empleado cuando se calculo la planilla (por fines históricos)")]
        public Cargo Cargo
        {
            get => cargo;
        }

        #endregion

        #region Colecciones
        [Association("PlanillaDetalle-Operaciones"), DevExpress.Xpo.Aggregated, XafDisplayName("Operaciones"), Index(0)]
        public XPCollection<PlanillaDetalleOperacion> Operaciones
        {
            get
            {
                return GetCollection<PlanillaDetalleOperacion>(nameof(Operaciones));
            }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Retorna el acumulado de los dias de un tipo de accion de personal para el empleado actual del detalle y periodo de la planilla
        /// </summary>
        /// <param name="ATipo">Tipo de Accion de Personal</param>
        /// <returns>El acumulado de dias de la accion de personal que se recibe en el parametro</returns>
        private int ObtenerDiasDeAccion(params object[] pa)
        {
            return Convert.ToInt32(Session.ExecuteScalar("select dbo.fnPlaAcumuladoDiasAccion(@OidEmpleado, @Tipo, @FechaInicio, @FechaFin)",
                new string[] { "@OidEmpleado", "@Tipo", "@FechaInicio", "@FechaFin" }, new object[] { Empleado.Oid, pa[0], Planilla.FechaInicio, Planilla.FechaFin }));
        }

        /// <summary>
        /// retorna el monto acumulado de horas extras a pagar en la planilla, para el empleado actual del detalle de la planilla
        /// </summary>
        /// <returns>Acumulado de las horas extras cuya fecha de pago se encuentra en el periodo de la planilla para el empleado del detalle</returns>
        private decimal TotalHorasExtra()
        {
            return Convert.ToDecimal(Session.ExecuteScalar("select dbo.fnPlaTotalHorasExtra(emple.Oid, @FechaPago)", new object[] { Empleado.Oid, Planilla.FechaPago }));
        }

        /// <summary>
        /// Retorna el monto acumulado de una operacion en el mes para el empleado actual del detalle de la planilla
        /// </summary>
        /// <param name="OidOperacion">El Oid de la operacion</param>
        /// <returns>Acumulado del Id de la operacion del parametro en el mes para el empleado del detalle de la planilla</returns>
        private decimal TotalOperacionMes(params object[] pa)
        {
            DateTime fInicioMes = new DateTime(Planilla.FechaFin.Value.Year, Planilla.FechaFin.Value.Month, 1);
            return Convert.ToDecimal(Session.ExecuteScalar("select dbo.fnPlaOperacionAcumulada(emple.Oid, @InicioMes, @FinMes, @OidOperacion)",
                new object[] { Empleado.Oid, fInicioMes, fInicioMes.AddMonths(1).AddSeconds(-1), pa[0] }));
        }

        private decimal TransaccionSuma(params object[] pa)
        {
            return Convert.ToDecimal(Session.ExecuteScalar("select dbo.fnPlaTransaccionDe(@OidEmpleado, @FechaFin, @Clasificacion)",
                new object[] { Empleado.Oid, Planilla.FechaFin, pa[0] }));
        }
  
        private DateTime FechaCumpleAnioContrato()
        {
            return new DateTime(Planilla.FechaFin.Value.Year, Empleado.FechaIngreso.Month, Empleado.FechaIngreso.Day);
        }

        public object Evaluar(string ANombre)
        {
            return Metodos[ANombre](new object[] { });
        }

        public object Evaluar(string ANombre, object p1)
        {
            return Metodos[ANombre](new object[] { p1 });
        }

        public object Evaluar(string ANombre, object p1, object p2)
        {
            return Metodos[ANombre](new object[] { p1, p2 });
        }

        private Dictionary<string, Func<object[], object>> Metodos = new Dictionary<string, Func<object[], object>>();
        private void AddToMetodos()
        {
            if (!Metodos.ContainsKey(nameof(ObtenerDiasDeAccion)))
                Metodos.Add(nameof(ObtenerDiasDeAccion), (x) => ObtenerDiasDeAccion(x));
            if (!Metodos.ContainsKey(nameof(TotalHorasExtra)))
                Metodos.Add(nameof(TotalHorasExtra), (x) => TotalHorasExtra());
            if (!Metodos.ContainsKey(nameof(TotalOperacionMes)))
                Metodos.Add(nameof(TotalOperacionMes), (x) => TotalOperacionMes(x));
            if (!Metodos.ContainsKey(nameof(TransaccionSuma)))
                Metodos.Add(nameof(TransaccionSuma), (x) => TransaccionSuma(x));
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

    //public static class GenericMethods
    //{
    //    public static Dictionary<string, Func<object[], object>> Metodos =  new Dictionary<string, Func<object[], object>>();

    //}
}