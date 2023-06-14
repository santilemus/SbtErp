USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spGeneraLibroVentaContribuyente]    Script Date: 9/6/2023 23:49:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 09/04/2021
-- Description:	Generar Libro de Ventas a Contribuyentes - IVA
-- Modificaciones
-- 13/junio/2023
--  1. Se agrega la columna de correlativo para cumplir con el reglamento de aplicación del código tributario
--
-- 09/junio/2023 por SELM
--  1. Se elimina del select del merge la tabla VentaDetalle y la tabla principal ahora es Venta
--  2. Para resolver las exportaciones de servicio (se utilizaba la categoria del producto via la
--     tabla VentaDetalle) se agrego la columa ExportacionServicio a la tabla Venta
--  3. Ahoras las columnas de la tabla Venta son las que se utilizan para obtener los montos
--  4. Se quitan las columnas de Moneda y ValorMoneda
--  6. Se quita del merge el delete porque se probo en la compra que eso es problematico hay que
--     incluir condiciones para que sea valido (y la columna Empresa se requiere para evitar borrar
--     los datos de otras empresas y esa columna no existe en la tabla (porque esta en la tabla Venta)
--  7. Se agrega la sentencia separada para borrar los registros que no existen en Venta pero que 
--     estan en el libro (cuandno es una segunda o posterior generacion del libro)
--  8. Se agrega el insert cuando no hay registros en el periodo para optimizar y evitar el merge

-- ================================================================================================
create or alter procedure [dbo].[spGeneraLibroVentaContribuyente](
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
	 and v.TipoFactura in ('COVE01') -- comprobantes de credito fiscal
  --- procesamos, la informacion de las ventas
  select count(*) from LibroVentaContribuyente x
   inner join Venta v
      on x.Venta = v.Oid
   where v.Empresa = @Empresa
     and month(x.Fecha) = @Mes 
	 and year(x.Fecha)  = @Anio 

  if (@@rowcount = 0)
  begin
    insert into LibroVentaContribuyente 
	      (Correlativo, Fecha, AutorizacionDocumento, TipoDocumento, Numero, NoControlInterno, Nit, Cliente, Exenta, NoSujeta, 
		   GravadaLocal, DebitoFiscal, IvaPercibido, IvaRetenido, VentaTercero, DebitoFiscalTercero, Venta, Cerrado)
	select @CorrInicio + row_number() over(partition by v.Empresa, Year(v.Fecha) order by v.Fecha),
	       v.Fecha, v.AutorizacionDocumento, '03', v.NoFactura, 
	       iif(a.Clase = 2, v.NoFactura, v.Numero), nit.Numero, v.Cliente,
		   coalesce(v.Exenta, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
		   coalesce(v.NoSujeta, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
		   iif(v.TerceroNoDomiciliado is null, coalesce(v.Gravada, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0),
		   iif(v.TerceroNoDomiciliado is null, coalesce(v.Iva, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0),
		   coalesce(v.IvaPercibido, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
		   coalesce(v.IvaRetenido, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 	
		   iif(v.TerceroNoDomiciliado is not null, coalesce(v.Gravada, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0),
		   iif(v.TerceroNoDomiciliado is not null, coalesce(v.Iva, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 0),		   
		    v.Oid, 0
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
		   and v.TipoFactura in ('COVE01') -- comprobantes de credito fiscal, facturas de exportacion
  end
  else
  begin
	  merge LibroVentaContribuyente as dest
	      using
		  (select @CorrInicio + row_number() over(partition by v.Empresa, Year(v.Fecha) order by v.Fecha),
		         v.Numero, v.Caja, v.Fecha, v.AutorizacionDocumento, v.NoFactura,
				 v.Cliente, coalesce(v.Exenta, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(v.NoSujeta, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(v.Gravada, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio,
				 coalesce(v.IvaRetenido, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(v.IvaPercibido, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(v.Iva, 0.0) / coalesce(v.ValorMoneda, 1.0) * @factorCambio, 
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
			  and v.TipoFactura in ('COVE01') -- comprobantes de credito fiscal
		  ) as origen
		  (Correlativo, Numero, Caja, Fecha, AutorizacionDocumento, NoFactura, Cliente, Exenta, NoSujeta, 
		   Gravada, IvaRetenido, IvaPercibido, Iva, Resolucion, Serie, Clase, Nombre, Nit, Nrc, OidVenta, 
		   TerceroNoDomiciliado)
		on dest.Venta = origen.OidVenta
	   and dest.TipoDocumento = '03'
	  when not matched by target then
	       insert (Correlativo, Fecha, AutorizacionDocumento, TipoDocumento, Numero, NoControlInterno, Nit, Cliente,
		           Exenta,NoSujeta, GravadaLocal, DebitoFiscal, VentaTercero, DebitoFiscalTercero, 
				   IvaPercibido, IvaRetenido, Venta, Cerrado)
		   values (origen.Correlativo, origen.Fecha, origen.AutorizacionDocumento, '03', origen.NoFactura, 
		           iif(origen.Clase = 2, origen.NoFactura, origen.Numero), origen.Nit, origen.Cliente, 
				   origen.Exenta, origen.NoSujeta, 
				   iif(origen.TerceroNoDomiciliado is null, origen.Gravada, 0),       -- gravada local 
				   iif(origen.TerceroNoDomiciliado is null, origen.Iva, 0),           -- debito fiscal
				   iif(origen.TerceroNoDomiciliado is not null, origen.Gravada, 0),   -- Venta Tercero
				   iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0),       -- DebitoFiscalVtaTercero 
				   origen.IvaPercibido, origen.IvaRetenido, origen.OidVenta, 0)
	  when matched then
	     update set
		            Correlativo = origen.Correlativo,
					Fecha = origen.Fecha,
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
					DebitoFiscalTercero = iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0),
					IvaPercibido = origen.IvaPercibido,
					IvaRetenido = origen.IvaRetenido,
					Cerrado = 0;
	  -- borrar los registros cuando en una generacion posterior no existen en las Ventas (porque fueron eliminados)
	  delete x
		from LibroVentaContribuyente x	  
	   where not exists (select Oid from Venta v where v.Oid = x.Venta)
	     and CxCDocumento is null
  end;

  -- insertamos la informacion de Notas de Credito y Debito
  -- agregamos la informacion de las cxc. Aqui se registran notas de credito y debito
  -- pendiente de identificar como obtendremos el correlativo interno para notas de credito y debito
  -- PENDIENTE, implementar el correlativo como se hace con las facturas
	  merge LibroVentaContribuyente as dest
	     using 
		 (select cc.Numero, cc.Fecha, cc.AutorizacionDocumento, v.NoFactura, v.Cliente, 
				 coalesce(cc.Exenta, 0.0) / coalesce(cc.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(cc.Gravada, 0.0) / coalesce(cc.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(cc.Iva, 0.0) / coalesce(cc.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(cc.IvaPercibido, 0.0) / coalesce(cc.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(cc.IvaRetenido, 0.0) / coalesce(cc.ValorMoneda, 1.0) * @factorCambio, 
				 coalesce(cc.NoSujeta, 0.0) / coalesce(cc.ValorMoneda, 1.0) * @factorCambio, 
				 a.Resolucion, a.Serie, a.Clase, nit.Numero as Nit, rc.Numero as Nrc, cc.Venta, 
				 cc.Oid, iif(cc.Tipo != 17, '05', '06') as TipoDocumento, v.TerceroNoDomiciliado
		    from CxCTransaccion cc
           inner join Venta v
		      on cc.Venta = v.Oid
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
			  and cc.Tipo in (2, 3, 4, 17)  -- notas de credito: devolucion, descuento pronto pago, descuento prod. deteriorado, nota debito
		 ) as origen
		 (Numero, Fecha, AutorizacionDocumento, NoFactura, Cliente, Exenta, Gravada,
		  Iva, IvaPercibido, IvaRetenido, NoSujeta, Resolucion, Serie, Clase, Nit, Nrc, Venta, 
		  OidDocumento, TipoDocumento, TerceroNoDomiciliado)
	    on dest.Venta = origen.Venta
	   and dest.CxCDocumento = origen.OidDocumento
	  when not matched by target then
	       insert (Fecha, AutorizacionDocumento, TipoDocumento, Numero, NoControlInterno, Nit, 
		           Cliente, Exenta, NoSujeta, GravadaLocal, DebitoFiscal, VentaTercero, DebitoFiscalTercero,
				   IvaPercibido, IvaRetenido, Venta, CxCDocumento, Cerrado)
		   values (origen.Fecha, origen.AutorizacionDocumento, origen.TipoDocumento, origen.Numero, 
		           iif(origen.Clase = 1, origen.Numero, oidDocumento), -- falta reemplazar oidDocumento por el numero de control interno 
				   origen.Nit, origen.Cliente, origen.Exenta, origen.NoSujeta, 
				   iif(origen.TerceroNoDomiciliado is null, origen.Gravada, 0), 
				   iif(origen.TerceroNoDomiciliado is null, origen.Iva, 0), 
				   iif(origen.TerceroNoDomiciliado is not null,  origen.Gravada, 0),
				   iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0),
				   origen.IvaPercibido, origen.IvaRetenido, origen.Venta, origen.OidDocumento, 1)
	  when matched then
	     update set
				Fecha = origen.Fecha,
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
				DebitoFiscalTercero = iif(origen.TerceroNoDomiciliado is not null, origen.Iva, 0.00),
				IvaRetenido = origen.IvaRetenido,
				IvaPercibido= origen.IvaPercibido,
				Venta = origen.Venta,
				CxCDocumento = origen.OidDocumento,
				Cerrado = 0;
end


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Generar Libro de Ventas a Contribuyentes - IVA', 
     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'spGeneraLibroVentaContribuyente'
