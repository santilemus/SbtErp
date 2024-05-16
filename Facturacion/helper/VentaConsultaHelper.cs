using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Iva.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;

namespace SBT.Apps.Facturacion.Module.helper
{
    /// <summary>
    /// Clase con helper method para consultar ventas y retornar el resultado
    /// </summary>
    public class VentaConsultaHelper
    {
        /// <summary>
        /// Obtener los datos para exportar el libro de ventas de contribuyente de acuerdo a la estructura requerida por el MH
        /// </summary>
        /// <param name="objectSpace">objeto que implementa IObjectSpace y que se utilizará para recuperar los datos desde la bd</param>
        /// <param name="empresaOid">Id de la empresa cuyos datos se van a recuperar</param>
        /// <param name="fechaDesde">Fecha de Inicio del período a obtner datos</param>
        /// <param name="fechaHasta">Fecha Fin del período a obtener los datos</param>
        /// <returns>IEnumerable de tipo dinamico con los datos según la estructura y orden requerido para exportarlos </returns>
        public static IEnumerable<dynamic> GetDataLibroVentaContribuyente(IObjectSpace objectSpace, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            var criteria = CriteriaOperator.FromLambda<LibroVentaContribuyente>(x => x.Venta.Empresa.Oid == empresaOid &&
                    x.Fecha.Date >= fechaDesde && x.Fecha.Date <= fechaHasta && x.Venta.Estado != EEstadoFactura.Anulado);
            var datos = objectSpace.GetObjects<LibroVentaContribuyente>(criteria).Select(x => new
            {
                Fecha = string.Format("{0:dd/MM/yyyy}", x.Fecha),
                x.Clase,
                x.TipoDocumento,
                x.AutorizacionDocumento.Resolucion,
                x.AutorizacionDocumento.Serie,
                x.Numero,
                x.NoControlInterno,
                x.Nit,
                x.Cliente.Nombre,
                x.Exenta,
                x.NoSujeta,
                x.GravadaLocal,
                x.DebitoFiscal,
                x.VentaTercero,
                x.DebitoFiscalTercero,
                x.Total,
                x.Dui,
                x.NumeroAnexo
            });
            return datos;
        }

        /// <summary>
        /// Obtener los datos para exportar el libro de ventas a consumidor final de acuerdo a la estructura requerida por el MH
        /// </summary>
        /// <param name="objectSpace">objeto que implementa IObjectSpace y que se utilizará para recuperar los datos desde la bd</param>
        /// <param name="empresaOid">Id de la empresa cuyos datos se van a recuperar</param>
        /// <param name="fechaDesde">Fecha de Inicio del período a obtner datos</param>
        /// <param name="fechaHasta">Fecha Fin del período a obtener los datos</param>
        /// <returns>IEnumerable de tipo dinamico con los datos según la estructura y orden requerido para exportarlos </returns>
        public static IEnumerable<dynamic> GetDataLibroVentaConsumidor(IObjectSpace objectSpace, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            var criteria = CriteriaOperator.FromLambda<LibroVentaConsumidor>(x => x.Empresa.Oid == empresaOid && 
                            x.Fecha.Date >= fechaDesde && x.Fecha.Date <= fechaHasta);
            var datos = objectSpace.GetObjects<LibroVentaConsumidor>(criteria).Select(x => new
            {
                Fecha = string.Format("{0:dd/MM/yyyy}", x.Fecha),
                x.Clase,
                x.TipoDocumento,
                x.AutorizacionDocumento.Resolucion,
                x.AutorizacionDocumento.Serie,
                x.NoControlInternoDel,
                x.NoControlInternoAl,
                x.NoDocumentoDel,
                x.NoDocumentoAl,
                NoCaja = (x.AutorizacionDocumento.Caja != null) ? Convert.ToString(x.AutorizacionDocumento.Caja): string.Empty,
                x.Exenta,
                x.InternaExenta,
                x.NoSujeta,
                x.GravadaLocal,
                x.ExportacionCA,
                x.ExportacionFueraCA,
                x.ExportacionServicio,
                x.VentaZonaFranca,
                x.VentaTercero,
                x.Total,
                x.NumeroAnexo
            });
            return datos;
        }

        /// <summary>
        /// Obtener el detalle de los documentos anulados a la estructura requerida por el MH
        /// </summary>
        /// <param name="objectSpace">objeto que implementa IObjectSpace y que se utilizará para recuperar los datos desde la bd</param>
        /// <param name="empresaOid">Id de la empresa cuyos datos se van a recuperar</param>
        /// <param name="fechaDesde">Fecha de Inicio del período a obtner datos</param>
        /// <param name="fechaHasta">Fecha Fin del período a obtener los datos</param>
        /// <returns>IEnumerable de tipo dinamico con los datos según la estructura y orden requerido para exportarlos </returns>
        public static IEnumerable<dynamic> GetDataDocumentosAnulados(IObjectSpace objectSpace, int empresaOid, DateTime fechaDesde, DateTime fechaHasta)
        {
            var criteria = CriteriaOperator.FromLambda<Venta>(x => x.Empresa.Oid == empresaOid &&
                    x.Fecha.Date >= fechaDesde && x.Fecha.Date <= fechaHasta && x.Estado == EEstadoFactura.Anulado);
            var datos = objectSpace.GetObjects<Venta>(criteria).Select(x => new
            {
                x.AutorizacionDocumento.Resolucion,
                Clase = (int)x.AutorizacionDocumento.Clase,
                PreimpresoDesde = x.NoFactura,
                PreimpresoHasta = x.NoFactura,
                TipoDocumento = (x.TipoFactura.Codigo == "COVE01") ? "03" : (x.TipoFactura.Codigo == "COVE02") ? "01" : "11",
                TipoDetalle = (x.AutorizacionDocumento.Clase != EClaseDocumento.Dte) ? "A" : "D",
                x.AutorizacionDocumento.Serie,
                Desde = x.NoFactura,
                Hasta = x.NoFactura,
                CodigoGeneracion = (x.AutorizacionDocumento.Clase != EClaseDocumento.Dte) ? "" : ""
            }) ;
            return datos;
        }

    }
}
