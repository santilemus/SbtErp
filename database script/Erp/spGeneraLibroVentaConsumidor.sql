USE [Sbt_Erp]
GO

IF EXISTS(SELECT 1 FROM sysobjects WHERE type = 'P' and name = 'spGeneraLibroVentaConsumidor')
BEGIN
   DROP PROCEDURE spGeneraLibroVentaConsumidor
END

/****** Object:  StoredProcedure [dbo].[spConCierreDiario]    Script Date: 9/4/2021 23:52:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 09/04/2021
-- Description:	Generar Libro de Ventas a Consumidor Final - IVA
-- ================================================================================================
CREATE procedure [dbo].[spGeneraLibroVentaConsumidor](
   @Empresa     int,
   @Moneda      varchar(3),
   @Mes         int,
   @Anio        int)
as
begin
  set nocount on
  declare @error_msg nvarchar(4000)
  declare @error_sev int;   

  declare @factorCambio numeric(12,2);
  select @factorCambio = m.FactorCambio from Moneda m
   where m.Codigo = @Moneda

  --- procesamos, la partida de apertura (si se encuentra en @fecha_desde)
  begin try
    begin tran t1  
	  -- agregamos la informacion de las ventas.
	  merge LibroVentaConsumidor as dest
	      using
		  (select caja.NoCaja, v.Fecha, v.AutorizacionDocumento, v.TipoFactura, min(v.Numero), max(v.Numero), min(v.NoFactura), 
		          max(v.NoFactura), sum(iif(c.TipoContribuyente = 0, 
				  coalesce(dv.Exenta, 0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)),   -- vta exenta a contribuyentes
				  -- vta exenta a misiones diplomaticas, o. internacionales
				  sum(iif(c.TipoContribuyente = 1, coalesce(dv.Exenta, 0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)),   
				  sum(coalesce(dv.NoSujeta, 0.0)/coalesce(v.ValorMoneda, 1.0) * @factorCambio), 
				  sum(iif(v.TipoFactura in ('COVE02', 'COVE04', 'COVE05') and v.TerceroNoDomiciliado is null, 
				      (coalesce(dv.Gravada, 0) + coalesce(dv.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- gravado local
				  sum(iif(v.TipoFactura = 'COVE03' and dir.Pais in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and cp.Clasificacion <=3, 			          
				      (coalesce(dv.Gravada, 0) + coalesce(dv.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones a CA
				  sum(iif(v.TipoFactura = 'COVE03' and dir.Pais not in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and cp.Clasificacion <= 3, 			          
				      (coalesce(dv.Gravada, 0) + coalesce(dv.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones fuera de CA
				  sum(iif(v.TipoFactura = 'COVE03' and dir.Pais not in ('CRI', 'GTM', 'HND', 'NIC', 'SLV') and cp.Clasificacion in (4, 5), 			          
				      (coalesce(dv.Gravada, 0) + coalesce(dv.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- exportaciones de servicios
				  sum(iif(v.TipoFactura = 'COVE02' and v.GravadaTasaCero is null, 
				      (coalesce(dv.Gravada, 0) + coalesce(dv.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0)), -- ventas a zona franca y dpa con factura consumidor				  
				  sum(iif(v.TipoFactura in ('COVE02', 'COVE04', 'COVE05') And v.TerceroNoDomiciliado is not null, 
				      (coalesce(dv.Gravada, 0) + coalesce(dv.Iva, 0))/coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0.0))  -- venta a cta de tercero no domiciliado
			from VentaDetalle dv
		   inner join Venta v
		      on dv.Venta = v.Oid
		   inner join Producto p
		      on dv.Producto = p.Oid
		   inner join ProCategoria cp
		      on p.Categoria = cp.Oid
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
			group by caja.NoCaja, v.Fecha, v.AutorizacionDocumento, v.TipoFactura
		  ) as origen
		  (NoCaja, Fecha, AutorizacionDocumento, TipoFactura, NoControlDel, NoControlAl, NoDocumentoDel, 
		   NoDocumentoAl, Exenta, InternaExenta, NoSujeta, GravadaLocal, ExportacionCA, ExportacionFueraCA, 
		   ExportacionServicio, VentaZonaFranca, VentaCtaTercero)
		on dest.FechaEmision = origen.Fecha
	   And dest.AutorizacionDocumento = origen.AutorizacionDocumento
	  when not matched by target then
	       insert (FechaEmision, AutorizacionDocumento, TipoDocumento, NoControlInternoDel, NoControlInternoAl,
		           NoDocumentoDel, NoDocumentoAl, Exenta, InternaExenta, NoSujeta, GravadaLocal, ExportacionCA,
				   ExportacionFueraCA, ExportacionServicio, VentaZonaFranca, VentaTercero, Moneda, ValorMoneda)
		   values (origen.Fecha, origen.AutorizacionDocumento, 
		           iif(origen.TipoFactura = 'COVE02','01', iif(origen.TipoFactura = 'COVE03', '11', 
				   iif(origen.TipoFactura = 'COVE04', '02', '10'))),
				   origen.NoControlDel, origen.NoControlAl, origen.NoDocumentoDel, origen.NoDocumentoAl, origen.Exenta, 
				   origen.InternaExenta, origen.NoSujeta, origen.GravadaLocal, origen.ExportacionCA, origen.ExportacionFueraCA, 
				   origen.ExportacionServicio, origen.VentaZonaFranca, origen.VentaCtaTercero, @Moneda, @factorCambio)
	  when not matched by source then
	     Delete	
	  when matched then
	     update set
					TipoDocumento = iif(origen.TipoFactura = 'COVE02','01', iif(origen.TipoFactura = 'COVE03', '11', 
				                    iif(origen.TipoFactura = 'COVE04', '02', '10'))),
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
					Moneda = @Moneda,
					ValorMoneda = @factorCambio;
	commit tran t1
  end try
  begin catch
    rollback tran t1
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
end
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Generar Libro de Ventas a Consumidor Final - IVA', 
     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'spGeneraLibroVentaConsumidor'
GO

grant execute on spGeneraLibroVentaConsumidor to public
go
