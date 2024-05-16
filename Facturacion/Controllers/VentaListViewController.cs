using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using SBT.Apps.Base.Module.BusinessObjects;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.DependencyInjection;
using DevExpress.ExpressApp.Model;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    public class VentaListViewController: ViewController<ListView>
    {
        private int fEmpresaOid = -1;
        private PopupWindowShowAction pwsaExportarAnulados;

        private int EmpresaOid
        {
            get
            {
                if (fEmpresaOid <= 0)
                {
                    if (SecuritySystem.CurrentUser == null)
                        fEmpresaOid = ObjectSpace.GetObjectByKey<Usuario>(ObjectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().User).Empresa.Oid;
                    else
                        fEmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
                }
                return fEmpresaOid;
            }
        }

        public VentaListViewController() : base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            pwsaExportarAnulados = new PopupWindowShowAction(this, "Venta_ExportarAnulados", DevExpress.Persistent.Base.PredefinedCategory.Export.ToString());
            pwsaExportarAnulados.TargetViewType = ViewType.ListView;
            pwsaExportarAnulados.Caption = @"Exportar Anulados";
            pwsaExportarAnulados.TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            pwsaExportarAnulados.ToolTip += @"Clic para exportar los documentos de venta anulados, para cargarlos en la plataforma electrónica de pago de impuestos";
            pwsaExportarAnulados.ImageName = "ExportToCSV";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaExportarAnulados.CustomizePopupWindowParams += PwsaExportarAnulados_CustomizePopupWindowParams;
            pwsaExportarAnulados.Execute += PwsaExportarAnulados_Execute;
        }

        private void PwsaExportarAnulados_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            FechaParam pa = (e.PopupWindowViewCurrentObject as FechaParam);
            var fechaInicio = new DateTime(pa.Fecha.Year, pa.Fecha.Month, 01);
            var fechaFin = pa.Fecha.Date.AddMonths(1).AddSeconds(-1);
            CriteriaOperator criteria = CriteriaOperator.FromLambda<Venta>(x => x.Empresa.Oid == EmpresaOid &&
                                            x.Fecha >= fechaInicio && x.Fecha <= fechaFin);
            ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
            try
            {
                DoExecuteExport(pa);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error al exportar documentos anulados{System.Environment.NewLine}{ex.Message}",
                     InformationType.Error);
            }
        }

        protected override void OnDeactivated()
        {
            pwsaExportarAnulados.CustomizePopupWindowParams -= PwsaExportarAnulados_CustomizePopupWindowParams;
            pwsaExportarAnulados.Execute -= PwsaExportarAnulados_Execute;
            base.OnDeactivated();
        }

        private void PwsaExportarAnulados_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(FechaParam));
            var pa = os.CreateObject<FechaParam>();
            IModelView detailViewModel = Application.FindModelView("FechaParam_DetailView_MonthYear");
            if (detailViewModel != null)
                e.View = Application.CreateDetailView(os, detailViewModel.Id, false, pa);
            else
                e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Exportar Documentos Anulados";
        }

        protected virtual void DoExecuteExport(FechaParam parametros)
        {

        }


    }
}
