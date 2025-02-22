USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spGeneraLibroVentaConsumidor]    Script Date: 12/6/2023 21:40:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 09/04/2021
-- Description:	Generar Libro de Ventas a Consumidor Final - IVA
-- Modificaciones
-- 12/junio/2023 por SELM
-- 1. Optimizaciones
-- 1.1. Se agrega sentencia para ver si hay registros en el mes que se desea generar el libro
-- 2.2. Si no hay registros en el mes se aplica de una sola vez el insert, sino realiza el merge
-- 2.3. Se quita del merge el delete porque se comprobo que es problematico en ciertos casos 
--      sino se consideran todas las condiciones para borrar del destino, unicamente los registros 
--      que no existen en el origen
-- 2. Correcciones
-- 2.1. Se quita la moneda no tiene sentido e igual ValorMoneda de la tabla del LibroVentaConsumidor
-- 2.2. Se quito del from la tabla VentaDestino (detalle), Producto y CategoriaProducto, porque se 
--      agrego al encabezado de la factura la columna ExportacionServicio
-- 13/junio/2023 por SELM
-- Se agrega la columna correlativo para cumplir con lo establecido en el reglamento de aplicacion del C.T
-- ================================================================================================
ALTER procedure [dbo].[spGeneraLibroVentaConsumidor](
   @Empresa     int,
   @Moneda      varchar(3),
   @Mes         int,
   @Anio        int)
