using DevExpress.Web;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Iva.Module.Controllers;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    /// <summary>
    /// ver https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Web.SystemModule.WebExportController
    /// https://docs.devexpress.com/eXpressAppFramework/113287/shape-export-print-data/printing-exporting-in-listview/how-to-customize-the-export-action-behavior
    /// </summary>
    public class LibroCompraListViewControllerWeb2 : LibroCompraListViewController
    {
        public LibroCompraListViewControllerWeb2()
        {

        }

        protected override void DoExecuteExport(RangoFechaParams parametros)
        {
            base.DoExecuteExport(parametros);
            try
            {
                using (MemoryStream ms = ToCsvStream(parametros.FechaInicio, parametros.FechaFin))
                {
                    using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
                    {
                        //HttpResponse response = HttpContext.Current.Response;
                        HttpResponse response = HttpContext.Current.ApplicationInstance.Response;
                        response.Clear();
                        response.ClearHeaders();
                        response.ContentType = "application/x-msdownload";
                        //response.AddHeader("Content-Type", "application/csv");
                        response.AddHeader("Content-Disposition", "attachment; filename=LibroCompras.csv");
                        ms.Position = 0;
                        response.BinaryWrite(ms.ToArray());
                        //ms.WriteTo(response.OutputStream); //works too
                        response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    ms.Close();
                }
            }
            catch
            {
                throw;
            }
        }

        protected override void CustomExport(object sender, DevExpress.ExpressApp.SystemModule.CustomExportEventArgs e)
        {
            base.CustomExport(sender, e);
            if (e.ExportTarget == DevExpress.XtraPrinting.ExportTarget.Csv)
            {
                e.Handled = true;
                var ms = ToCsvStream(DateTime.Now, DateTime.Now);
                ms.Position = 0;
                ms.CopyTo(e.Stream);
                //  e.Stream = ToCsvStream(DateTime.Now, DateTime.Now);
            }
            ASPxGridViewExporter exporter = e.Printable as ASPxGridViewExporter;
            if (exporter != null)
            {
                exporter.ExportSelectedRowsOnly = true;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

        }
    }
}
