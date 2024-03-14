using System;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraPrinting;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Base.Module;
using SBT.Apps.Compra.Module.BusinessObjects;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.DependencyInjection;

namespace SBT.Apps.Iva.Module.Controllers
{
    public class LibroCompraListViewController : ViewController<ListView>
    {
        private int fEmpresaOid = -1;
        private PopupWindowShowAction pwsaGenerar;
        private PopupWindowShowAction pwsaExportarAnexosIva;
        private ExportController exportController;

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
        public LibroCompraListViewController() : base()
        {
            this.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroCompra);
            pwsaGenerar = new PopupWindowShowAction(this, "LibroCompras_Generar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaGenerar.Caption = "Generar";
            pwsaGenerar.CancelButtonCaption = "Cancelar";
            pwsaGenerar.AcceptButtonCaption = "Generar";
            pwsaGenerar.ToolTip = "Clic para generar el libro de compras";
            pwsaGenerar.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroCompra);
            pwsaGenerar.ImageName = "service";
            pwsaGenerar.CustomizePopupWindowParams += PwsaGenerar_CustomizePopupWindowParams;
            pwsaGenerar.Execute += PwsaGenerar_Execute;

            pwsaExportarAnexosIva = new PopupWindowShowAction(this, "LibroCompras_Anexos_Exportar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            pwsaExportarAnexosIva.Caption = "Exportar";
            pwsaExportarAnexosIva.AcceptButtonCaption = "Exportar";
            pwsaExportarAnexosIva.CancelButtonCaption = "Cancelar";
            pwsaExportarAnexosIva.ActionMeaning = ActionMeaning.Accept;
            pwsaExportarAnexosIva.ToolTip = "Clic para exportar anexos del libro de compras para cargarlos en plataforma de declaración de impuestos";
            pwsaExportarAnexosIva.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroCompra);
            pwsaExportarAnexosIva.ImageName = "ExportToCSV";
            pwsaExportarAnexosIva.CustomizePopupWindowParams += PwsaExportarCompras_CustomizePopupWindowParams;
            pwsaExportarAnexosIva.Execute += PwsaExportarCompras_Execute;
        }

        private void PwsaExportarCompras_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            LibroCompraExportarParams pa = (e.PopupWindowViewCurrentObject as LibroCompraExportarParams);
            var inicioMes = pa.Fecha.Date;
            CriteriaOperator criteria = GroupOperator.And(new BinaryOperator("CompraFactura.Empresa.Oid", EmpresaOid),
                                             new BetweenOperator("Fecha", inicioMes, inicioMes.AddMonths(1).AddSeconds(-1)));
            ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
            try
            {
                DoExecuteExport(pa);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error al exportar Libro de Compras{System.Environment.NewLine}{ex.Message}", InformationType.Error);
            }
        }

        protected virtual void DoExecuteExport(LibroCompraExportarParams parametros)
        {

        }

        private void PwsaExportarCompras_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(LibroCompraExportarParams));
            var pa = os.CreateObject<LibroCompraExportarParams>();
            e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Exportar Anexos Libro Compras";
        }

        private void PwsaGenerar_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            LibroIvaParams pa = (e.PopupWindowViewCurrentObject as LibroIvaParams);
            if (pa.Anio != null && pa.Mes != null)
            {
                try
                {
                    string codMoneda = pa.Moneda.Codigo;
                    ((XPObjectSpace)ObjectSpace).Session.ExecuteSproc("spGeneraLibroCompra", EmpresaOid, codMoneda, Convert.ToInt16(pa.Mes), pa.Anio);
                    DateTime inicioMes = new DateTime(Convert.ToInt16(pa.Anio), (int)pa.Mes, 01);
                    //View.RefreshDataSource(); comentario para probar sino afecta quitar esto 03/feb/2024
                    CriteriaOperator criteria = GroupOperator.And(new BinaryOperator("CompraFactura.Empresa.Oid", EmpresaOid),
                                                                 new BetweenOperator("Fecha", inicioMes, inicioMes.AddMonths(1).AddMinutes(-1)));
                    ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
                    Application.ShowViewStrategy.ShowMessage($@"Libro de Compras Generado", InformationType.Success);
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage($@"No se generó el Libro de Compras{System.Environment.NewLine}{ex.Message}", InformationType.Error);
                }
            }
            else
                Application.ShowViewStrategy.ShowMessage(@"No se genero el Libro de Compras porque uno o más parámetros son nulos", InformationType.Warning);
        }

        private void PwsaGenerar_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Base.Module.BusinessObjects.LibroIvaParams));
            var pa = os.CreateObject<LibroIvaParams>();
            CriteriaOperator condicion = CriteriaOperator.Parse("CompraFactura.Empresa = ? And Cerrado = 1", EmpresaOid);
            var fecha = Convert.ToDateTime(ObjectSpace.Evaluate(typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroCompra), CriteriaOperator.Parse("max([Fecha])"), condicion));
            if (fecha.Year > 1)
            {
                pa.Anio = fecha.AddMonths(1).Year;
                pa.Mes = (EMes)fecha.AddMonths(1).Month;
            }
            else
            {
                pa.Anio = DateTime.Now.Year;
            }
            Constante constante = os.GetObjectByKey<Constante>("MONEDA DEFECTO");
            if (constante != null)
            {
                Moneda mone = os.GetObjectByKey<Moneda>(constante.Valor.Trim());
                if (mone != null)
                    pa.Moneda = mone;
            }
            e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Generar Libro de Compras";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (!(View.CollectionSource.Criteria.ContainsKey("Empresa Actual")) && SecuritySystem.CurrentUser != null)
                View.CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[CompraFactura.Empresa.Oid] = ?", EmpresaOid);

            exportController = Frame.GetController<ExportController>();
            exportController.CustomGetDefaultFileName += ExportController_CustomGetDefaultFileName;
            exportController.CustomExport += new EventHandler<CustomExportEventArgs>(CustomExport);
        }

        private void ExportController_CustomGetDefaultFileName(object sender, CustomGetDefaultFileNameEventArgs e)
        {
            if (View.CurrentObject != null)
                e.FileName = e.FileName + "_" + string.Format("{0:MMMyyyy}", (View.CurrentObject as SBT.Apps.Iva.Module.BusinessObjects.LibroCompra).Fecha);
        }

        protected virtual void CustomExport(object sender, CustomExportEventArgs e)
        {
            if (e.ExportTarget == ExportTarget.Csv)
            {
                CsvExportOptions options = (CsvExportOptions)e.ExportOptions;
                if (options == null)
                {
                    options = new CsvExportOptions();
                }
                options.Separator = ";";
                options.SkipEmptyRows = true;
                options.Encoding = Encoding.UTF8;
                //options.TextExportMode = TextExportMode.Text;
                e.ExportOptions = options;
            }
        }

        protected override void OnDeactivated()
        {
            exportController.CustomGetDefaultFileName -= ExportController_CustomGetDefaultFileName;
            exportController.CustomExport -= new EventHandler<CustomExportEventArgs>(CustomExport);
            base.OnDeactivated();
        }
    }
}
