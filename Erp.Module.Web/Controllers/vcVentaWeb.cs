using System;
using System.Linq;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module;
using SBT.Apps.Facturacion.Module;
using System.Web;
using System.IO;
using DevExpress.Data.Filtering;

namespace SBT.Apps.Erp.Module.Web.Controllers
{
    public class vcVentaWeb: ViewControllerBaseWeb
    {
        private PopupWindowShowAction pwsaExportSalesF07Contribuyente;
        private PopupWindowShowAction pwsaExportSalesF07Consumidor;

        public vcVentaWeb(): base()
        { 
            
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Ejecuta la accion para descargar las ventas a contribuyentes a formato csv (anexo1)
        /// 
        /// </summary>
        /// <param name="AMonth"></param>
        /// <param name="AYear"></param>
        /// <remarks>
        /// Info en: https://supportcenter.devexpress.com/ticket/details/t490652/view-controller-download-file
        /// Nota: Please make sure your action that triggers a file download is executed on a postback. For this, set the IsPostBackRequired property of the action's model to True.
        ///   en: https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Web.SystemModule.IModelActionWeb.IsPostBackRequired
        /// </remarks>
        private void DoExportSalesF07Contribuyente(int AMonth, int AYear)
        {
            //Venta vc;
            SbtExport export = new SbtExport(((XPObjectSpace)ObjectSpace).Session.DataLayer);
            /// cambiar, debera de ser del BO que corresponde a 
            export.BOClass = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            export.Format = EExportFormat.Csv;
            export.Separator = ";";
            export.Properties.Add(new ColumnDefinition(export, "Fecha", "{0:dd/MM/yyyy}"));
            export.Properties.Add(new ColumnDefinition(export, "Clase", true));
            export.Properties.Add(new ColumnDefinition(export, "TipoFactura.Codigo", "{0,2}"));
            export.Properties.Add(new ColumnDefinition(export, "AutorizacionCorrelativo.Resolucion", "{0, -19}"));
            export.Properties.Add(new ColumnDefinition(export, "Serie", "{0, -8}"));
            export.Properties.Add(new ColumnDefinition(export, "NoFactura", "{0:00000000}"));
            export.Properties.Add(new ColumnDefinition(export, "Numero", "{0:00000000}"));

            HttpResponse response = HttpContext.Current.Response;
            MemoryStream data = export.Execute(CriteriaOperator.Parse("GetMonth([Fecha]) == ? && GetYear([Fecha]) == ?", AMonth, AYear));
            data.Seek(0, SeekOrigin.Begin);
            byte[] content = data.ToArray();
            response.Clear();
            response.ClearHeaders();
            response.ClearContent();
            response.AddHeader("Content-Disposition", "attachment; filename=" + "IvaF07Anexo1" + ".csv");
            response.AddHeader("Content-Length", content.Length.ToString());
            response.ContentType = "text/csv";
            response.OutputStream.Write(content, 0, content.Length);
            response.Flush();
            response.End();
            data.Close();
            data.Dispose();
        }

    }
}
