using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace SBT.Apps.Iva.Module.Controllers
{

    public class LibroVentaContribuyenteListViewController : ViewController<ListView>
    {
        private PopupWindowShowAction pwsaGenerar;
        private PopupWindowShowAction pwsaExportarVentas;
        private ExportController exportController;

        public LibroVentaContribuyenteListViewController(): base()
        {
            this.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaContribuyente);
            pwsaGenerar = new PopupWindowShowAction(this, "LibroVentasContribuyente_Generar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaGenerar.Caption = "Generar";
            pwsaGenerar.CancelButtonCaption = "Cancelar";
            pwsaGenerar.AcceptButtonCaption = "Generar";
            pwsaGenerar.ToolTip = "Clic para generar el libro de ventas a contribuyentes";
            pwsaGenerar.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaContribuyente);
            pwsaGenerar.ImageName = "service";
            pwsaGenerar.CustomizePopupWindowParams += PwsaGenerar_CustomizePopupWindowParams;
            pwsaGenerar.Execute += PwsaGenerar_Execute;

            pwsaExportarVentas = new PopupWindowShowAction(this, "LibroVentasContribuyente_Exportar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaExportarVentas.Caption = "Exportar Ventas";
            pwsaExportarVentas.AcceptButtonCaption = "Exportar";
            pwsaExportarVentas.CancelButtonCaption = "Cancelar";
            pwsaExportarVentas.ActionMeaning = ActionMeaning.Accept;
            pwsaExportarVentas.ToolTip = "Clic para exportar el libro de ventas a contribuyentes al formato requerido para cargarlo en la plataforma de declaración de impuestos";
            pwsaExportarVentas.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaContribuyente);
            pwsaExportarVentas.ImageName = "ExportToCSV";
            pwsaExportarVentas.CustomizePopupWindowParams += PwsaExportarVentas_CustomizePopupWindowParams;
            pwsaExportarVentas.Execute += PwsaExportarVentas_Execute;
        }

        private void PwsaExportarVentas_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            RangoFechaParams pa = (e.PopupWindowViewCurrentObject as RangoFechaParams);
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

        protected virtual void DoExecuteExport(RangoFechaParams parametros)
        {

        }

        protected MemoryStream ToCsvStream(DateTime fechaInicio, DateTime fechaFin)
        {
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            using (SqlConnection conn = new SqlConexion().CreateConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(LibroVentaContribuyenteSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Empresa", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                    var reader = cmd.ExecuteReader();
                    string sFila;
                    byte[] row;
                    UTF8Encoding utf8 = new UTF8Encoding();
                    while (reader.Read())
                    {
                        string dui = !string.IsNullOrEmpty(Convert.ToString(reader["Nit"])) ? string.Empty : Convert.ToString(reader["DUI"]);

                        sFila = $@"";

                        row = utf8.GetBytes(sFila);
                        ms.Write(row, 0, sFila.Length);
                    }
                    reader.Close();
                    conn.Close();
                    return ms;
                }
            }
        }

        private string LibroVentaContribuyenteSql
        {
            get => @"select  ";
        }

        private void PwsaExportarVentas_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Base.Module.BusinessObjects.RangoFechaParams));
            var pa = os.CreateObject<RangoFechaParams>();
            e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Exportar Libro de Ventas";
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
                    ((XPObjectSpace)ObjectSpace).Session.ExecuteSproc("spGeneraLibroVentaContribuyente", EmpresaOid, codMoneda, Convert.ToInt16(pa.Mes), pa.Anio);
                    DateTime inicioMes = new DateTime(Convert.ToInt16(pa.Anio), (int)pa.Mes, 01);
                    CriteriaOperator criteria = new BetweenOperator("Fecha", inicioMes, inicioMes.AddMonths(1).AddDays(-1));
                    View.RefreshDataSource();
                    ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
                    Application.ShowViewStrategy.ShowMessage($@"Libro de Ventas a Contribuyentes Generado", InformationType.Success);
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage($@"Error al generar Libro de Ventas{System.Environment.NewLine}{ex.Message}",
                        InformationType.Error);
                }
            }
            else
                Application.ShowViewStrategy.ShowMessage(@"No se puede ejecutar el proceso de Generación del Libro de Ventas, porque uno o más parámetros son nulos",
                    InformationType.Warning);
        }

        private void PwsaGenerar_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Base.Module.BusinessObjects.LibroIvaParams));
            var pa = os.CreateObject<LibroIvaParams>();
            var EmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            CriteriaOperator condicion = CriteriaOperator.Parse("Venta.Empresa = ? And Cerrado = 1", EmpresaOid);
            var fecha = Convert.ToDateTime(ObjectSpace.Evaluate(typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroVentaContribuyente), CriteriaOperator.Parse("max([Fecha])"), condicion));
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
            e.View.Caption = "Generar Libro de Ventas";
        }
    }
}
