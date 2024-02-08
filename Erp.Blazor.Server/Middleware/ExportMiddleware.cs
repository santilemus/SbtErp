using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Http.Extensions;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Iva.Module.BusinessObjects;
using System.Text;
using CsvHelper;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp;
using CsvHelper.Configuration;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.Compra.Module.helper;
using SBT.Apps.Facturacion.Module.helper;

namespace SBT.Apps.Erp.Blazor.Server.Middleware
{
    internal class ExportMiddleware
    {
        private readonly RequestDelegate next;
        public ExportMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Crear un Memory stream de los datos recibidos en el parametro
        /// </summary>
        /// <typeparam name="T">El Tipo de datos de IList</typeparam>
        /// <param name="datos">IList con los datos a exportar a Csv</param>
        /// <returns>MemoryStream con los datos exportados a formato Csv</returns>
        private byte[] CreateCsvStream<T>(IList<T> datos)
        {
            MemoryStream ms = new MemoryStream();
            UTF8Encoding utf8 = new UTF8Encoding();
            using StreamWriter writer = new StreamWriter(ms, utf8);
            var config = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false
            };
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(datos);
            }
            return ms.ToArray();
        }

        private IObjectSpaceFactory objectSpaceFactory;
        private string GetParameter(IQueryCollection parameters, string paramName)
        {
            return parameters.ContainsKey(paramName) ? parameters[paramName] : string.Empty;
        }

        private IObjectSpace CreateObjectSpace(IXafApplicationProvider applicationProvider, Type tipo)
        {
            var app = applicationProvider.GetApplication();
            return app.CreateObjectSpace(tipo);
        }

        private dynamic GetDataLibroCompra(IXafApplicationProvider applicationProvider, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            using var objectSpace = CreateObjectSpace(applicationProvider, typeof(LibroCompra));
            return CompraConsultaHelper.GetDataLibroCompra(objectSpace, empresaOid, fechaDesde, fechaHasta).ToList();
        }

        private dynamic GetDataPercepcion(IXafApplicationProvider applicationProvider, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            using var objectSpace = CreateObjectSpace(applicationProvider, typeof(LibroCompra));
            return CompraConsultaHelper.GetDataPercepcion(objectSpace, empresaOid, fechaDesde, fechaHasta).ToList();
        }

        private dynamic GetDataLibroVentaContribuyente(IXafApplicationProvider applicationProvider, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            using var objectSpace = CreateObjectSpace(applicationProvider, typeof(LibroVentaContribuyente));
            return VentaConsultaHelper.GetDataLibroVentaContribuyente(objectSpace, empresaOid, fechaDesde, fechaHasta).ToList();
        }

        private dynamic GetDataLibroVentaConsumidor(IXafApplicationProvider applicationProvider, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            using var objectSpace = CreateObjectSpace(applicationProvider, typeof(LibroVentaConsumidor));
            return VentaConsultaHelper.GetDataLibroVentaConsumidor(objectSpace, empresaOid, fechaDesde, fechaHasta).ToList();
        }

        private dynamic GetPagoCuentaAnexo(IXafApplicationProvider applicationProvider, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            using var objectSpace = CreateObjectSpace(applicationProvider, typeof(CompraFactura));
            return CompraConsultaHelper.GetDataPagoCuentaAnexo(objectSpace, empresaOid, fechaDesde, fechaHasta).ToList();
        }


        public async Task InvokeAsync(HttpContext httpContext, IXafApplicationProvider applicationProvider)
        {
            if (httpContext.Request.Path.Value!.TrimStart('/').StartsWith("ExportService", StringComparison.Ordinal))
            {
                var fechaDesde = Convert.ToDateTime(GetParameter(httpContext.Request.Query, "fecha"));
                var fechaHasta = fechaDesde.AddMonths(1).AddSeconds(-1);
                var empresaOid = Convert.ToInt32(GetParameter(httpContext.Request.Query, "empresaOid"));
                dynamic datos = null;
                string sFileName = $@"attachment; filename={string.Format("{0:00}", empresaOid)}_";
                switch (GetParameter(httpContext.Request.Query, "typeName"))
                {
                    case "LibroCompra":
                        {
                            datos = GetDataLibroCompra(applicationProvider, empresaOid, fechaDesde, fechaHasta);
                            sFileName = sFileName + string.Format("LibroCompra_{0:MMMyyyy}.csv", fechaHasta);
                            break;
                        }
                    case "Percepcion":
                        {
                            datos = GetDataPercepcion(applicationProvider, empresaOid, fechaDesde, fechaHasta);
                            sFileName = sFileName + string.Format("Percepcion_{0:MMMyyyy}.csv", fechaHasta);
                            break;
                        }
                    case "LibroVentaContribuyente":
                        {
                            datos = GetDataLibroVentaContribuyente(applicationProvider, empresaOid, fechaDesde, fechaHasta);
                            sFileName = sFileName + string.Format("LibroVentaContribuyente_{0:MMMyyyy}.csv", fechaHasta);
                            break;
                        }
                    case "VentaConsumidorFinal":
                        {
                            datos = GetDataLibroVentaConsumidor(applicationProvider, empresaOid, fechaDesde, fechaHasta);
                            sFileName = sFileName + string.Format("LibroVentaConsumidor_{0:MMMyyyy}.csv", fechaHasta);
                            break;
                        }
                    case "PagoCuenta":
                        {
                            datos = GetPagoCuentaAnexo(applicationProvider, empresaOid, fechaDesde, fechaHasta);
                            sFileName = sFileName + string.Format("PagoCuenta_{0:MMMyyyy}.csv", fechaHasta);
                            break;
                        }
                }
                if (datos == null)
                    return; // aqui habria que generar log indicando que no se genero nada
                using MemoryStream ms = new MemoryStream(CreateCsvStream(datos));
                ms.Position = 0;
                httpContext.Response.ContentType = "application/csv";
                //httpContext.Response.Headers[key: "Content-Disposition"] = "attachment; filename=nombreArchivo.csv";
                httpContext.Response.Headers[key: "Content-Disposition"] = sFileName;
                await StreamCopyOperation.CopyToAsync(ms, httpContext.Response.Body, new long?(), 65536, httpContext.Response.HttpContext.RequestAborted);
            }
            else
                await next(httpContext);
        }
    }
}
