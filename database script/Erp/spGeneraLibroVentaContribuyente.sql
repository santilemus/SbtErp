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
    begin tran t1  
	  -- agregamos la informacion de las ventas.
	  merge LibroVentaContribuyente as dest
	      using
		  (select v.Numero, v.Caja, v.Fecha, v.Moneda, v.ValorMoneda, v.AutorizacionDocumento, v.NoFactura,
				 v.Cliente, v.Exenta, v.NoSujeta, v.Gravada, v.IvaRetenido, v.IvaPercibido, v.Iva, 
				 v.Exenta + v.NoSujeta + v.Gravada + v.Iva + v.IvaPercibido - v.IvaRetenido as Total,
				 a.Resolucion, a.Serie, a.Clase, c.Nombre, nit.Numero as Nit, rc.Numero as Nrc, v.Oid, 
				 v.TerceroNoDomiciliado
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
		  (Numero, Caja, Fecha, Moneda, ValorMoneda, AutorizacionDocumento, NoFactura, Cliente, Exenta, NoSujeta, 
		   Gravada, IvaRetenido, IvaPercibido, Iva, Total, Resolucion, Serie, Clase, Nombre, Nit, Nrc, OidVenta, 
		   TerceroNoDomiciliado)
		on dest.Venta = origen.OidVenta
	   and dest.TipoDocumento = '03'
	  when not matched by target then
	       insert (FechaEmision, AutorizacionDocumento, TipoDocumento, Numero, NoControlInterno, Nit, Cliente,
		           Exenta,NoSujeta, GravadaLocal, DebitoFiscal, VentaTercero, DebitoFiscalVtaTercero, 
				   IvaPercibido, IvaRetenido, Venta)
		   values (origen.Fecha, origen.AutorizacionDocumento, '03', origen.NoFactura, 
		           iif(origen.Clase = 1, origen.NoFactura, origen.Numero), origen.Nit, origen.Cliente, 
				   origen.Exenta, origen.NoSujeta, 
				   iif(origen.TerceroNoDomiciliado is null, origen.Gravada, 0), 
				   iif(origen.TerceroNoDomiciliado is null, origen.Iva, 0), 
				   iif(origen.TerceroNoDomiciliado is not null, origen.Gravada, 0), 
				   iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0), 
				   origen.IvaPercibido, origen.IvaRetenido, origen.OidVenta)
	  when not matched by source then
	     Delete	
	  when matched then
	     update set
					FechaEmision = origen.Fecha,
		            AutorizacionDocumento = origen.AutorizacionDocumento,
					Numero = origen.NoFactura,
					NoControlInterno = iif(origen.Clase = 1, origen.NoFactura, origen.Numero),
		            Nit = origen.Nit,
					Cliente = origen.Cliente,
					Exenta = origen.Exenta,
					NoSujeta = origen.NoSujeta,
					GravadaLocal = iif(origen.TerceroNoDomiciliado is null, origen.Gravada, 0),
					DebitoFiscal = iif(origen.TerceroNoDomiciliado is null, origen.Iva, 0),
					VentaTercero = iif(origen.TerceroNoDomiciliado is not null, origen.Gravada, 0),
					DebitoFiscalVtaTercero = iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0),
					IvaPercibido = origen.IvaPercibido,
					IvaRetenido = origen.IvaRetenido;
	commit tran t1
  end try
  begin catch
    rollback tran t1
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
  -- insertamos la informacion de Notas de Credito y Debito
  begin try
    begin tran t2 
      -- agregamos la informacion de las cxc. Aqui se registran notas de credito y debito
	  -- pendiente de identificar como obtendremos el correlativo interno para notas de credito y debito
	  merge LibroVentaContribuyente as dest
	     using 
		 (select cd.Numero, cd.Fecha, cd.Moneda, cd.ValorMoneda, cc.AutorizacionDocumento, v.NoFactura,
		         v.Cliente, cd.Exenta, cd.Gravada, cd.Iva, cd.IvaPercibido, cd.IvaRetenido, cd.NoSujeta, 
				 cd.Exenta + cd.NoSujeta + cd.Gravada + cd.Iva + cd.IvaPercibido - cd.IvaRetenido as Total,
				 a.Resolucion, a.Serie, a.Clase, nit.Numero as Nit, rc.Numero as Nrc, cd.Venta, 
				 cd.Oid, iif(cc.Concepto != 17, '05', '06') as TipoDocumento, v.TerceroNoDomiciliado
		    from CxCTransaccion cc
		   inner join CxCDocumento cd
			  on cd.CxCTransaccion = cc.Oid
           inner join Venta v
		      on cd.Venta = v.Oid
		   inner join FacAutorizacionDoc a
			  on cc.AutorizacionDocumento = a.OID
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
			  and cc.Estado != 2
			  and cc.Concepto in (2, 3, 4, 17)  -- notas de credito: devolucion, descuento pronto pago, descuento prod. deteriorado, nota debito
		 ) as origen
		 (Numero, Fecha, Moneda, ValorMoneda, AutorizacionDocumento, NoFactura, Cliente, Exenta, Gravada,
		  Iva, IvaPercibido, IvaRetenido, NoSujeta, Total, Resolucion, Serie, Clase, Nit, Nrc, Venta, 
		  OidDocumento, TipoDocumento, TerceroNoDomiciliado)
	    on dest.Venta = origen.Venta
	   and dest.CxCDocumento = origen.OidDocumento
	  when not matched by target then
	       insert (FechaEmision, AutorizacionDocumento, TipoDocumento, Numero, NoControlInterno, Nit, 
		           Cliente, Exenta, NoSujeta, GravadaLocal, DebitoFiscal, VentaTercero, DebitoFiscalVtaTercero,
				   IvaPercibido, IvaRetenido, Venta, CxCDocumento)
		   values (origen.Fecha, origen.AutorizacionDocumento, origen.TipoDocumento, origen.Numero, 
		           iif(origen.Clase = 1, origen.Numero, oidDocumento), -- falta reemplazar oidDocumento por el numero de control interno 
				   origen.Nit, origen.Cliente, origen.Exenta, origen.NoSujeta, 
				   iif(origen.TerceroNoDomiciliado is null, origen.Gravada, 0), 
				   iif(origen.TerceroNoDomiciliado is null, origen.Iva, 0), 
				   iif(origen.TerceroNoDomiciliado is not null,  origen.Gravada, 0),
				   iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0),
				   origen.IvaPercibido, origen.IvaRetenido, origen.Venta, origen.OidDocumento)
	  when not matched by source then
	     Delete	
	  when matched then
	     update set
				FechaEmision = origen.Fecha,
				AutorizacionDocumento = origen.AutorizacionDocumento,
		        Numero = origen.Numero,
				NoControlInterno = iif(origen.Clase = 1, origen.NoFactura, OidDocumento), -- falta reeplazar OidDocumento
				Nit = origen.Nit,
				Cliente = origen.Cliente,
				Exenta = origen.Exenta, 
				NoSujeta = origen.NoSujeta,
				GravadaLocal = iif(origen.TerceroNoDomiciliado is null, origen.Gravada, 0.00),
				DebitoFiscal = iif(origen.TerceroNoDomiciliado is null, origen.Iva, 0.00),
				VentaTercero = iif(origen.TerceroNoDomiciliado is not null, origen.Gravada, 0.00),
				DebitoFiscalVtaTercero = iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0.00),
				IvaRetenido = origen.IvaRetenido,
				IvaPercibido= origen.IvaPercibido,
				Venta = origen.Venta,
				CxCDocumento = origen.OidDocumento;
	commit tran t2
  end try
  begin catch
    rollback tran t2
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
end
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Generar Libro de Ventas a Contribuyentes - IVA', 
     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'spGeneraLibroVentaContribuyente'
GO


