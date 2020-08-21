using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.RecursoHumano.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.RecursoHumano.Module.Controllers
{
    /// <summary>
    /// View Controller Planilla
    /// </summary>
    /// <remarks>
    /// Mas info en https://docs.devexpress.com/XPO/8914/feature-center/querying-a-data-store/direct-sql-queries
    /// </remarks>
    public class vcPlanilla : ViewControllerBase
    {
        private SimpleAction saAprobar;
        private PopupWindowShowAction pwsaCalcular;
        private IContainer components;
        private DevExpress.ExpressApp.View vParam;

        public vcPlanilla() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            //if (View.GetType().Name == "ListView")
            //    ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Empresa] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);

        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.Planilla);
            // simple action aprobar la planilla
            saAprobar = new SimpleAction(this, "saPlanillaAprobar", PredefinedCategory.Edit);
            saAprobar.Caption = "Aprobar";
            saAprobar.TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.Planilla);
            saAprobar.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            //// popup window action Calcular
            pwsaCalcular = new PopupWindowShowAction(this, "pwsaCalcular", PredefinedCategory.Edit);
            pwsaCalcular.Caption = "Calcular Planilla";
            pwsaCalcular.TargetObjectType = typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.Planilla);
            pwsaCalcular.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            pwsaCalcular.ImageName = "service";
            pwsaCalcular.AcceptButtonCaption = "Calcular";
            pwsaCalcular.CancelButtonCaption = "Cancelar";

            //pwsaCalcular.Execute += new PopupWindowShowActionExecuteEventHandler(pwsaCalcular_Execute);
            pwsaCalcular.Execute += pwsaCalcular_Execute;
            pwsaCalcular.CustomizePopupWindowParams += pwsaCalcular_CustomizePopupWindowsParams;

        }

        protected override void Dispose(bool disposing)
        {
            saAprobar.Dispose();
            pwsaCalcular.Dispose();

            base.Dispose(disposing);
        }


        //static string sSQL = @"select Empleado, DiasLicenciaSinSueldo, DiasInasistencia, DiasAmonestacion, DiasIncapacidad, DiasMaternidad " +
        //                     @"TotalHoraExtra, CotizaAcumuladaIsss, CotizaAcumuladaAfp, CotizaAcumuladaRenta, IngresoBrutoQuincena, " +
        //                     @"FechaInicio, FechaFin, FechaPago, ParametroEmpresa " +
        //                     @"  from dbo.fnPlaEmpleadoPlanilla(@OidEmpresa, @OidEmpleado, @FechaInicio, @FechaFin, @FechaPago)";

        //////Define a mapping array that specifies the order of columns in a result set.
        //static LoadDataMemberOrderItem[] empleadoPlanillaLoadOrder = new LoadDataMemberOrderItem[]
        //{
        //    new LoadDataMemberOrderItem(0, "Empleado"),
        //    new LoadDataMemberOrderItem(1, "DiasLicenciaSinSueldo"),
        //    new LoadDataMemberOrderItem(2, "DiasInasistencia"),
        //    new LoadDataMemberOrderItem(3, "DiasAmonestacion"),
        //    new LoadDataMemberOrderItem(4, "DiasIncapacidad"),
        //    new LoadDataMemberOrderItem(5, "DiasMaternidad"),
        //    new LoadDataMemberOrderItem(6, "TotalHorasExtra"),
        //    new LoadDataMemberOrderItem(7, "CotizaAcumuladaIsss"),
        //    new LoadDataMemberOrderItem(8, "CotizaAcumuladaAfp"),
        //    new LoadDataMemberOrderItem(9, "CotizaAcumuladaRenta"),
        //    new LoadDataMemberOrderItem(10, "IngresoBrutoQuincena") //,
        //    //new LoadDataMemberOrderItem(11, "FechaInicio"),
        //    //new LoadDataMemberOrderItem(12, "FechaFin"),
        //    //new LoadDataMemberOrderItem(13, "FechaPago")
        //};

        private void pwsaCalcular_CustomizePopupWindowsParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace oSpace = (NonPersistentObjectSpace)Application.ObjectSpaceProviders[1].CreateObjectSpace(); 
            var pa = oSpace.CreateObject<CalcularPlanillaParam>();
            pa.ObjectSpace = ObjectSpace; // Application.CreateObjectSpace(typeof(SBT.Apps.RecursoHumano.Module.BusinessObjects.TipoPlanilla));
            e.View = Application.CreateDetailView(oSpace, pa);
            e.View.Caption = "Calcular Planilla";
            e.Size = new System.Drawing.Size(500, 500);
            e.IsSizeable = false;
            vParam = e.View;
        }

        /// <summary>
        ///  Crear la condicion para los empleados a calcular por tipo de planilla. Evaluar si esta condicion queda
        ///  parametrizable en el BO TipoPlanilla, Habra que agregar un ExpressionEditor y ademas sera del BO empleado
        /// </summary>
        /// <returns></returns>
        private string CondicionEmpleado(EClasePlanilla AClasePlanilla)
        {
            string sCond = string.Empty;
            switch (AClasePlanilla)
            {
                case EClasePlanilla.Salarios:
                    sCond = "[Estado.Codigo] In ('EMPL01', 'EMPL04', 'EMPL05') && [TipoContrato] In (0, 1) && [TipoPlanillas][[TipoPlanilla.Clase] == ? && [TipoPlanilla.Activo] == True]";
                    break;
                case EClasePlanilla.Vacacion:
                    sCond = "[Estado.Codigo] In ('EMPL01', 'EMPL04', 'EMPL05') && [TipoContrato] In (0, 1) && [TipoPlanillas][[TipoPlanilla.Clase] == ? && [TipoPlanilla.Activo] == True] " +
                        " && GetMonth([FechaIngreso]) == ? && GetDay([FechaIngreso] Between (?, ?) && GetYear([FechaIngreso]) < ?";
                    break;
            }
            return sCond;
        }

        private void pwsaCalcular_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            //IObjectSpace ospace = Application.ObjectSpaceProvider.CreateObjectSpace();

            //ICollection<PlanillaDetalleFuncion> empleFuncs;
            // filtramos los empleados activos con tipos de contrato Indefinido, plazo y que se les debe calcular el tipo de planilla seleccionado y el tipo de planilla activo
            CalcularPlanillaParam cp = e.PopupWindowView.CurrentObject as CalcularPlanillaParam;
            EClasePlanilla clasePlani = cp.TipoPlanilla.Clase;

            IList<DevExpress.Xpo.SortProperty> sortp = new List<DevExpress.Xpo.SortProperty>();
            sortp.Add(new DevExpress.Xpo.SortProperty("Oid", DevExpress.Xpo.DB.SortingDirection.Ascending));
            
            IList<Empleado.Module.BusinessObjects.Empleado> empleados = ObjectSpace.GetObjects<Empleado.Module.BusinessObjects.Empleado>(
                CriteriaOperator.Parse(CondicionEmpleado(clasePlani), new object[] { clasePlani }), sortp, true);
            // si no hay datos, no calcula nada y sale
            if (empleados.Count == 0)
                return;  
            Planilla plani = (Planilla)View.CurrentObject;          
            plani.SetEncabezadoDePlanilla(cp.TipoPlanilla, cp.FechaInicio, cp.FechaFin, cp.FechaPago);
            foreach (Empleado.Module.BusinessObjects.Empleado emple in empleados)
            {
                if (!plani.Detalles.Any<PlanillaDetalle>(item => item.Empleado.Oid == emple.Oid))
                {
                    var pd = new PlanillaDetalle(plani.Session, plani, emple);

                    //empleFuncs = ((XPObjectSpace)ospace).Session.GetObjectsFromQuery<PlanillaDetalleFuncion>(empleadoPlanillaLoadOrder, sSQL,
                    //                new string[] { "@OidEmpresa", "@OidEmpleado", "@FechaInicio", "@FechaFin", "@FechaPago" },
                    //                new object[] { plani.Empresa, emple.Oid, plani.FechaInicio, plani.FechaFin, plani.FechaPago });
                    //foreach (PlanillaDetalleFuncion detfunc in empleFuncs)
                    //    pd.Funciones.Add(detfunc);
                    plani.Detalles.Add(pd);
                    CalcularOperaciones(pd);
                    //empleFuncs.Clear();
                }
            }
            if (plani.Session.InTransaction)
            {
                plani.Save();
                plani.Session.CommitTransaction();
            }
        }


        private void CalcularOperaciones(PlanillaDetalle planillaDetalle)
        {
            using (var os = Application.CreateObjectSpace(typeof(OperacionTipoPlanilla)))
            {
                ICollection<OperacionTipoPlanilla> ops = os.GetObjects<OperacionTipoPlanilla>(CriteriaOperator.Parse("[Tipo.Oid] == ?", planillaDetalle.Planilla.Tipo.Oid));
                decimal valor = 0.0m;
                foreach (OperacionTipoPlanilla op in ops)
                {
                    if (op.Operacion.TipoBO != null && op.Operacion.Formula.Length > 0)
                    {
                        // ultimo error en la siguiente linea, creo que es en la formula, revisar 
                        // DevExpress.Data.Filtering.Exceptions.InvalidPropertyPathException: 'Can't find property 'Empleado!''
                        ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(op.Operacion.TipoBO), op.Operacion.Formula);
                        valor = Convert.ToDecimal(eval.Evaluate(planillaDetalle));
                    }
                    else
                        valor = op.Operacion.Valor;
                    planillaDetalle.Operaciones.Add(new PlanillaDetalleOperacion(planillaDetalle.Planilla.Session, planillaDetalle, op.Operacion, valor));
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            // 
            // vcPlanilla
            // 

        }
    }
}
