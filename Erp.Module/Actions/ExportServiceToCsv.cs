using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using DevExpress.Spreadsheet;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.Xpo;
using SBT.Apps.Producto.Module.BusinessObjects;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Erp.Module.Actions
{
    public class ExportServiceToCsv
    {
        IObjectSpace objectSpace;
        public ExportServiceToCsv(IObjectSpace objectSpace)
        {
            this.objectSpace = objectSpace;
        }

        public void Import<T>(IWorkbook workbook, IQueryable<T> query)
        {
            // 1. Importar los datos
            if (workbook == null || query == null)
            {
                Logger.Instance.Log(@"Workbook instance or query is null", LogLevel.Error, 0);
                return;
            }
            Worksheet worksheet = workbook.Worksheets["Hoja1"];
            if (worksheet == null)
            {
                worksheet = workbook.Worksheets.Add();
                worksheet.Name = "Hoja1";
            }
            worksheet.Import(query.ToList(), 0, 0);
        }

        public MemoryStream Export<T>(IQueryable query)
        {
            IObjectSpace os;
            

            IWorkbook workbook = new Workbook();
            //Import(workbook, query);
            using MemoryStream ms = new MemoryStream();
            workbook.SaveDocument(ms, DocumentFormat.Csv);
            return ms; 
        }
    }
}
