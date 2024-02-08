using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.XtraReports.UI;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    public class vcEstadoFinanciero : ViewControllerBase
    {
        private PopupWindowShowAction pwsaGenerarEstadoFinanciero;
        private EstadoFinancieroParams efParams;
        private PopupWindowShowAction pwsaSelectEmpleado;
        public vcEstadoFinanciero() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaGenerarEstadoFinanciero.CustomizePopupWindowParams += pwsaGenerarEstadoFinanciero_CustomizePopupWindowsParams;
            pwsaGenerarEstadoFinanciero.Execute += pwsaGenerarEstadoFinanciero_Execute;
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;

            pwsaSelectEmpleado.CustomizePopupWindowParams += PwsaSelectEmpleado_CustomizePopupWindowParams;
            pwsaSelectEmpleado.Execute += PwsaSelectEmpleado_Execute;
        }

        protected override void OnDeactivated()
        {
            pwsaGenerarEstadoFinanciero.CustomizePopupWindowParams += pwsaGenerarEstadoFinanciero_CustomizePopupWindowsParams;
            pwsaGenerarEstadoFinanciero.Execute += pwsaGenerarEstadoFinanciero_Execute;
            Application.ObjectSpaceCreated -= Application_ObjectSpaceCreated;
            base.OnDeactivated();
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.EstadoFinancieroModelo);
            // pwsaGenerarEstadoFinanciero
            pwsaGenerarEstadoFinanciero = new(this, "pwsaGenerarEstadoFinanciero", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation)
            {
                Caption = "Generar",
                TargetViewType = DevExpress.ExpressApp.ViewType.ListView,
                ToolTip = "Clic para generar el estado financiero que corresponde al modelo seleccionado",
                AcceptButtonCaption = "Generar",
                CancelButtonCaption = "Cerrar",
                ImageName = "CierreDiario",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            };
            // Lista de seleccion de empleado para el Contador y Rep. Legal
            pwsaSelectEmpleado = new(this, "pwsaSelectEmpleado", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit)
            {
                Caption = "Empleado",
                TargetViewType = ViewType.DetailView,
                ToolTip = "Clic para seleccionar el Contador o Representante Legal",
                CancelButtonCaption = "Cerrar"
            };
        }

        protected override void DoDispose()
        {
            //pwsaGenerarEstadoFinanciero.Dispose();
            //pwsaSelectEmpleado.Dispose();
            base.DoDispose();
        }

        private void pwsaGenerarEstadoFinanciero_CustomizePopupWindowsParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace osParams = Application.CreateObjectSpace(typeof(EstadoFinancieroParams));
            EstadoFinancieroParams pa = osParams.CreateObject<EstadoFinancieroParams>();
            DetailView paDetailView = Application.CreateDetailView(osParams, pa, true);
            paDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = paDetailView;
        }

        /// <summary>
        /// Generar el reporte del estado financiero para el modelo seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Mas informacion para desplegar el previo del reporte en codigo en
        /// https://docs.devexpress.com/eXpressAppFramework/113703/shape-export-print-data/reports/invoke-the-report-preview-from-code
        /// </remarks>
        private void pwsaGenerarEstadoFinanciero_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            // generar el preview del reporte
            if (View.SelectedObjects == null || View.SelectedObjects.Count == 0)
                return;
            if (((EstadoFinancieroModelo)View.SelectedObjects[0]).Reporte == null)
                return;   // no hay reporte vinculado al estado financiero
            efParams = ((EstadoFinancieroParams)e.PopupWindowViewCurrentObject);

            var reportData = ((EstadoFinancieroModelo)View.SelectedObjects[0]).Reporte;
            string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
            ReportServiceController reportController = Frame.GetController<ReportServiceController>();
            if (reportController != null)
            {
                reportController.ShowPreview(handle);
            }
        }

        private void Application_ObjectSpaceCreated(Object sender, ObjectSpaceCreatedEventArgs e)
        {
            if (e.ObjectSpace is NonPersistentObjectSpace space)
            {
                space.ObjectsGetting += os2_ObjectsGetting;
            }
        }

        /// <summary>
        /// Crear la coleccion no persistente para el informe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// mas informacion de reportes con non persistent objects en los siguientes enlaces
        /// https://docs.devexpress.com/eXpressAppFramework/114516/business-model-design-orm/non-persistent-objects/how-to-display-non-persistent-objects-in-a-report?utm_source=SupportCenter&utm_medium=website&utm_campaign=docs-feedback&utm_content=T846224
        /// https://supportcenter.devexpress.com/ticket/details/t725595/how-to-use-the-show-in-report-action-with-non-persistent-objects
        /// https://supportcenter.devexpress.com/ticket/details/t929653/refresh-nonpersistent-objects-for-report
        /// https://docs.devexpress.com/eXpressAppFramework/403189/business-model-design-orm/non-persistent-objects/how-to-display-a-list-view-with-data-from-a-stored-procedure-with-a-parameter
        /// </remarks>

        private void os2_ObjectsGetting(Object sender, ObjectsGettingEventArgs e)
        {
            if (e.ObjectType != typeof(EFinancieroDetalle))
                return;
            BindingList<EFinancieroDetalle> objects = new();
            // preparar los datos para el informe
            IObjectSpace os = Application.CreateObjectSpace(typeof(SaldoMes));
            EstadoFinancieroModelo ef = (View.SelectedObjects[0] as EstadoFinancieroModelo);

            int[] ctas1 = ef.Detalles.Where(y => y.Cuenta1 != null).Select(y => y.Cuenta1.Oid).ToArray();
            var ctas2 = ef.Detalles.Where(y => y.Cuenta2 != null).Select(y => y.Cuenta2.Oid).ToArray();
            var ctas = ctas1.Concat(ctas2.ToArray<int>());

            var saldos = os.GetObjects<SaldoMes>(new GroupOperator(new BinaryOperator("MesAnio", Convert.ToInt32(string.Format("{0:D}{1:MMyyyy}", ef.Empresa.Oid, efParams.FechaHasta))),
                                                      new InOperator("Cuenta.Oid", ctas)));

            var saldosEF = saldos.Where(y => y.Periodo.Oid == efParams.FechaHasta.Year && y.Mes == efParams.FechaHasta.Month && ctas.Contains(y.Cuenta.Oid));

            foreach (EstadoFinancieroModeloDetalle item in ef.Detalles)
            {
                decimal saldo1 = 0.0m;
                decimal saldo2 = 0.0m;
                SaldoMes saldoMes = null;
                if (item.Cuenta1 != null)
                {
                    saldoMes = saldosEF.FirstOrDefault(y => y.Cuenta.Oid == item.Cuenta1.Oid);
                    saldo1 = (saldoMes != null) ? saldoMes.SaldoFin : 0.0m;
                }
                else if (!string.IsNullOrEmpty(item.Formula1))
                {
                    saldo1 = SaldoCalculado(item.Formula1, item);
                }
                if (item.Cuenta2 != null)
                {
                    saldoMes = saldosEF.FirstOrDefault(y => y.Cuenta.Oid == item.Cuenta2.Oid);
                    saldo2 = (saldoMes != null) ? saldoMes.SaldoFin : 0.0m;
                }
                else if (!string.IsNullOrEmpty(item.Formula2))
                {
                    saldo2 = SaldoCalculado(item.Formula2, item);
                }
                objects.Add(new EFinancieroDetalle()
                {
                    Oid = item.Oid,
                    Nombre1 = EvaluarNombre(item, item.Nombre1),
                    Nivel1 = (item.Cuenta1 != null) ? item.Cuenta1.Nivel : 0,
                    Nombre2 = EvaluarNombre(item, item.Nombre2),
                    Nivel2 = (item.Cuenta2 != null) ? item.Cuenta2.Nivel : 0,
                    Valor1 = saldo1,
                    Valor2 = saldo2,
                    EstadoFinancieroModelo = ef,
                    Plural = efParams.Moneda.Plural,
                    FechaHasta = efParams.FechaHasta,
                    Orden = item.Orden
                });
            }
            objects.OrderBy(y => y.Orden);
            e.Objects = objects;
        }

        private decimal SaldoCalculado(string formula, EstadoFinancieroModeloDetalle item)
        {
            var expresion = formula.Replace("?P0", Convert.ToString(efParams.FechaHasta.Year));
            expresion = expresion.Replace("?P1", Convert.ToString(efParams.FechaHasta.Month));
            return Convert.ToDecimal(item.EstadoFinanciero.Evaluate(CriteriaOperator.Parse(expresion)));
        }

        private string EvaluarNombre(EstadoFinancieroModeloDetalle item, string expresion)
        {
            CriteriaOperator criteriaOp = CriteriaOperator.TryParse(expresion);
            if (criteriaOp is null)
                return expresion;
            else
            {
                var exp2 = expresion.Replace("?P0", Convert.ToString(efParams.FechaHasta.Year));
                exp2 = exp2.Replace("?P1", Convert.ToString(efParams.FechaHasta.Month));
                return Convert.ToString(item.EstadoFinanciero.Evaluate(exp2));
            }
        }

        private void reportController_BeforePrint(XtraReport report)
        {

        }


        private void PwsaSelectEmpleado_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var empleado = (e.PopupWindowView.CurrentObject as Empleado.Module.BusinessObjects.Empleado);
            (View.CurrentObject as EstadoFinancieroModelo).Contador = empleado.NombreCompleto;
        }



        private void PwsaSelectEmpleado_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = e.Application.CreateObjectSpace(typeof(Empleado.Module.BusinessObjects.Empleado));
            string idView = e.Application.FindLookupListViewId(typeof(Empleado.Module.BusinessObjects.Empleado));
            IModelListView modelListView = (IModelListView)Application.FindModelView(idView);
            modelListView.FilterEnabled = true;
            modelListView.DataAccessMode = CollectionSourceDataAccessMode.Server;
            //alternativa CollectionSourceDataAccessMode.ServerView es más light
            //modelListView.DataAccessMode = CollectionSourceDataAccessMode.ServerView;
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(os, typeof(Empleado.Module.BusinessObjects.Empleado), idView);
            e.View = Application.CreateListView(modelListView, collectionSource, true); ;
        }

    }
}
