using DevExpress.Data.ExpressionEditor;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Facturacion.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using SBT.eFactura.Dte.Poco;
using SBT.eFactura.Dte.Poco.Send;
using System;
using System.Linq;

namespace SBT.Apps.Facturacion.Module.helper
{
    public class CcfToPoco
    {
        public CcfToPoco(Venta vta, int version, string ambiente, int modeloFact, int tipoTransmision)
        {
            SBT.eFactura.Dte.Poco.Send.FeCcf ccf = new SBT.eFactura.Dte.Poco.Send.FeCcf();
            // agregamos los datos de identificacion del dte
            ccf.Identificacion = new eFactura.Dte.Poco.Identificacion()
            {
                Version = version,    // hay que parametrizarlo en algún lugar. La versión es por tipo de dte
                Ambiente = ambiente,
                TipoDte = "03", // credito fiscal
                NumeroControl = vta.NumeroControl,
                CodigoGeneracion = Convert.ToString(vta.CodigoGeneracion),
                FechaEmision = vta.Fecha.Date,
                HoraEmision = new TimeSpan(vta.Fecha.Hour, vta.Fecha.Minute, vta.Fecha.Second),
                TipoOperacion = tipoTransmision, 
                TipoMoneda = "USD",
                TipoModelo = modeloFact     
            };

            // agregamos los datos del emisor del dte
            ccf.Emisor = new eFactura.Dte.Poco.Emisor()
            {
                Nit = vta.Empresa.Nit,
                Nrc = vta.Empresa.Nrc,
                Nombre = vta.Empresa.RazonSocial,
                CodigoActividad = vta.Empresa.Giros.FirstOrDefault()?.ActEconomica.Codigo,
                DescripcionActividad = vta.Empresa.Giros.FirstOrDefault()?.ActEconomica.Concepto,
                Direccion = new eFactura.Dte.Poco.Direccion()
                { Departamento = vta.Empresa.Provincia.Codigo.Substring(3, 2), Municipio = vta.Empresa.Ciudad.Codigo.Substring(5, 2), Complemento = vta.Empresa.Direccion },
                Telefono = vta.Empresa.Telefonos.FirstOrDefault<EmpresaTelefono>(x => x.Telefono.Tipo == TipoTelefono.Pbx)?.Telefono.Numero ?? string.Empty,
                Correo = vta.Empresa.EMail,
                TipoEstablecimiento = "", // revisar que va acá
                CodEstable = "", // revisar que va acá
                CodigoPuntoVenta = "" // solo cuando aplique
            };
            if (ccf.Emisor.Telefono == string.Empty)
                ccf.Emisor.Telefono = vta.Empresa.Telefonos.FirstOrDefault<EmpresaTelefono>(x => x.Empresa.Oid == vta.Empresa.Oid)?.Telefono.Numero ?? string.Empty;
            // agregamos los datos del receptor
            ccf.Receptor = new eFactura.Dte.Poco.Emisor()
            {
                Nit = vta.Cliente.Documentos.FirstOrDefault<TerceroDocumento>(x => x.Tercero.Oid == vta.Cliente.Oid && x.Tipo.Codigo == "NIT")?.Numero ?? string.Empty,
                Nrc = vta.Nrc?.Numero ?? string.Empty,
                CodEstable = vta.Cliente.Oid,
                Nombre = vta.Cliente.Nombre,
                Direccion = new eFactura.Dte.Poco.Direccion()
                {
                    // revisar si se utiliza direccion de entrega
                    Departamento = vta.Cliente.DireccionPrincipal.Provincia.Codigo.Substring(4, 2),
                    Municipio = vta.Cliente.DireccionPrincipal.Ciudad.Codigo.Substring(6, 2),
                    Complemento = vta.Cliente.DireccionPrincipal.Direccion
                },
                Telefono = vta.Cliente.Telefonos.FirstOrDefault<TerceroTelefono>(x => x.Tercero.Oid == vta.Cliente.Oid &&
                    x.Telefono.Tipo == TipoTelefono.Pbx || x.Telefono.Tipo == TipoTelefono.Fijo)?.Telefono.Numero ?? string.Empty,
                Correo = vta.Cliente.EMail,
                CodigoActividad = vta.Giro.ActEconomica.Codigo,
                DescripcionActividad = vta.Giro.ActEconomica.Concepto,
                TipoEstablecimiento = ""  // revisar que va aca
            };
            if (ccf.Receptor.Telefono  == string.Empty)
                ccf.Receptor.Telefono = vta.Cliente.Telefonos.FirstOrDefault<TerceroTelefono>(x => x.Tercero.Oid == vta.Cliente.Oid)?.Telefono.Numero;
            // agregamos el cuerpo del documento (detalle del crédito fiscal)
            ccf.CuerpoDocumento = vta.Detalles.Select((p, idx) => new CuerpoDocumento
                    {
                        NumeroItem = idx + 1,
                        TipoItem = p.Producto.Categoria.Clasificacion == Producto.Module.BusinessObjects.EClasificacion.Servicios ? 2 : 1,
                        NumeroDocumento = ccf.Identificacion.CodigoGeneracion,
                        Codigo = p.Producto.Codigo,
                        CodTributo = "null",
                        Descripcion = p.Descripcion ?? p.Producto.Nombre,
                        Cantidad = p.Cantidad,
                        UnidadMedida = 59, // unidad. Se debe agregar el atributo de unidad de medida al catalogo de producto
                        PrecioUnidad = p.PrecioUnidad,
                        MontoDescuento = 0.0m,
                        VentaNoSujeta = Convert.ToDecimal(p.NoSujeta),
                        VentaExenta = Convert.ToDecimal(p.Exenta),
                        VentaGravada = Convert.ToDecimal(p.Gravada),
                        Psv = 0.0m,  // precio sugerido de venta *solo informativo
                        NoGravado = 0.0m // detalle de cargos o abonos al receptor que no afectan la base imponible 
                    }).ToList();
            // ahora va el resumen
        } 


    }
}
