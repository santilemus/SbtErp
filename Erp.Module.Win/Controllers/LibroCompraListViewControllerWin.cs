using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Iva.Module.BusinessObjects;
using SBT.Apps.Iva.Module.Controllers;
using DevExpress.XtraEditors;
using SBT.Apps.Compra.Module.BusinessObjects;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Model;
using SBT.Apps.Compra.Module.helper;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System.IO;

namespace SBT.Apps.Erp.Module.Win.Controllers
{
    public class LibroCompraListViewControllerWin: LibroCompraListViewController
    {
        public LibroCompraListViewControllerWin(): base()
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

        /// <summary>
        /// Implementación específica para la plataforma Windows de la exportación de los datos a formato Csv requerido por el MH
        /// En este caso se esta haciendo utilizando DeveExpress.Spreadsheet para entender como funciona, pero podría cambiarse a
        /// a csvHelper
        /// </summary>
        /// <param name="parametros"></param>
        protected override void DoExecuteExport(LibroCompraExportarParams parametros)
        {
            base.DoExecuteExport(parametros);
            using (DevExpress.Spreadsheet.Workbook wb = new())
            {
                var ws = wb.Worksheets.Add("Hoja1");
                var empresaOid = (DevExpress.ExpressApp.SecuritySystem.CurrentUser as Usuario).Empresa.Oid;
                var fechaDesde = new DateTime(parametros.Fecha.Year, parametros.Fecha.Month, 1);
                var fechaHasta = fechaDesde.AddMonths(1).AddSeconds(-1);
                using var os = Application.CreateObjectSpace(typeof(LibroCompra));
                var datos = CompraConsultaHelper.GetDataLibroCompra(os, empresaOid, fechaDesde, fechaHasta).ToList();
                ws.Import(datos, 0, 0);
                string sFileName = string.Format("{0:00}_LibroCompra_{1:MMMyyyy}.csv", empresaOid, fechaHasta);
                using (XtraSaveFileDialog saveDlg = new XtraSaveFileDialog() { Title = "Exportar Libro Compra", FileName = sFileName })
                {
                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        wb.Options.Export.Csv.ValueSeparator = ';';
                        wb.SaveDocument(saveDlg.FileName, DocumentFormat.Csv);
                    }
                }      
            }
        }
    }
}
