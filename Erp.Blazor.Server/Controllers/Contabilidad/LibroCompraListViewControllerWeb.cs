using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Editors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.Iva.Module.Controllers;
using System.Text;


namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{
    [NonController]
    public class LibroCompraListViewControllerWeb : LibroCompraListViewController // BaseListViewControllerWeb
    {
        public LibroCompraListViewControllerWeb() : base()
        {
            TargetObjectType = typeof(Iva.Module.BusinessObjects.LibroCompra);
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is DxGridListEditor gridListEditor)
            {
                IDxGridAdapter dataGridAdapter = gridListEditor.GetGridAdapter();
                dataGridAdapter.GridModel.ColumnResizeMode = DevExpress.Blazor.GridColumnResizeMode.ColumnsContainer;
            }
        }

        protected override void DoExecuteExport(LibroCompraExportarParams parametros)
        {
            base.DoExecuteExport(parametros);
            var empresaOid = (DevExpress.ExpressApp.SecuritySystem.CurrentUser as Usuario).Empresa.Oid;
            string urlFmt = string.Empty;
            if (parametros.Anexo == EAnexoLibroCompraExportar.LibroCompras)
                urlFmt = @"ExportService/?typeName=LibroCompra&empresaOid={0}&fecha={1:yyyy-MM-dd}";
            else if (parametros.Anexo == EAnexoLibroCompraExportar.Percepcion)
                urlFmt = @"ExportService/?typeName=Percepcion&empresaOid={0}&fecha={1:yyyy-MM-dd}";
            else if (parametros.Anexo == EAnexoLibroCompraExportar.Retencion)
                urlFmt = @"ExportService/?typeName=Retencion&empresaOid={0}&fecha={1:yyyy-MM-dd}";
            else if (parametros.Anexo == EAnexoLibroCompraExportar.SujetoExcluido)
                urlFmt = @"ExportService/?typeName=SujetoExcluido&empresaOid={0}&fecha={1:yyyy-MM-dd}";
            else
                urlFmt = @"ExportService/?typeName=PagoCuenta&empresaOid={0}&fecha={1:yyyy-MM-dd}";
            DateTime fechaInicio = new DateTime(parametros.Fecha.Year, parametros.Fecha.Month, 01);
            var url = string.Format(urlFmt, empresaOid, fechaInicio);
            var navigationManager = ((BlazorApplication)Application).ServiceProvider.GetRequiredService<NavigationManager>();
            navigationManager.NavigateTo(url, forceLoad: true);

            Application.ShowViewStrategy.ShowMessage("Finalizó exportar los datos");

        }
    }
}
