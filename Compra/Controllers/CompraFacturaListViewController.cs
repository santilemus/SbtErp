using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using System;
using System.IO;
using System.Text;


namespace SBT.Apps.Compra.Module.Controllers
{
    public class CompraFacturaListViewController : ViewController<ListView>
    {
        private int fEmpresaOid = -1;
        private PopupWindowShowAction pwsaExportarAnexoPagoCuenta;
        private PopupWindowShowAction pwsaCargaDte;

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

            pwsaCargaDte = new PopupWindowShowAction(this, "CargarDteCompra", PredefinedCategory.RecordEdit.ToString());
            pwsaCargaDte.Caption = "Cargar Dte";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaCargaDte.CustomizePopupWindowParams += PwsaCargaDte_CustomizePopupWindowParams;
            pwsaCargaDte.Execute += PwsaCargaDte_Execute;
        }

        private void PwsaCargaDte_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            DoCargarDte((FileUploadParameter)e.PopupWindowViewCurrentObject);
        }

        protected virtual void DoCargarDte(FileUploadParameter parameter)
        {
            using MemoryStream ms = new MemoryStream();
            parameter.FileData.SaveToStream(ms);
            ms.Position = 0;
            using StreamReader rd = new StreamReader(ms, Encoding.UTF8);
            string json = rd.ReadToEnd();
            Application.ShowViewStrategy.ShowMessage($@"ejecuto cargar el Dte {parameter.Oid}");
        }

        protected override void OnDeactivated()
        {
            pwsaCargaDte.CustomizePopupWindowParams -= PwsaCargaDte_CustomizePopupWindowParams;
            base.OnDeactivated();
        }

        private void PwsaCargaDte_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace osParam = Application.CreateObjectSpace(typeof(FileUploadParameter));
            FileUploadParameter fileParams = osParam.CreateObject<FileUploadParameter>();
            e.View = Application.CreateDetailView(osParam, fileParams);
            e.View.Caption = "Cargar Dte";
        }

        private void PwsaExportarAnexoPagoCuenta_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            FechaParam pa = (e.PopupWindowViewCurrentObject as FechaParam);
            var fechaInicio = new DateTime(pa.Fecha.Year, pa.Fecha.Month, 01);
            var fechaFin = fechaInicio.AddMonths(1).AddSeconds(-1);
            CriteriaOperator criteria = CriteriaOperator.FromLambda<CompraFactura>(x => x.Empresa.Oid == EmpresaOid &&
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
