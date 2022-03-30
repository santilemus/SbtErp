using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module;
using DevExpress.Data.Filtering;
using System.IO;
using SBT.Apps.Facturacion.Module.BusinessObjects;


namespace SBT.Apps.Erp.Module.Win.Controllers
{
    public class vcVentaWin: ViewController 
    {
        //private PopupWindowShowAction pwsaExportSalesF07Contribuyente;
        //private PopupWindowShowAction pwsaExportSalesF07Consumidor;
        public vcVentaWin()
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

        private void DoExportSalesF07Contribuyente(int AMonth, int AYear)
        {
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
            using (FileStream file = new FileStream("archivo.csv", FileMode.Create))
            {
                var source = export.Execute(CriteriaOperator.Parse("GetMonth([Fecha]) == ? && GetYear([Fecha]) == ?", AMonth, AYear));
                source.WriteTo(file);
                file.Close();
                source.Close();
            }
        }
    }
}
