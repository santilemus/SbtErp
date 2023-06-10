using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using SBT.Apps.Base.Module.BusinessObjects;
using System.Configuration;
using SBT.Apps.Base.Module;

namespace SBT.Apps.Iva.Module.Controllers
{
    public class LibroCompraListViewController : ViewController<ListView>
    {
        private PopupWindowShowAction pwsaGenerar;
        private PopupWindowShowAction pwsaExportarCompras;
        private ExportController exportController;

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

            pwsaExportarCompras = new PopupWindowShowAction(this, "LibroCompras_Exportar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaExportarCompras.Caption = "Exportar Compras";
            pwsaExportarCompras.AcceptButtonCaption = "Exportar";
            pwsaExportarCompras.CancelButtonCaption = "Cancelar";
            pwsaExportarCompras.ActionMeaning = ActionMeaning.Accept;
            pwsaExportarCompras.ToolTip = "Clic para exportar el libro de compras al formato requerido para cargarlo en la plataforma de declaración de impuestos";
            pwsaExportarCompras.TargetObjectType = typeof(SBT.Apps.Iva.Module.BusinessObjects.LibroCompra);
            pwsaExportarCompras.ImageName = "ExportToCSV";
            pwsaExportarCompras.CustomizePopupWindowParams += PwsaExportarCompras_CustomizePopupWindowParams;
            pwsaExportarCompras.Execute += PwsaExportarCompras_Execute;
        }

        private void PwsaExportarCompras_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            RangoFechaParams pa = (e.PopupWindowViewCurrentObject as RangoFechaParams);
            try
            {
                DoExecuteExport(pa);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error al exportar Libro de Compras{System.Environment.NewLine}{ex.Message}",
                     InformationType.Error);
            }
        }

        protected MemoryStream ToCsvStream(DateTime fechaInicio, DateTime fechaFin)
        {
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            using (SqlConnection conn = new SqlConexion().CreateConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(LibroCompraSql, conn))
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
                        sFila = $@"{string.Format("{0:dd/MM/yyyy}", reader["Fecha"])};{reader["ClaseDocumento"]};{reader["TipoDocumento"]};
                                {reader["Numero"]};{reader["Nit"]};{reader["NombreProveedor"]};{string.Format("{0:F2}", reader["InternaExenta"])};
                                {string.Format("{0:F2}", reader["InternacionExenta"])};{string.Format("{0:F2}", reader["ImportacionExenta"])};
                                {string.Format("{0:F2}", reader["InternaGravada"])};{string.Format("{0:F2}", reader["InternacionGravadaBien"])};
                                {string.Format("{0:F2}", reader["ImportacionGravadaBien"])};{string.Format("{0:F2}", reader["ImportacionGravadaServicio"])};
                                {string.Format("{0:F2}", reader["CreditoFiscal"])};{string.Format("{0:F2}", reader["Total"])};
                                {dui};{reader["Anexo"]}{System.Environment.NewLine}";
                        row = utf8.GetBytes(sFila);
                        ms.Write(row, 0, sFila.Length);
                    }
                    reader.Close();
                    conn.Close();
                    return ms;
                }
            }
        }

        private string LibroCompraSql
        {
            get => @"select l.Fecha, l.ClaseDocumento, l.TipoDocumento, l.Numero, l.Nit, t.Nombre as NombreProveedor, l.InternaExenta, 
                            l.InternacionExenta, l.ImportacionExenta, l.InternaGravada, l.InternacionGravadaBien, l.ImportacionGravadaBien, 
	                        l.ImportacionGravadaServicio, l.CreditoFiscal, 
	                        coalesce(l.InternaExenta, 0) + coalesce(l.InternacionExenta, 0) + coalesce(l.ImportacionExenta, 0) +
	                        coalesce(l.InternaGravada, 0) + coalesce(l.InternacionGravadaBien, 0) + coalesce(l.ImportacionGravadaBien, 0) +
	                        coalesce(l.IMportacionGravadaServicio, 0) + coalesce(l.CreditoFiscal, 0) + coalesce(CompraExcluido, 0) as Total, 
	                        l.DUI, '03' as Anexo
                      from dbo.LibroCompra l 
                     inner join CompraFactura f
                        on l.CompraFactura = f.Oid
                     inner join Tercero t
                        on l.Proveedor = t.Oid
                     where f.Empresa = @Empresa
                       and l.Fecha between @FechaInicio and @FechaFin ";
        }

        protected virtual void DoExecuteExport(RangoFechaParams parametros)
        {

        }

        private void PwsaExportarCompras_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Base.Module.BusinessObjects.RangoFechaParams));
            var pa = os.CreateObject<RangoFechaParams>();
            e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Exportar Libro de Compras";

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
                    ((XPObjectSpace)ObjectSpace).Session.ExecuteSproc("spGeneraLibroCompra", EmpresaOid, codMoneda, Convert.ToInt16(pa.Mes), pa.Anio);
                    DateTime inicioMes = new DateTime(Convert.ToInt16(pa.Anio), (int)pa.Mes, 01);
                    CriteriaOperator criteria = new BetweenOperator("Fecha", inicioMes, inicioMes.AddMonths(1).AddDays(-1));
                    View.RefreshDataSource();
                    ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
                    Application.ShowViewStrategy.ShowMessage($@"Libro de Compras Generado", InformationType.Success);
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage($@"Error al generar Libro de Compras{System.Environment.NewLine}{ex.Message}",
                        InformationType.Error);
                }
            }
            else
                Application.ShowViewStrategy.ShowMessage(@"No se puede ejecutar el proceso de Generación del Libro de Compras, porque uno o más parámetros son nulos",
                    InformationType.Warning);
        }

        private void PwsaGenerar_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(SBT.Apps.Base.Module.BusinessObjects.LibroIvaParams));
            var pa = os.CreateObject<LibroIvaParams>();
            var EmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
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
            if ((View.ObjectTypeInfo.FindMember("[CompraFactura]") != null && !(View.CollectionSource.Criteria.ContainsKey("Empresa Actual")) && SecuritySystem.CurrentUser != null))
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[CompraFactura.Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);

            exportController = Frame.GetController<ExportController>();
            exportController.CustomGetDefaultFileName += ExportController_CustomGetDefaultFileName;
            exportController.CustomExport += new EventHandler<CustomExportEventArgs>(CustomExport);
        }

        private void ExportController_CustomGetDefaultFileName(object sender, CustomGetDefaultFileNameEventArgs e)
        {
            if (View.CurrentObject != null)
                e.FileName = e.FileName + "_" + string.Format("{0:mmmmYYYY}", (View.CurrentObject as SBT.Apps.Iva.Module.BusinessObjects.LibroCompra).Fecha);
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
