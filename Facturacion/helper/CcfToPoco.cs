﻿using DevExpress.CodeParser;
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
        const int fVersion = 1;
        public CcfToPoco(Venta vta, int version, string ambiente, int modeloFact, int tipoTransmision)
        {
            SBT.eFactura.Dte.Poco.Send.FeCcf ccf = new SBT.eFactura.Dte.Poco.Send.FeCcf();
            // agregamos los datos de identificacion del dte
            ccf.Identificacion = new eFactura.Dte.Poco.Identificacion()
            {
                Version = version,    // hay que parametrizarlo en algún lugar. La versión es por tipo de dte
                Ambiente = ambiente,
                TipoDte = vta.TipoFactura.CodigoAlterno, // credito fiscal
                NumeroControl = vta.NumeroControl,
                CodigoGeneracion = Convert.ToString(vta.CodigoGeneracion).ToUpper(),
                FechaEmision = vta.Fecha.Date,
                HoraEmision = new TimeSpan(vta.Fecha.Hour, vta.Fecha.Minute, vta.Fecha.Second),
                TipoOperacion = tipoTransmision, 
                TipoMoneda = vta.Moneda.Codigo,
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
                TipoEstablecimiento = (vta.Agencia.Role != ETipoRoleUnidad.Departamento) ? string.Format("{0:00}", (int)vta.Agencia.Role) : "20",
                //CodEstableMH = "", // no va por el momento
                CodEstable = vta.Agencia.Codigo 
                //CodigoPuntoVentaMH = "", // codigo del punto de venta (emisor) asignado por el MH (no va por el moento)
                //CodigoPuntoVenta = ""  // codigo del punto de venta (emisor) asignado por el contribuyente (no va por el momento)
            };
            if (ccf.Emisor.Telefono == string.Empty)
                ccf.Emisor.Telefono = vta.Empresa.Telefonos.FirstOrDefault<EmpresaTelefono>(x => x.Empresa.Oid == vta.Empresa.Oid)?.Telefono.Numero ?? string.Empty;
            // agregamos los datos del receptor
            ccf.Receptor = new eFactura.Dte.Poco.ReceptorCcf()
            {
                Nit = vta.Cliente.Documentos.FirstOrDefault<TerceroDocumento>(x => x.Tercero.Oid == vta.Cliente.Oid && x.Tipo.Codigo == "NIT")?.Numero ?? string.Empty,
                Nrc = vta.Nrc?.Numero ?? string.Empty,
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
                        UnidadMedida = Convert.ToInt32(p.UnidadMedida.CodigoDte), 
                        PrecioUnidad = p.PrecioUnidad,
                        MontoDescuento = 0.0m,
                        VentaNoSujeta = Convert.ToDecimal(p.NoSujeta),
                        VentaExenta = Convert.ToDecimal(p.Exenta),
                        VentaGravada = Convert.ToDecimal(p.Gravada),
                        Psv = 0.0m,  // precio sugerido de venta *solo informativo
                        NoGravado = 0.0m // detalle de cargos o abonos al receptor que no afectan la base imponible 
                    }).ToList();
            // ahora va el resumen
            ccf.Resumen = new Resumen()
            {
                TotalNoSujeta = (decimal)vta.NoSujeta,
                TotalExenta = (decimal)vta.Exenta,
                TotalGravada = (decimal)vta.Gravada,
                SubTotalVentas = (decimal)vta.SubTotal,
                // revisar mas adelante, cuando se hagan los cambios en el BO para obtener el valor de los descuentos
                DescuentoNoSujeta = 0.0m,
                DescuentoExenta = 0.0m,
                DescuentoGravada = 0.0m,
                TotalDescuento = 0.0m,
                PorcentajeDescuento = 0.0m
                // faltan datos aqui
            };
        } 


    }
}
