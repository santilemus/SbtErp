using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Editors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Iva.Module.Controllers;

namespace SBT.Apps.Erp.Blazor.Server.Controllers.Contabilidad
{ 

    [NonController]
    public class LibroVentaContribuyenteListViewControllerWeb : LibroVentaContribuyenteListViewController
    {
        public LibroVentaContribuyenteListViewControllerWeb() : base()
        {
            
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

        protected override void DoExecuteExport(FechaParam parametros)
        {
            base.DoExecuteExport(parametros);
            var empresaOid = (DevExpress.ExpressApp.SecuritySystem.CurrentUser as Usuario).Empresa.Oid;
            string urlFmt = @"ExportService/?typeName=LibroVentaContribuyente&empresaOid={0}&fecha={1:yyyy-MM-dd}";
            DateTime fechaInicio = new DateTime(parametros.Fecha.Year, parametros.Fecha.Month, 01);
            var url = string.Format(urlFmt, empresaOid, fechaInicio);
            var navigationManager = ((BlazorApplication)Application).ServiceProvider.GetRequiredService<NavigationManager>();
            navigationManager.NavigateTo(url, forceLoad: true);
            Application.ShowViewStrategy.ShowMessage("Finalizó exportar los datos");
        }
    }
}
