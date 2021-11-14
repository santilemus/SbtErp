using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Utils.Filtering.Internal;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;
using SBT.Apps.Contabilidad.Module.BusinessObjects;

namespace SBT.Apps.Contabilidad.Module.Controllers
{
    public class vcEstadoFinanciero : ViewControllerBase
    {
        private PopupWindowShowAction pwsaGenerarEstadoFinanciero;
        private EstadoFinancieroParams efParams;
        public vcEstadoFinanciero() : base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaGenerarEstadoFinanciero.CustomizePopupWindowParams += pwsaGenerarEstadoFinanciero_CustomizePopupWindowsParams;
            pwsaGenerarEstadoFinanciero.Execute += pwsaGenerarEstadoFinanciero_Execute;
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
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
            pwsaGenerarEstadoFinanciero = new PopupWindowShowAction(this, "pwsaGenerarEstadoFinanciero", DevExpress.Persistent.Base.PredefinedCategory.RecordsNavigation);
            pwsaGenerarEstadoFinanciero.Caption = "Generar";
            pwsaGenerarEstadoFinanciero.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            pwsaGenerarEstadoFinanciero.ToolTip = "Clic para generar el estado financiero que corresponde al modelo seleccionado";
            pwsaGenerarEstadoFinanciero.AcceptButtonCaption = "Generar";
            pwsaGenerarEstadoFinanciero.CancelButtonCaption = "Cerrar";
            pwsaGenerarEstadoFinanciero.ImageName = "CierreDiario";
            pwsaGenerarEstadoFinanciero.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        }

        protected override void DoDispose()
        {
            base.DoDispose();
        }

        private void pwsaGenerarEstadoFinanciero_CustomizePopupWindowsParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace osParams = Application.CreateObjectSpace(typeof(EstadoFinancieroParams));
            EstadoFinancieroParams pa = osParams.CreateObject<EstadoFinancieroParams>();
            DetailView paDetailView= Application.CreateDetailView(osParams, pa, true);
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
            IObjectSpace os2 = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(EFinancieroDetalle));

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
            if (e.ObjectSpace is NonPersistentObjectSpace)
            {
                ((NonPersistentObjectSpace)e.ObjectSpace).ObjectsGetting += os2_ObjectsGetting;
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
            BindingList<EFinancieroDetalle> objects = new BindingList<EFinancieroDetalle>();
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
                else if (item.Formula1 != null)
                {
                    saldo1 = Convert.ToDecimal(ef.Evaluate(CriteriaOperator.Parse(item.Formula1, efParams.FechaHasta.Year, efParams.FechaHasta.Month)));
                }
                if (item.Cuenta2 != null)
                {
                    saldoMes = saldosEF.FirstOrDefault(y => y.Cuenta.Oid == item.Cuenta2.Oid);
                    saldo2 = (saldoMes != null) ? saldoMes.SaldoFin : 0.0m;
                }
                else if (item.Formula2 != null)
                {
                    saldo2 = Convert.ToDecimal(ef.Evaluate(CriteriaOperator.Parse(item.Formula2, efParams.FechaHasta.Year, efParams.FechaHasta.Month)));
                }
                objects.Add(new EFinancieroDetalle()
                {
                    Oid = item.Oid,
                    Nombre1 = item.Nombre1,
                    Nivel1 =  (item.Cuenta1 != null) ? item.Cuenta1.Nivel: 0,
                    Nombre2 = item.Nombre2,
                    Nivel2 =  (item.Cuenta2 != null) ? item.Cuenta2.Nivel: 0,
                    Valor1 = saldo1,
                    Valor2 = saldo2,
                    EstadoFinancieroModelo = ef,
                    Plural = efParams.Moneda.Plural
                });
            }
            e.Objects = objects;
        }

        private decimal SaldoCalculado(EstadoFinancieroModelo bo, string cuentaCodigo)
        {
            CriteriaOperator formula = CriteriaOperator.Parse("SaldoDeCuenta(?, ?, ?, 10, '1')", bo, bo.Empresa.Oid,
                                                              efParams.FechaHasta.Year, efParams.FechaHasta.Month, cuentaCodigo);
            ExpressionEvaluator eval = new ExpressionEvaluator(TypeDescriptor.GetProperties(bo), formula);
            return Convert.ToDecimal(eval.Evaluate(bo));
        }

        private void reportController_BeforePrint(XtraReport report)
        {

        }

    }
}
