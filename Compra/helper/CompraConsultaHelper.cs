using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.Iva.Module.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SBT.Apps.Compra.Module.helper
{
    /// <summary>
    /// Clase con helper method para consultar compras y retornar el resultado
    /// </summary>
    public class CompraConsultaHelper
    {
        /// <summary>
        /// Obtener los datos para exportar el libro de compras de acuerdo a la estructura requerida por el MH
        /// </summary>
        /// <param name="objectSpace">objeto que implementa IObjectSpace y que se utilizará para recuperar los datos desde la bd</param>
        /// <param name="empresaOid">Id de la empresa cuyos datos se van a recuperar</param>
        /// <param name="fechaDesde">Fecha de Inicio del período a obtner datos</param>
        /// <param name="fechaHasta">Fecha Fin del período a obtener los datos</param>
        /// <returns>IEnumerable de tipo dinamico con los datos según la estructura y orden requerido para exportarlos </returns>
        /// <cambios>
        /// 01/marzo/2024 por SELM
        /// Se agregan las columnas de TipoOperacion, ClasifiacionRenta, Sector, TipoCostoGasto
        /// </cambios>
        public static IEnumerable<dynamic> GetDataLibroCompra(IObjectSpace objectSpace, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            var criteria = CriteriaOperator.FromLambda<LibroCompra>(x => x.CompraFactura.Empresa.Oid == empresaOid &&
                x.Fecha.Date >= fechaDesde && x.Fecha.Date <= fechaHasta && x.CompraFactura.Estado != EEstadoFactura.Anulado);
            var datos = objectSpace.GetObjects<LibroCompra>(criteria).Select(x => new
            {
                Fecha = string.Format("{0:dd/MM/yyyy}", x.Fecha),
                ClaseDocumento = (int)x.ClaseDocumento,
                x.TipoDocumento,
                x.Numero,
                x.Nit,
                x.Proveedor.Nombre,
                x.InternaExenta,
                x.InternacionExenta,
                x.ImportacionExenta,
                x.InternaGravada,
                x.InternacionGravadaBien,
                x.ImportacionGravadaBien,
                x.ImportacionGravadaServicio,
                x.CreditoFiscal,
                x.Total,
                x.Dui,
                x.TipoOperacion,
                x.ClasificacionRenta,
                x.Sector,
                x.TipoCostoGasto,
                x.NumeroAnexo
            });
            return datos;
        }
         

        /// <summary>
        /// Obtener los datos para exportar las percepciones aplicadas a las compras de acuerdo a la estructura requerida por el MH
        /// </summary>
        /// <param name="objectSpace">objeto que implementa IObjectSpace y que se utilizará para recuperar los datos desde la bd</param>
        /// <param name="empresaOid">Id de la empresa cuyos datos se van a recuperar</param>
        /// <param name="fechaDesde">Fecha de Inicio del período a obtner datos</param>
        /// <param name="fechaHasta">Fecha Fin del período a obtener los datos</param>
        /// <returns>IEnumerable de tipo dinamico con los datos según la estructura y orden requerido para exportarlos </returns>
        public static IEnumerable<dynamic> GetDataPercepcion(IObjectSpace objectSpace, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            var criteria = CriteriaOperator.FromLambda<LibroCompra>(x => x.CompraFactura.Empresa.Oid == empresaOid &&
                x.Fecha.Date >= fechaDesde && x.Fecha.Date <= fechaHasta && x.CompraFactura.Estado != EEstadoFactura.Anulado);
            var datos = objectSpace.GetObjects<LibroCompra>(criteria).Select(x => new
            {
                x.Nit,
                Fecha = string.Format("{0:dd/MM/yyyy}", x.Fecha),
                x.TipoDocumento,
                x.Serie,
                x.Numero,
                x.InternaGravada,
                x.IvaPercibido,
                x.Dui,
                x.AnexoPercepcion
            });
            return datos;
        }

        /// <summary>
        /// Obtener los datos para exportar el anexo con las retenciones de renta acumuladas por proveedor (Solo de proveedores, en este archivo)
        /// </summary>
        /// <param name="objectSpace">objeto que implementa IObjectSpace y que se utilizará para recuperar los datos desde la bd</param>
        /// <param name="empresaOid">Id de la empresa cuyos datos se van a recuperar</param>
        /// <param name="fechaDesde">Fecha de Inicio del período a obtner datos</param>
        /// <param name="fechaHasta">Fecha Fin del período a obtener los datos</param>
        /// <returns>IEnumerable de tipo dinamico con los datos según la estructura y orden requerido para exportarlos </returns>
        public static IEnumerable<dynamic> GetDataPagoCuentaAnexo(IObjectSpace objectSpace, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            var criteria = CriteriaOperator.FromLambda<CompraFactura>(x => x.Empresa.Oid == empresaOid && x.Fecha.Date >= fechaDesde && x.Fecha.Date <= fechaHasta && x.Renta > 0.00m);
            var datos = objectSpace.GetObjects<CompraFactura>(criteria).Select(x => new
            {
                Domiciliado = x.Proveedor.DireccionPrincipal.Pais.Codigo == "SLV" ? 1 : 0,
                Pais = x.Proveedor.DireccionPrincipal.Pais.Codigo == "SLV" ? "9300" : "XXXX", // modificar para que el codigo del MH de pais este en el catalogo de zonageo
                x.Proveedor.Nombre,
                Nit = x.Proveedor.Nit ?? string.Empty,
                Dui = string.IsNullOrEmpty(x.Proveedor.Nit) ? x.Proveedor.Dui : string.Empty,
                CodigoIngreso = "11",
                Devengado = x.Exenta + x.Gravada + x.NoSujeta,
                Bonificacion = 0.00m,
                x.Renta,
                AguinaldoExento = 0.0m,
                AguinaldoGravado = 0.0m,
                AFP = 0.0m,
                Isss = 0.0m,
                Inpep = 0.0m,
                Ipsfa = 0.0m,
                Cefafa = 0.0m,
                Bmg = 0.0m,
                IssOvm = 0.0m,
                Periodo = string.Format("{0:MMyyyy}", x.Fecha)
            });
            return datos;
        }
    }
}
