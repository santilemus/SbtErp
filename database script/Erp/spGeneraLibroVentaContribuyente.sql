USE [Sbt_Erp]
GO

/****** Object:  StoredProcedure [dbo].[spConCierreDiario]    Script Date: 9/4/2021 23:52:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 09/04/2021
-- Description:	Generar Libro de Ventas a Contribuyentes - IVA
-- ================================================================================================
CREATE procedure [dbo].[spGeneraLibroVentaContribuyente](
   @Empresa     int,
   @Mes         int,
   @Anio        int)
as
begin
  set nocount on
  declare @error_msg nvarchar(4000)
  declare @error_sev int;   

  --- procesamos, la partida de apertura (si se encuentra en @fecha_desde)
  begin try
    begin tran t2  
	  -- agregamos la informacion de las ventas. Falta aqui el ID de la venta, lo necesitamos
	  -- porque en un ambiente multiempresa es lo que hara diferente cada registro
	  merge LibroVentaContribuyente as dest
	      using
		  (select v.Numero, v.Caja, v.Fecha, v.Moneda, v.ValorMoneda, v.TipoFactura, v.AutorizacionDocumento, v.NoFactura,
				 v.Cliente, v.Exenta, v.NoSujeta, v.Gravada, v.IvaRetenido, v.IvaPercibido, v.Iva, 
				 v.Exenta + v.NoSujeta + v.Gravada + v.Iva + v.IvaPercibido - v.IvaRetenido as Total,
				 a.Resolucion, a.Serie, a.Clase, c.Nombre, nit.Numero as Nit, rc.Numero as Nrc
			from Venta v
		   inner join FacAutorizacionDoc a
			  on v.AutorizacionDocumento = a.OID
		   inner join Tercero c
			  on v.Cliente = c.OID
			left join TerceroDocumento nit
			  on v.ClienteDocumento = nit.OID
			left join TerceroDocumento rc
			  on v.NRC = rc.OID
			where v.Empresa = @Empresa
			  and Month(v.Fecha) = @Mes
			  and Year(v.Fecha) = @Anio
			  and v.Estado != 2 -- Anulado
			  and v.TipoFactura = 'COVE01' -- comprobantes de credito fiscal
		  ) as origen
		  (Numero, Caja, Fecha, Moneda, ValorMoneda, TipoFactura, AutorizacionDocumento, NoFactura,
		   Cliente, Exenta, NoSujeta, Gravada, IvaRetenido, IvaPercibido, Iva, Total, 
		   Resolucion, Serie, Clase, Nombre, Nit, Nrc)
		on dest.NoControlInterno = origen.Numero
	   and dest.Nit = origen.Nit
	   and dest.FechaEmision = origen.Fecha
	   and dest.Numero = origen.NoFactura
	  when not matched by target then
	       insert (FechaEmision, ClaseDocumento, TipoDocumento, NoResolucion, NoSerie, Numero, NoControlInterno,
		           Nit, RazonSocial, VentaExenta, VentaNoSujeta, VentaGravadaLocal, DebitoFiscal, VentaTercero,
				   DebitoFiscalVtaTercero, TotalVenta)
		   values (origen.Fecha, origen.Clase, '03', origen.Resolucion, origen.Serie, origen.Nofactura, origen.Numero,
		           origen.Nit, origen.Nombre, origen.Exenta, origen.NoSujeta, origen.Gravada, origen.Iva, 0, 0, 
				   origen.Total)
	  when not matched by source then
	     Delete	
	  when matched then
	     update set
		            Nit = origen.Nit,
					RazonSocial = origen.Nombre,
					VentaExenta = origen.Exenta,
					VentaNoSujeta = origen.NoSujeta,
					VentaGravadaLocal = origen.Gravada,
					DebitoFiscal = origen.Iva,
					VentaTercero = 0.0,
					DebitoFiscalVtaTercero = 0.0,
					TotalVenta = origen.Total;
	commit tran t2
  end try
  begin catch
    rollback tran t2
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
  -- insertamos la informacion de Notas de Credito y Debito
end
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Generar Libro de Ventas a Contribuyentes - IVA', 
     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'spGeneraLibroVentaContribuyente'
GO


