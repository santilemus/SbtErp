using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Iva.Module.BusinessObjects;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    public class LibroVentaConsumidorListViewController: ViewController<ListView>
    {
        private PopupWindowShowAction pwsaGenerar;
        private PopupWindowShowAction pwsaExportarVentas;
        private static string ParametrosNulos = @"No se puede ejecutar el proceso de Generación del Libro de Ventas, porque uno o más parámetros son nulos";
        private static string Error1 = @"Error al {0} Libro de Ventas{1}{2}";
        private static string Informacion1 = @"Libro de Ventas a Contribuyentes Generado";

        public LibroVentaConsumidorListViewController(): base()
        {
            this.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaConsumidor);
            pwsaGenerar = new PopupWindowShowAction(this, "LibroVentasConsumidor_Generar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaGenerar.Caption = "Generar";
            pwsaGenerar.CancelButtonCaption = "Cancelar";
            pwsaGenerar.AcceptButtonCaption = "Generar";
            pwsaGenerar.ToolTip = "Clic para generar el libro de ventas a consumidor final";
            pwsaGenerar.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaConsumidor);
            pwsaGenerar.ImageName = "service";
            pwsaGenerar.CustomizePopupWindowParams += PwsaGenerar_CustomizePopupWindowParams;
            pwsaGenerar.Execute += PwsaGenerar_Execute;

            pwsaExportarVentas = new PopupWindowShowAction(this, "LibroVentasConsumidor_Exportar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaExportarVentas.Caption = "Exportar";
            pwsaExportarVentas.AcceptButtonCaption = "Exportar";
            pwsaExportarVentas.CancelButtonCaption = "Cancelar";
            pwsaExportarVentas.ActionMeaning = ActionMeaning.Accept;
            pwsaExportarVentas.ToolTip = "Clic para exportar el libro de ventas a consumidor final al formato requerido para cargarlo en la plataforma de declaración de impuestos";
            pwsaExportarVentas.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaContribuyente);
            pwsaExportarVentas.ImageName = "ExportToCSV";
            pwsaExportarVentas.CustomizePopupWindowParams += PwsaExportarVentas_CustomizePopupWindowParams;
            pwsaExportarVentas.Execute += PwsaExportarVentas_Execute;
        }

        private void PwsaExportarVentas_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            FechaParam pa = (e.PopupWindowViewCurrentObject as FechaParam);
            var empresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            var fechaInicio = new DateTime(pa.Fecha.Year, pa.Fecha.Month, 01);
            var fechaFin = fechaInicio.AddMonths(1).AddSeconds(-1);
            CriteriaOperator criteria = CriteriaOperator.FromLambda<LibroVentaConsumidor>(x => x.Empresa.Oid == empresaOid &&
                                            x.Fecha >= fechaInicio && x.Fecha <= fechaFin);
            ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
            try
            {
                DoExecuteExport(pa);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error al exportar Libro de Ventas{System.Environment.NewLine}{ex.Message}",
                     InformationType.Error);
            }
        }

        protected virtual void DoExecuteExport(FechaParam parametros)
        {
        }

        private void PwsaExportarVentas_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(FechaParam));
            var pa = os.CreateObject<FechaParam>();
            IModelView detailViewModel = Application.FindModelView("FechaParam_DetailView_MonthYear");  // Fecha con formato MM/yyyy
            if (detailViewModel != null)
                e.View = Application.CreateDetailView(os, detailViewModel, false);
            else
                e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Exportar Libro de Ventas";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.ObjectTypeInfo.FindMember("Empresa") != null && !(View.CollectionSource.Criteria.ContainsKey("Empresa Actual")) && SecuritySystem.CurrentUser != null)
                View.CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
        }

        private void PwsaGenerar_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            LibroIvaParams pa = (e.PopupWindowViewCurrentObject as LibroIvaParams);
            if (pa.Anio != null && pa.Mes != null)
            {
                var EmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
                try
                {
                    string codMoneda = pa.Moneda.Codigo;
                    ((XPObjectSpace)ObjectSpace).Session.ExecuteSproc("spGeneraLibroVentaConsumidor", EmpresaOid, codMoneda, Convert.ToInt16(pa.Mes), pa.Anio);
                    DateTime inicioMes = new DateTime(Convert.ToInt16(pa.Anio), (int)pa.Mes, 01);
                    CriteriaOperator criteria = new BetweenOperator("Fecha", inicioMes, inicioMes.AddMonths(1).AddDays(-1));
                    View.RefreshDataSource();
                    ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, CriteriaOperator.Parse("Empresa.Oid = ? And [Fecha] Between(?, ?)",
                        ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid, inicioMes, inicioMes.AddMonths(1).AddMinutes(-1)));
                    Application.ShowViewStrategy.ShowMessage(Informacion1, InformationType.Success);
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage(string.Format(Error1, "Generar", System.Environment.NewLine, ex.Message), InformationType.Error);
                }
            }
            else
                Application.ShowViewStrategy.ShowMessage(ParametrosNulos, InformationType.Warning);
            var resultado = ObjectSpace.GetObjects<LibroVentaConsumidor>(CriteriaOperator.FromLambda<LibroVentaConsumidor>(x => x.Empresa.Oid == 1)).
                Select(xy => new { xy.Clase, xy.Correlativo, xy.Exenta }).ToList();
        }

        private void PwsaGenerar_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Base.Module.BusinessObjects.LibroIvaParams));
            var pa = os.CreateObject<LibroIvaParams>();
            var EmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            CriteriaOperator condicion = CriteriaOperator.Parse("Empresa = ? And Cerrado = 1", EmpresaOid);
            var fecha = Convert.ToDateTime(ObjectSpace.Evaluate(typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaConsumidor), CriteriaOperator.Parse("max([Fecha])"), condicion));
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
            e.View.Caption = "Generar Libro de Ventas a Consumidor";
        }
    }
}
