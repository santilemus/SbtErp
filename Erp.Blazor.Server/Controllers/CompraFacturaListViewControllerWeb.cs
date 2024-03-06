using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.SystemModule;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.Controllers;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    [NonController]
    public class CompraFacturaListViewControllerWeb: CompraFacturaListViewController
    {
        public CompraFacturaListViewControllerWeb() : base() 
        {
        }

        protected override void DoExecuteExport(FechaParam parametros)
        {
            base.DoExecuteExport(parametros);
            var empresaOid = (DevExpress.ExpressApp.SecuritySystem.CurrentUser as Usuario).Empresa.Oid;
            string urlFmt = @"ExportService/?typeName=PagoCuenta&empresaOid={0}&fecha={1:yyyy-MM-dd}";
            DateTime fechaInicio = new DateTime(parametros.Fecha.Year, parametros.Fecha.Month, 01);
            var url = string.Format(urlFmt, empresaOid, fechaInicio);
            var navigationManager = ((BlazorApplication)Application).ServiceProvider.GetRequiredService<NavigationManager>();
            navigationManager.NavigateTo(url, forceLoad: true);
            Application.ShowViewStrategy.ShowMessage("Finalizó exportar los datos");
        }

    }
}
