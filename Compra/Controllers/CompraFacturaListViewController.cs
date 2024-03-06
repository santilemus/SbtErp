using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using System;


namespace SBT.Apps.Compra.Module.Controllers
{
    public class CompraFacturaListViewController : ViewController<ListView>
    {
        private PopupWindowShowAction pwsaExportarAnexoPagoCuenta;
        public CompraFacturaListViewController()
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);

            pwsaExportarAnexoPagoCuenta = new PopupWindowShowAction(this, "Compras_AnexoPagoCuenta_Exportar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaExportarAnexoPagoCuenta.Caption = "Exportar";
            pwsaExportarAnexoPagoCuenta.AcceptButtonCaption = "Exportar";
            pwsaExportarAnexoPagoCuenta.CancelButtonCaption = "Cancelar";
            pwsaExportarAnexoPagoCuenta.ActionMeaning = ActionMeaning.Accept;
            pwsaExportarAnexoPagoCuenta.ToolTip = "Clic para exportar el anexo con las retenciones de renta al formato requerido de pago a cuenta para cargarlo en la plataforma de declaración de impuestos";
            pwsaExportarAnexoPagoCuenta.TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
            pwsaExportarAnexoPagoCuenta.ImageName = "ExportToCSV";
            pwsaExportarAnexoPagoCuenta.CustomizePopupWindowParams += PwsaExportarAnexoPagoCuenta_CustomizePopupWindowParams;
            pwsaExportarAnexoPagoCuenta.Execute += PwsaExportarAnexoPagoCuenta_Execute;
        }

        private void PwsaExportarAnexoPagoCuenta_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            FechaParam pa = (e.PopupWindowViewCurrentObject as FechaParam);
            var empresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            var fechaInicio = new DateTime(pa.Fecha.Year, pa.Fecha.Month, 01);
            var fechaFin = fechaInicio.AddMonths(1).AddSeconds(-1);
            CriteriaOperator criteria = CriteriaOperator.FromLambda<CompraFactura>(x => x.Empresa.Oid == empresaOid &&
                                            x.Fecha >= fechaInicio && x.Fecha <= fechaFin && x.Renta > 0.00m);
            ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
            try
            {
                DoExecuteExport(pa);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error al exportar Anexo Pago a Cuenta (Retenciones de Renta){System.Environment.NewLine}{ex.Message}",
                     InformationType.Error);
            }
        }

        private void PwsaExportarAnexoPagoCuenta_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(FechaParam));
            var pa = os.CreateObject<FechaParam>();
            IModelView detailViewModel = Application.FindModelView("FechaParam_DetailView_MonthYear");
            if (detailViewModel != null)
                e.View = Application.CreateDetailView(os, detailViewModel.Id, false, pa);
            else
                e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Exportar Anexo Pago a Cuenta";
        }

        protected virtual void DoExecuteExport(FechaParam parametros)
        {

        }
    }
}