as
begin
  set nocount on
  declare @corrInicio int = 0;
  declare @factorCambio numeric(12,2);

  select @factorCambio = m.FactorCambio from Moneda m
   where m.Codigo = @Moneda

  select @corrInicio = coalesce(max(Correlativo), 0) from LibroVentaContribuyente x
   inner join Venta v
	  on x.Venta = v.Oid
   where v.Empresa = @Empresa
	 and Year(x.Fecha) = @Anio
	 and v.Estado != 2 -- Anulado
	 and v.TipoFactura in ('COVE02', 'COVE03', 'COVE04', 'COVE05')

  select count(*) from LibroVentaConsumidor
   where Empresa = @Empresa
     and Month(Fecha) = @Mes 
	 and Year(Fecha)  = @Anio 

  if (@@rowcount > 0)
  begin
    insert into LibroVentaConsumidor
	       (Empresa, Correlativo, Fecha, AutorizacionDocumento, TipoDocumento, NoControlInternoDel, NoControlInternoAl, 
		    NoDocumentoDel, NoDocumentoAl, Exenta, InternaExenta, NoSujeta, GravadaLocal, ExportacionCA, 
			ExportacionFueraCA, ExportacionServicio, VentaZonaFranca, VentaTercero)
	select v.Empresa, @CorrInicio + row_number() over(partition by v.Empresa, Year(v.Fecha) order by v.Fecha),
	       v.Fecha, v.AutorizacionDocumento, 
	       iif(v.TipoFactura = 'COVE02', '01', iif(v.TipoFactura = 'COVE04', '02', iif(v.TipoFactura = 'COVE05', '10', iif(v.TipoFactura = 'COVE03', '11', null)))),
	       min(v.Numero), max(v.Numero), min(v.NoFactura), max(v.NoFactura), sum(iif(c.TipoContribuyente = 0, 
		   coalesce(v.Exenta, 0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)),   -- vta exenta a contribuyentes
			-- vta exenta a misiones diplomaticas, o. internacionales
		   sum(iif(c.TipoContribuyente = 1, coalesce(v.Exenta, 0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)),   
		   sum(coalesce(v.NoSujeta, 0.0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio), 
		   sum(iif(v.TipoFactura in ('COVE02', 'COVE04', 'COVE05') and v.TerceroNoDomiciliado is null, 
				(coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- gravado local
		   sum(iif(v.TipoFactura = 'COVE03' and dir.Pais in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and v.ExportacionServicio = 0, 			          
				(coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones a CA
		   sum(iif(v.TipoFactura = 'COVE03' and dir.Pais not in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and v.ExportacionServicio = 0, 			          
				(coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones fuera de CA
		   sum(iif(v.TipoFactura = 'COVE03' and dir.Pais not in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and v.ExportacionServicio = 1, 			          
				(coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones de servicios
		   sum(iif(v.TipoFactura = 'COVE02' and v.GravadaTasaCero is null, 
				(coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- ventas a zona franca y dpa con factura consumidor				  
		   sum(iif(v.TipoFactura in ('COVE02', 'COVE04', 'COVE05') And v.TerceroNoDomiciliado is not null, 
				(coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0))  -- venta a cta de tercero no domiciliado
	 from Venta v
	inner join FacAutorizacionDoc a
	   on v.AutorizacionDocumento = a.OID
	inner join Tercero c
	   on v.Cliente = c.OID
	 left join TerceroDocumento nit
	   on v.ClienteDocumento = nit.OID
	 left join TerceroDocumento rc
	   on v.NRC = rc.OID
	 left join TerceroDireccion dir
	   on c.DireccionPrincipal = dir.Oid
	 left join FacCaja caja
	   on a.Caja = caja.Oid
	where v.Empresa = @Empresa
	  and Month(v.Fecha) = @Mes
	  and Year(v.Fecha) = @Anio
	  and v.Estado != 2 -- Anulado
	  and v.TipoFactura in ('COVE02', 'COVE03', 'COVE04', 'COVE05')
	group by v.Empresa, caja.NoCaja, v.Fecha, v.AutorizacionDocumento, v.TipoFactura
  end
  else
  begin
	  -- agregamos la informacion de las ventas.
	  merge LibroVentaConsumidor as dest
	      using
		  (select v.Empresa, @CorrInicio + row_number() over(partition by v.Empresa, Year(v.Fecha) order by v.Fecha), 
		          caja.NoCaja, v.Fecha, v.AutorizacionDocumento, 
		          iif(v.TipoFactura = 'COVE02', '01', iif(v.TipoFactura = 'COVE04', '02', iif(v.TipoFactura = 'COVE05', '10', iif(v.TipoFactura = 'COVE03', '11', null)))), 
		          min(v.Numero), max(v.Numero), min(v.NoFactura), max(v.NoFactura), sum(iif(c.TipoContribuyente = 0, 
				  coalesce(v.Exenta, 0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)),   -- vta exenta a contribuyentes
				  -- vta exenta a misiones diplomaticas, o. internacionales
				  sum(iif(c.TipoContribuyente = 1, coalesce(v.Exenta, 0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)),   
				  sum(coalesce(v.NoSujeta, 0.0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio), 
				  sum(iif(v.TipoFactura in ('COVE02', 'COVE04', 'COVE05') and v.TerceroNoDomiciliado is null, 
				      (coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- gravado local
				  sum(iif(v.TipoFactura = 'COVE03' and dir.Pais in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and v.ExportacionServicio = 0, 			          
				      (coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones a CA
				  sum(iif(v.TipoFactura = 'COVE03' and dir.Pais not in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and v.ExportacionServicio = 0, 			          
				      (coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones fuera de CA
				  sum(iif(v.TipoFactura = 'COVE03' and dir.Pais not in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and v.ExportacionServicio = 1, 			          
				      (coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones de servicios
				  sum(iif(v.TipoFactura = 'COVE02' and v.GravadaTasaCero is null, 
				      (coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- ventas a zona franca y dpa con factura consumidor				  
				  sum(iif(v.TipoFactura in ('COVE02', 'COVE04', 'COVE05') And v.TerceroNoDomiciliado is not null, 
				      (coalesce(v.Gravada, 0) + coalesce(v.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0))  -- venta a cta de tercero no domiciliado
	        from Venta v
		   inner join FacAutorizacionDoc a
			  on v.AutorizacionDocumento = a.OID
		   inner join Tercero c
			  on v.Cliente = c.OID
			left join TerceroDocumento nit
			  on v.ClienteDocumento = nit.OID
			left join TerceroDocumento rc
			  on v.NRC = rc.OID
			left join TerceroDireccion dir
			  on c.DireccionPrincipal = dir.Oid
			left join FacCaja caja
			  on a.Caja = caja.Oid
			where v.Empresa = @Empresa
			  and Month(v.Fecha) = @Mes
			  and Year(v.Fecha) = @Anio
			  and v.Estado != 2 -- Anulado
			  and v.TipoFactura in ('COVE02', 'COVE03', 'COVE04', 'COVE05')
			group by v.Empresa, caja.NoCaja, v.Fecha, v.AutorizacionDocumento, v.TipoFactura
		  ) as origen
		  (Empresa, Correlativo, NoCaja, Fecha, AutorizacionDocumento, TipoFactura, NoControlDel, NoControlAl, NoDocumentoDel, 
		   NoDocumentoAl, Exenta, InternaExenta, NoSujeta, GravadaLocal, ExportacionCA, ExportacionFueraCA, 
		   ExportacionServicio, VentaZonaFranca, VentaCtaTercero)
		on dest.Empresa = origen.Empresa
	   and dest.Fecha = origen.Fecha
	   and dest.AutorizacionDocumento = origen.AutorizacionDocumento
	  when not matched by target then
	       insert (Empresa, Correlativo, Fecha, AutorizacionDocumento, TipoDocumento, NoControlInternoDel, NoControlInternoAl,
		           NoDocumentoDel, NoDocumentoAl, Exenta, InternaExenta, NoSujeta, GravadaLocal, ExportacionCA,
				   ExportacionFueraCA, ExportacionServicio, VentaZonaFranca, VentaTercero)
		   values (origen.Empresa, origen.Correlativo, origen.Fecha, origen.AutorizacionDocumento, origen.TipoFactura,
				   origen.NoControlDel, origen.NoControlAl, origen.NoDocumentoDel, origen.NoDocumentoAl, origen.Exenta, 
				   origen.InternaExenta, origen.NoSujeta, origen.GravadaLocal, origen.ExportacionCA, origen.ExportacionFueraCA, 
				   origen.ExportacionServicio, origen.VentaZonaFranca, origen.VentaCtaTercero)
	  when matched then
	     update set
		            Correlativo = origen.Correlativo,
					TipoDocumento = origen.TipoFactura,
					NoControlInternoDel = origen.NoControlDel,
					NoControlInternoAl  = origen.NoControlAl,
					NoDocumentoDel = origen.NoDocumentoDel,
					NoDocumentoAl  = origen.NoDocumentoAl,
					Exenta = origen.Exenta,
					InternaExenta = origen.InternaExenta,
					NoSujeta = origen.NoSujeta,
					GravadaLocal = origen.GravadaLocal,
					ExportacionCA = origen.ExportacionCA,
					ExportacionFueraCA = origen.ExportacionFueraCA,
					ExportacionServicio = origen.ExportacionServicio,
					VentaZonaFranca = origen.VentaZonaFranca,
					VentaTercero = origen.VentaCtaTercero,
					Empresa = origen.Empresa;
	delete x 
	  from LibroVentaConsumidor x
	 where x.Empresa = @Empresa 
	   and month(x.Fecha) = @Mes 
	   and year(x.Fecha) = @Anio
	   and not exists (select distinct Empresa, Fecha from Venta v
	                    where v.Empresa = x.Empresa 
				          and v.Fecha = x.Fecha
					      and v.TipoFactura in ('COVE02', 'COVE03', 'COVE04', 'COVE05')
					      and v.Estado != 2 -- anulado
					   )
  end
end
